using System;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement
{
    public partial class GenreForm : Form
    {
        BookstoreDataDataContext db = new BookstoreDataDataContext();

        public GenreForm()
        {
            InitializeComponent();
            LoadGenres();
        }

        private void LoadGenres()
        {
            dgvGenres.DataSource = db.Genres.ToList();
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
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvGenres.CurrentRow != null)
            {
                int id = (int)dgvGenres.CurrentRow.Cells["Id"].Value;
                var genre = db.Genres.FirstOrDefault(g => g.Id == id);
                if (genre != null)
                {
                    db.Genres.DeleteOnSubmit(genre);
                    db.SubmitChanges();
                    LoadGenres();
                }
            }
        }
    }
}
