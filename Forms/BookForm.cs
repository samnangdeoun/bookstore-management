using BookstoreManagement.Data;
using BookstoreManagement.Models;
using System;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
                // Clone the book to avoid entity tracking issues
                editingBook = new Book
                {
                    Id = book.Id,
                    Name = book.Name,
                    AuthorName = book.AuthorName,
                    PublisherName = book.PublisherName,
                    Pages = book.Pages,
                    GenreId = book.GenreId,
                    DatePublished = book.DatePublished,
                    PrimeCost = book.PrimeCost,
                    SalePrice = book.SalePrice,
                    IsSequel = book.IsSequel
                };

                txtName.Text = editingBook.Name;
                txtAuthor.Text = editingBook.AuthorName;
                txtPublisher.Text = editingBook.PublisherName;
                numPages.Value = (decimal)editingBook.Pages;
                cmbGenre.SelectedValue = editingBook.GenreId;
                dtpPublishDate.Value = editingBook.DatePublished != default ? editingBook.DatePublished : DateTime.Now;
                numPrime.Value = editingBook.PrimeCost != default ? editingBook.PrimeCost : 0m;
                numSale.Value = editingBook.SalePrice != default ? editingBook.SalePrice : 0m;
                chkSequel.Checked = editingBook.IsSequel;
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

                    Book bookToSave;

                    if (editingBook == null || editingBook.Id == 0)
                    {
                        // New book
                        bookToSave = new Book();
                        db.Books.Add(bookToSave);
                    }
                    else
                    {
                        // Existing book, fetch it fresh from the database
                        bookToSave = db.Books.Find(editingBook.Id);
                        if (bookToSave == null)
                        {
                            MessageBox.Show("Book not found in database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    // Update values from the form
                    bookToSave.Name = txtName.Text;
                    bookToSave.AuthorName = txtAuthor.Text;
                    bookToSave.PublisherName = txtPublisher.Text;
                    bookToSave.Pages = (int)numPages.Value;
                    bookToSave.GenreId = (int)cmbGenre.SelectedValue;
                    bookToSave.DatePublished = dtpPublishDate.Value;
                    bookToSave.PrimeCost = numPrime.Value;
                    bookToSave.SalePrice = numSale.Value;
                    bookToSave.IsSequel = chkSequel.Checked;

                    db.SaveChanges();

                    MessageBox.Show("Book saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException)
                {
                    MessageBox.Show("Warning: Another user has modified this book. Your changes were not saved.",
                                    "Update Conflict", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving book: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
