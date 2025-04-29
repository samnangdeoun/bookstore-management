using System;
using System.Linq;
using System.Data;
using System.Data.Linq;
using System.Windows.Forms;
using BookstoreManagement.Data;

namespace BookstoreManagement.Forms
{
    public partial class BookForm : Form
    {
        BookstoreDataDataContext db = new BookstoreDataDataContext();
        Book editingBook = null;

        public BookForm(Book book = null)
        {
            InitializeComponent();
            LoadGenres();
            if (book != null)
            {
                editingBook = book;
                txtName.Text = book.Name;
                txtAuthor.Text = book.AuthorFullName;
                txtPublisher.Text = book.PublishingHouse;
                numPages.Value = book.PageCount;
                cmbGenre.SelectedValue = book.GenreId;
                dtpPublishDate.Value = book.PublishDate;
                numPrime.Value = book.PrimeCost;
                numSale.Value = book.SalePrice;
                chkSequel.Checked = book.IsSequel;
            }
        }

        private void LoadGenres()
        {
            cmbGenre.DataSource = db.Genres.ToList();
            cmbGenre.DisplayMember = "Name";
            cmbGenre.ValueMember = "Id";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (editingBook == null)
            {
                editingBook = new Book();
                db.Books.InsertOnSubmit(editingBook);
            }

            editingBook.Name = txtName.Text;
            editingBook.AuthorFullName = txtAuthor.Text;
            editingBook.PublishingHouse = txtPublisher.Text;
            editingBook.PageCount = (int)numPages.Value;
            editingBook.GenreId = (int)cmbGenre.SelectedValue;
            editingBook.PublishDate = dtpPublishDate.Value;
            editingBook.PrimeCost = numPrime.Value;
            editingBook.SalePrice = numSale.Value;
            editingBook.IsSequel = chkSequel.Checked;

            db.SubmitChanges();
            this.Close();
        }
    }
}
