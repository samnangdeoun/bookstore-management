using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.Json;

namespace BookstoreManagement.Forms
{
    public partial class BookForm : Form
    {
        private BookStoreDBDataContext db = new BookStoreDBDataContext();
        private Book editingBook = null;

        private TextBox txtName, txtAuthor, txtPublisher;
        private NumericUpDown numPages, numPrime, numSale;
        private ComboBox cmbGenre;
        private DateTimePicker dtpPublishDate;
        private CheckBox chkSequel;
        private Button btnSave;

        public BookForm(Book book = null)
        {
            this.Text = book == null ? "Add Book" : "Edit Book";
            this.Size = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeControls();
            LoadGenres();

            if (book != null)
            {
                MessageBox.Show(book.Name + " Book Name");
                editingBook = book;
                txtName.Text = book.Name;
                txtAuthor.Text = book.AuthorName;
                numPages.Value = (decimal)book.Pages;
                cmbGenre.SelectedValue = book.GenreId;
                dtpPublishDate.Value = (DateTime)book.DatePublished;
                numPrime.Value = (decimal)book.PrimeCost;
                numSale.Value = (decimal)book.SalePrice;
                chkSequel.Checked = (bool)book.IsSequel;
            }
        }

        private void InitializeControls()
        {
            int top = 20;

            Label CreateLabel(string text)
            {
                return new Label { Text = text, Top = top, Left = 20, Width = 120 };
            }

            Control AddControl(Control control)
            {
                control.Top = top;
                control.Left = 150;
                control.Width = 200;
                this.Controls.Add(control);
                top += 40;
                return control;
            }

            this.Controls.Add(CreateLabel("Book Name:"));
            txtName = (TextBox)AddControl(new TextBox());

            this.Controls.Add(CreateLabel("Author:"));
            txtAuthor = (TextBox)AddControl(new TextBox());

            this.Controls.Add(CreateLabel("Publisher:"));
            txtPublisher = (TextBox)AddControl(new TextBox());

            this.Controls.Add(CreateLabel("Pages:"));
            numPages = (NumericUpDown)AddControl(new NumericUpDown { Minimum = 1, Maximum = 10000 });

            this.Controls.Add(CreateLabel("Genre:"));
            cmbGenre = (ComboBox)AddControl(new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList });

            this.Controls.Add(CreateLabel("Publish Date:"));
            dtpPublishDate = (DateTimePicker)AddControl(new DateTimePicker());

            this.Controls.Add(CreateLabel("Prime Cost:"));
            numPrime = (NumericUpDown)AddControl(new NumericUpDown { Minimum = 0, Maximum = 1000000, DecimalPlaces = 2 });

            this.Controls.Add(CreateLabel("Sale Price:"));
            numSale = (NumericUpDown)AddControl(new NumericUpDown { Minimum = 0, Maximum = 1000000, DecimalPlaces = 2 });

            this.Controls.Add(CreateLabel("Is Sequel:"));
            chkSequel = (CheckBox)AddControl(new CheckBox());

            btnSave = new Button { Text = "Save", Top = top + 10, Left = 150, Width = 100 };
            btnSave.Click += btnSave_Click;
            this.Controls.Add(btnSave);
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
            editingBook.AuthorName = txtAuthor.Text;
            editingBook.Pages = (int)numPages.Value;
            editingBook.GenreId = (int)cmbGenre.SelectedValue;
            editingBook.DatePublished = dtpPublishDate.Value;
            editingBook.PrimeCost = numPrime.Value;
            editingBook.SalePrice = numSale.Value;
            editingBook.IsSequel = chkSequel.Checked;

            db.SubmitChanges();
            this.Close();
        }
    }
}
