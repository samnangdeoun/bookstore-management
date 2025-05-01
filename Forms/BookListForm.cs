using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement
{
    public partial class BookListForm : Form
    {
        private BookStoreDBDataContext db;
        private DataGridView dgvBooks;
        private Button btnAdd, btnEdit, btnDelete;

        public BookListForm()
        {
            this.Text = "Book List";
            this.Size = new Size(900, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            db = new BookStoreDBDataContext();
            InitializeControls();
            LoadBooks();
        }

        private void InitializeControls()
        {
            dgvBooks = new DataGridView
            {
                Top = 20,
                Left = 20,
                Width = 840,
                Height = 350,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvBooks);

            btnAdd = new Button { Text = "Add", Top = 390, Left = 20, Width = 100 };
            btnEdit = new Button { Text = "Edit", Top = 390, Left = 140, Width = 100 };
            btnDelete = new Button { Text = "Delete", Top = 390, Left = 260, Width = 100 };

            btnAdd.Click += btnAdd_Click;
            btnEdit.Click += btnEdit_Click;
            btnDelete.Click += btnDelete_Click;

            this.Controls.Add(btnAdd);
            this.Controls.Add(btnEdit);
            this.Controls.Add(btnDelete);
        }

        private void LoadBooks()
        {
            var books = db.Books.Select(b => new
            {
                b.Id,
                b.Name,
                Author = b.AuthorName,
                Genre = b.Genre.Name,
                b.SalePrice,
                IsSequel = (bool)b.IsSequel ? "Yes" : "No"
            }).ToList();

            dgvBooks.DataSource = books;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var form = new Forms.BookForm();
            form.ShowDialog();
            LoadBooks();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvBooks.CurrentRow != null)
            {
                int id = (int)dgvBooks.CurrentRow.Cells["Id"].Value;
                var book = db.Books.FirstOrDefault(b => b.Id == id);

                if (book != null)
                {
                    var form = new Forms.BookForm(book);
                    form.ShowDialog();
                    LoadBooks();
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
                    var result = MessageBox.Show($"Are you sure you want to delete \"{book.Name}\"?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        db.Books.DeleteOnSubmit(book);
                        db.SubmitChanges();
                        LoadBooks();
                    }
                }
            }
        }
    }
}
