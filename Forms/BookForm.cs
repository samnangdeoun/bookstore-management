using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BookstoreManagement.Data;
using BookstoreManagement.Models;

namespace BookstoreManagement.Forms
{
    public partial class BookForm : Form
    {
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
                editingBook = book;
                txtName.Text = book.Name;
                txtAuthor.Text = book.AuthorName;
                txtPublisher.Text = book.PublisherName;
                numPages.Value = (decimal)book.Pages;
                cmbGenre.SelectedValue = book.GenreId;
                dtpPublishDate.Value = book.DatePublished != default ? book.DatePublished : DateTime.Now;
                numPrime.Value = book.PrimeCost != default ? book.PrimeCost : 0m;
                numSale.Value = book.SalePrice != default ? book.SalePrice : 0m;
                chkSequel.Checked = book.IsSequel;
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
            using (BookStoreContext db = new BookStoreContext())
            {
                cmbGenre.DataSource = db.Genres.ToList();
                cmbGenre.DisplayMember = "Name";
                cmbGenre.ValueMember = "Id";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (BookStoreContext db = new BookStoreContext())
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txtName.Text))
                    {
                        MessageBox.Show("Book name is required!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (editingBook == null)
                    {
                        editingBook = new Book();
                        db.Books.Add(editingBook);
                        db.SaveChanges();
                    }

                    // FIX: Use ID comparison instead of db.Books.Contains(editingBook)
                    if (!db.Books.Local.Any(b => b.Id == editingBook.Id))
                    {
                        db.Books.Attach(editingBook);
                    }

                    // Update all properties
                    editingBook.Name = txtName.Text;
                    editingBook.AuthorName = txtAuthor.Text;
                    editingBook.PublisherName = txtPublisher.Text;
                    editingBook.Pages = (int)numPages.Value;
                    editingBook.GenreId = (int)cmbGenre.SelectedValue;
                    editingBook.DatePublished = dtpPublishDate.Value;
                    editingBook.PrimeCost = numPrime.Value;
                    editingBook.SalePrice = numSale.Value;
                    editingBook.IsSequel = chkSequel.Checked;

                    try
                    {
                        db.SaveChanges();
                        MessageBox.Show("Book saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                    {
                        var entry = db.Entry(editingBook);
                        entry.Reload();
                        MessageBox.Show("Warning: Another user has modified this book. Your changes were not saved.",
                                        "Update Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving book: {ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}