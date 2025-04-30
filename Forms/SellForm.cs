using System;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class SellForm : Form
    {
        private BookstoreDataDataContext db;

        private ComboBox cmbBooks;
        private NumericUpDown nudQuantity;
        private Button btnSell;

        public SellForm()
        {
            this.Text = "Sell Books";
            this.Size = new System.Drawing.Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            db = new BookstoreDataDataContext();
            InitializeControls();
            LoadBooks();
        }

        private void InitializeControls()
        {
            int top = 20;

            Label CreateLabel(string text)
            {
                return new Label
                {
                    Text = text,
                    Top = top,
                    Left = 20,
                    Width = 120
                };
            }

            Control AddControl(Control control)
            {
                control.Top = top;
                control.Left = 160;
                control.Width = 180;
                this.Controls.Add(control);
                top += 40;
                return control;
            }

            this.Controls.Add(CreateLabel("Select Book:"));
            cmbBooks = (ComboBox)AddControl(new ComboBox());

            this.Controls.Add(CreateLabel("Quantity:"));
            nudQuantity = (NumericUpDown)AddControl(new NumericUpDown { Minimum = 1 });

            btnSell = new Button
            {
                Text = "Sell",
                Top = top + 10,
                Left = 160,
                Width = 180
            };
            btnSell.Click += btnSell_Click;
            this.Controls.Add(btnSell);
        }

        private void LoadBooks()
        {
            var books = db.Books.Select(b => new
            {
                b.Id,
                b.Name,
                b.AuthorFullName,
                b.SalePrice,
                b.Pages
            }).ToList();

            cmbBooks.DataSource = books;
            cmbBooks.DisplayMember = "Name";
            cmbBooks.ValueMember = "Id";
        }

        private void btnSell_Click(object sender, EventArgs e)
        {
            int bookId = (int)cmbBooks.SelectedValue;
            int quantity = (int)nudQuantity.Value;
            var book = db.Books.FirstOrDefault(b => b.Id == bookId);

            if (book != null && quantity > 0)
            {
                decimal totalPrice = (decimal)(book.SalePrice * quantity);
                var sale = new Sale
                {
                    BookId = bookId,
                    Quantity = quantity,
                    TotalPrice = totalPrice,
                    DateSold = DateTime.Now
                };

                db.Sales.InsertOnSubmit(sale);
                db.SubmitChanges();

                MessageBox.Show($"Sale completed. Total: ${totalPrice}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Invalid selection or quantity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
