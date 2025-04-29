using System;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class DiscountForm : Form
    {
        private BookstoreDataDataContext db;

        public DiscountForm()
        {
            InitializeComponent();
            db = new BookstoreDataDataContext();
            LoadGenres();
        }

        private void LoadGenres()
        {
            var genres = db.Genres.Select(g => new { g.Id, g.Name }).ToList();
            cmbGenres.DataSource = genres;
            cmbGenres.DisplayMember = "Name";
            cmbGenres.ValueMember = "Id";
        }

        private void btnApplyDiscount_Click(object sender, EventArgs e)
        {
            int genreId = (int)cmbGenres.SelectedValue;
            decimal discount = nudDiscount.Value;
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;

            var discountEntry = new Discount
            {
                GenreId = genreId,
                Percentage = discount,
                StartDate = startDate,
                EndDate = endDate
            };

            db.Discounts.InsertOnSubmit(discountEntry);
            db.SubmitChanges();

            MessageBox.Show("Discount applied successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
