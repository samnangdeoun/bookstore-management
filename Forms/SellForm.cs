using BookstoreManagement.Data;
using BookstoreManagement.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class SellForm : Form
    {
        private ComboBox cmbBooks;
        private NumericUpDown nudQuantity;
        private Button btnAddToCart;
        private DataGridView dgvBooks;
        private Label lblGrandTotal;
        private Button btnSell;

        public SellForm()
        {
            this.Text = "Sell Books";
            this.Size = new Size(800, 550);
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeControls();
            LoadBooks();
        }

        private void InitializeControls()
        {
            // Book selection label
            var lblSelect = new Label
            {
                Text = "Select Book:",
                Top = 10,
                Left = 20,
                Width = 100
            };
            this.Controls.Add(lblSelect);

            // Book dropdown
            cmbBooks = new ComboBox
            {
                Top = 10,
                Left = 130,
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbBooks);

            // Quantity label
            var lblQty = new Label
            {
                Text = "Quantity:",
                Top = 10,
                Left = 450,
                Width = 80
            };
            this.Controls.Add(lblQty);

            // Quantity selector
            nudQuantity = new NumericUpDown
            {
                Top = 10,
                Left = 530,
                Width = 60,
                Minimum = 1,
                Maximum = 999
            };
            this.Controls.Add(nudQuantity);

            // Add to cart button
            btnAddToCart = new Button
            {
                Text = "Add to Cart",
                Top = 8,
                Left = 610,
                Width = 120
            };
            btnAddToCart.Click += btnAddToCart_Click;
            this.Controls.Add(btnAddToCart);

            // DataGridView for cart
            dgvBooks = new DataGridView
            {
                Top = 50,
                Left = 20,
                Width = 740,
                Height = 350,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };
            dgvBooks.Columns.Add("BookId", "BookId");
            dgvBooks.Columns["BookId"].Visible = false;
            dgvBooks.Columns.Add("BookName", "Book Name");
            dgvBooks.Columns.Add("SalePrice", "Price");
            dgvBooks.Columns.Add("Quantity", "Quantity");
            dgvBooks.Columns.Add("Discount", "Discount");
            dgvBooks.Columns.Add("Total", "Total");

            dgvBooks.CellEndEdit += dgvBooks_CellEndEdit;
            this.Controls.Add(dgvBooks);

            // Grand total label
            lblGrandTotal = new Label
            {
                Text = "Total: $0.00",
                Top = 410,
                Left = 540,
                Width = 220,
                Height = 30,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight
            };
            this.Controls.Add(lblGrandTotal);

            // Sell button
            btnSell = new Button
            {
                Text = "Sell Selected",
                Top = 450,
                Left = 20,
                Width = 740,
                Height = 40
            };
            btnSell.Click += btnSell_Click;
            this.Controls.Add(btnSell);
        }

        private void LoadBooks()
        {
            using (var db = new BookStoreContext())
            {
                var books = db.Books.Select(b => new
                {
                    b.Id,
                    b.Name,
                    b.SalePrice,
                    b.GenreId
                }).ToList();

                cmbBooks.DataSource = books;
                cmbBooks.DisplayMember = "Name";
                cmbBooks.ValueMember = "Id";
            }

            dgvBooks.Rows.Clear();
            lblGrandTotal.Text = "Total: $0.00";
        }

        private void btnAddToCart_Click(object sender, EventArgs e)
        {
            if (cmbBooks.SelectedItem == null) return;

            var selected = (dynamic)cmbBooks.SelectedItem;
            int bookId = selected.Id;
            string name = selected.Name;
            decimal price = selected.SalePrice;
            int quantity = (int)nudQuantity.Value;

            // Check if book is already in cart
            foreach (DataGridViewRow row in dgvBooks.Rows)
            {
                if (int.Parse(row.Cells["BookId"].Value.ToString()) == bookId)
                {
                    int existingQty = int.Parse(row.Cells["Quantity"].Value.ToString());
                    row.Cells["Quantity"].Value = existingQty + quantity;
                    RecalculateRow(row);
                    UpdateGrandTotal();
                    return;
                }
            }

            // Get discount
            decimal discount = 0;
            using (var db = new BookStoreContext())
            {
                var book = db.Books.Find(bookId);
                var disc = db.Discounts.FirstOrDefault(d =>
                    d.GenreId == book.GenreId &&
                    d.StartDate <= DateTime.Now &&
                    d.EndDate >= DateTime.Now);

                if (disc != null)
                {
                    discount = (price * disc.Percentage) / 100;
                }
            }

            decimal total = (price - discount) * quantity;

            dgvBooks.Rows.Add(bookId, name, price.ToString("F2"), quantity, discount.ToString("F2"), total.ToString("F2"));
            UpdateGrandTotal();
        }

        private void dgvBooks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvBooks.Columns["Quantity"].Index)
            {
                var row = dgvBooks.Rows[e.RowIndex];
                RecalculateRow(row);
                UpdateGrandTotal();
            }
        }

        private void RecalculateRow(DataGridViewRow row)
        {
            int quantity = int.TryParse(row.Cells["Quantity"].Value?.ToString(), out var q) ? q : 0;
            decimal price = decimal.TryParse(row.Cells["SalePrice"].Value?.ToString(), out var p) ? p : 0;
            decimal discount = decimal.TryParse(row.Cells["Discount"].Value?.ToString(), out var d) ? d : 0;

            decimal total = (price - discount) * quantity;
            row.Cells["Total"].Value = total.ToString("F2");
        }

        private void UpdateGrandTotal()
        {
            decimal grandTotal = 0;
            foreach (DataGridViewRow row in dgvBooks.Rows)
            {
                if (decimal.TryParse(row.Cells["Total"].Value?.ToString(), out decimal total))
                {
                    grandTotal += total;
                }
            }

            lblGrandTotal.Text = $"Total: {grandTotal:C2}";
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            using (var db = new BookStoreContext())
            {
                var sales = dgvBooks.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => int.TryParse(r.Cells["Quantity"].Value?.ToString(), out int q) && q > 0)
                    .Select(row =>
                    {
                        int quantity = int.Parse(row.Cells["Quantity"].Value.ToString());
                        int bookId = int.Parse(row.Cells["BookId"].Value.ToString());
                        decimal totalPrice = decimal.Parse(row.Cells["Total"].Value.ToString());
                        decimal discountAmount = decimal.Parse(row.Cells["Discount"].Value.ToString()) * quantity;

                        return new Sale
                        {
                            BookId = bookId,
                            Quantity = quantity,
                            TotalPrice = totalPrice,
                            DiscountAmount = discountAmount,
                            DateSold = DateTime.Now
                        };
                    })
                    .ToList();

                if (!sales.Any())
                {
                    MessageBox.Show("Please add books to the cart.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal total = sales.Sum(s => s.TotalPrice);
                var confirm = MessageBox.Show($"Are you sure you want to sell selected books?\nTotal: {total:C2}",
                    "Confirm Sale", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                db.Sales.AddRange(sales);
                db.SaveChanges();

                MessageBox.Show($"Sales completed. Grand Total: {total:C2}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadBooks(); // Reset form
            }
        }
    }
}
