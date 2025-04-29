using System;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class ReservedBooksForm : Form
    {
        private BookstoreDataDataContext db;

        public ReservedBooksForm()
        {
            InitializeComponent();
            db = new BookstoreDataDataContext();
            LoadReservedBooks();
        }

        private void LoadReservedBooks()
        {
            var reservedBooks = db.Reservations.Select(r => new
            {
                r.Id,
                Book = r.Book.Name,
                CustomerName = r.CustomerName,
                r.ReservedAt
            }).ToList();

            dgvReservedBooks.DataSource = reservedBooks;
        }

        private void btnMarkAsCollected_Click(object sender, EventArgs e)
        {
            if (dgvReservedBooks.CurrentRow != null)
            {
                int id = (int)dgvReservedBooks.CurrentRow.Cells["Id"].Value;
                var reservation = db.Reservations.FirstOrDefault(r => r.Id == id);

                if (reservation != null)
                {
                    db.Reservations.DeleteOnSubmit(reservation);
                    db.SubmitChanges();
                    LoadReservedBooks();
                    MessageBox.Show("Reservation marked as collected.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnCancelReservation_Click(object sender, EventArgs e)
        {
            if (dgvReservedBooks.CurrentRow != null)
            {
                int id = (int)dgvReservedBooks.CurrentRow.Cells["Id"].Value;
                var reservation = db.Reservations.FirstOrDefault(r => r.Id == id);

                if (reservation != null)
                {
                    db.Reservations.DeleteOnSubmit(reservation);
                    db.SubmitChanges();
                    LoadReservedBooks();
                    MessageBox.Show("Reservation canceled.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
