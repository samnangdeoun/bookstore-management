using System;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class SellForm : Form
    {
        private BookstoreDataDataContext db;

        public SellForm()
        {
            InitializeComponent();
            db = new BookstoreDataDataContext();
            LoadBooks();
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
                decimal totalPrice = book.SalePrice * quantity;
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
