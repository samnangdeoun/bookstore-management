using System;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement
{
    public partial class BookListForm : Form
    {
        private BookstoreDataDataContext db;

        public BookListForm()
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
                Author = b.AuthorFullName,
                b.Genre.Name,
                b.PublishingHouse,
                b.Pages,
                b.DatePublished,
                b.SalePrice,
                b.IsSequel
            }).ToList();

            dgvBooks.DataSource = books;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new BookForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                LoadBooks();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvBooks.CurrentRow != null)
            {
                int id = (int)dgvBooks.CurrentRow.Cells["Id"].Value;
                var book = db.Books.FirstOrDefault(b => b.Id == id);

                if (book != null)
                {
                    var form = new BookForm(book);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        LoadBooks();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvBooks.CurrentRow != null)
            {
                int id = (int)dgvBooks.CurrentRow.Cells["Id"].Value;
                var book = db.Books.FirstOrDefault(b => b.Id == id);

                if (book != null)
                {
                    db.Books.DeleteOnSubmit(book);
                    db.SubmitChanges();
                    LoadBooks();
                }
            }
        }
    }
}
