using BookstoreManagement.Data;
using BookstoreManagement.Models;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class SellForm : Form
    {
        private DataGridView dgvBooks;
        private Button btnSell;
        private Label lblGrandTotal;

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
            dgvBooks = new DataGridView
            {
                Dock = DockStyle.Top,
                Height = 370,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };

            dgvBooks.Columns.Add("BookId", "BookId");
            dgvBooks.Columns["BookId"].Visible = false;
            dgvBooks.Columns.Add("BookName", "Book Name");
            dgvBooks.Columns.Add("AuthorName", "Author");
            dgvBooks.Columns.Add("SalePrice", "Price");
            dgvBooks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Quantity" });
            dgvBooks.Columns.Add("Discount", "Discount");
            dgvBooks.Columns.Add("Total", "Total");

            dgvBooks.CellEndEdit += dgvBooks_CellEndEdit;

            this.Controls.Add(dgvBooks);

            lblGrandTotal = new Label
            {
                Text = "Total: $0.00",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 10, 10, 10),
                Height = 40
            };
            this.Controls.Add(lblGrandTotal);

            btnSell = new Button
            {
                Text = "Sell Selected",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            btnSell.Click += btnSell_Click;
            this.Controls.Add(btnSell);
        }

        private void LoadBooks()
        {
            dgvBooks.Rows.Clear();

            using (BookStoreContext db = new BookStoreContext())
            {
                var books = db.Books.Select(b => new
                {
                    b.Id,
                    b.Name,
                    b.AuthorName,
                    b.SalePrice,
                    b.GenreId
                }).ToList();

                foreach (var book in books)
                {
                    dgvBooks.Rows.Add(book.Id, book.Name, book.AuthorName, book.SalePrice.ToString("F2"), 0, "0", "0");
                }
            }

            lblGrandTotal.Text = "Total: $0.00";
        }

        private void dgvBooks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dgvBooks.Columns["Quantity"].Index)
            {
                var row = dgvBooks.Rows[e.RowIndex];
                if (int.TryParse(row.Cells["Quantity"].Value?.ToString(), out int quantity) && quantity > 0)
                {
                    decimal price = decimal.Parse(row.Cells["SalePrice"].Value.ToString());
                    int bookId = int.Parse(row.Cells["BookId"].Value.ToString());

                    using (var db = new BookStoreContext())
                    {
                        var book = db.Books.Find(bookId);
                        var discount = db.Discounts.FirstOrDefault(d =>
                            d.GenreId == book.GenreId &&
                            d.StartDate <= DateTime.Now &&
                            d.EndDate >= DateTime.Now);

                        decimal discountAmount = discount != null
                            ? (price * discount.Percentage) / 100
                            : 0;

                        decimal total = (price - discountAmount) * quantity;

                        row.Cells["Discount"].Value = discountAmount.ToString("F2");
                        row.Cells["Total"].Value = total.ToString("F2");
                    }
                }
                else
                {
                    row.Cells["Discount"].Value = "0";
                    row.Cells["Total"].Value = "0";
                }

                UpdateGrandTotal();
            }
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
            using (BookStoreContext db = new BookStoreContext())
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
                    MessageBox.Show("Please enter quantities to sell.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
