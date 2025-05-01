using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement
{
    public partial class GenreForm : Form
    {
        private BookStoreDBDataContext db;

        private TextBox txtGenreName;
        private DataGridView dgvGenres;
        private Button btnAdd, btnDelete;

        public GenreForm()
        {
            this.Text = "Manage Genres";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            db = new BookStoreDBDataContext();
            InitializeControls();
            LoadGenres();
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
                    Width = 100
                };
            }

            Control AddControl(Control control)
            {
                control.Top = top;
                control.Left = 120;
                control.Width = 180;
                this.Controls.Add(control);
                top += 40;
                return control;
            }

            this.Controls.Add(CreateLabel("Genre Name:"));
            txtGenreName = (TextBox)AddControl(new TextBox());

            btnAdd = new Button { Text = "Add Genre", Top = top + 10, Left = 120, Width = 100 };
            btnAdd.Click += btnAdd_Click;
            this.Controls.Add(btnAdd);

            dgvGenres = new DataGridView
            {
                Top = top + 60,
                Left = 20,
                Width = 540,
                Height = 200,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvGenres);

            btnDelete = new Button { Text = "Delete Genre", Top = top + 270, Left = 20, Width = 100 };
            btnDelete.Click += btnDelete_Click;
            this.Controls.Add(btnDelete);
        }

        private void LoadGenres()
        {
            dgvGenres.DataSource = db.Genres.Select(g => new { g.Id, g.Name }).ToList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtGenreName.Text))
            {
                db.Genres.InsertOnSubmit(new Genre { Name = txtGenreName.Text });
                db.SubmitChanges();
                LoadGenres();
                txtGenreName.Clear();
            }
            else
            {
                MessageBox.Show("Genre name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvGenres.CurrentRow != null)
            {
                int id = (int)dgvGenres.CurrentRow.Cells["Id"].Value;
                var genre = db.Genres.FirstOrDefault(g => g.Id == id);

                if (genre != null)
                {
                    var result = MessageBox.Show($"Are you sure you want to delete \"{genre.Name}\"?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        db.Genres.DeleteOnSubmit(genre);
                        db.SubmitChanges();
                        LoadGenres();
                    }
                }
            }
        }
    }
}
