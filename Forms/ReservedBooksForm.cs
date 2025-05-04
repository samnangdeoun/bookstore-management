using BookstoreManagement.Data;
using BookstoreManagement.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class ReservedBooksForm : Form
    {
        private DataGridView dgvReservedBooks;
        private Button btnMarkAsCollected, btnCancelReservation;

        public ReservedBooksForm()
        {
            this.Text = "Reserved Books";
            this.Size = new System.Drawing.Size(800, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeControls();
            LoadReservedBooks();
        }

        private void InitializeControls()
        {
            int top = 20;

            dgvReservedBooks = new DataGridView
            {
                Top = top,
                Left = 20,
                Width = 740,
                Height = 200,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            dgvReservedBooks.SelectionChanged += DgvReservedBooks_SelectionChanged;
            this.Controls.Add(dgvReservedBooks);

            top += 220;

            btnMarkAsCollected = new Button
            {
                Text = "Mark as Collected",
                Top = top,
                Left = 20,
                Width = 180
            };
            btnMarkAsCollected.Click += btnMarkAsCollected_Click;
            this.Controls.Add(btnMarkAsCollected);

            btnCancelReservation = new Button
            {
                Text = "Cancel Reservation",
                Top = top,
                Left = 220,
                Width = 180
            };
            btnCancelReservation.Click += btnCancelReservation_Click;
            this.Controls.Add(btnCancelReservation);
        }

        private void LoadReservedBooks()
        {
            using (BookStoreContext db = new BookStoreContext())
            {
                var reservedBooks = db.Reservations
                    .Select(r => new
                    {
                        r.Id,
                        r.CustomerName,
                        r.ReservedAt,
                        Status = r.Status ? "Collected" : "Pending"
                    }).ToList();

                dgvReservedBooks.DataSource = reservedBooks;
            }

            DgvReservedBooks_SelectionChanged(null, null); // Update button state
        }

        private void DgvReservedBooks_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReservedBooks.CurrentRow != null)
            {
                string status = dgvReservedBooks.CurrentRow.Cells["Status"].Value.ToString();
                btnMarkAsCollected.Enabled = status != "Collected";
            }
        }

        private void btnMarkAsCollected_Click(object sender, EventArgs e)
        {
            if (dgvReservedBooks.CurrentRow == null) return;

            int id = (int)dgvReservedBooks.CurrentRow.Cells["Id"].Value;

            using (var db = new BookStoreContext())
            {
                var reservation = db.Reservations
                    .Include(r => r.ReservationItems)
                    .FirstOrDefault(r => r.Id == id);

                if (reservation == null) return;

                if (reservation.Status)
                {
                    MessageBox.Show("This reservation is already marked as collected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Show confirmation dialog
                var confirm = MessageBox.Show(
                    $"Are you sure you want to mark reservation #{reservation.Id} as collected?\n" +
                    $"This will move the data to the sales records.",
                    "Confirm Collection",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirm != DialogResult.Yes)
                    return;

                // Mark as collected
                reservation.Status = true;

                // Add sales
                foreach (var item in reservation.ReservationItems)
                {
                    db.Sales.Add(new Sale
                    {
                        BookId = item.BookId,
                        Quantity = item.Quantity,
                        TotalPrice = item.UnitPrice * item.Quantity,
                        DiscountAmount = item.Discount * item.Quantity,
                        DateSold = DateTime.Now
                    });
                }

                db.SaveChanges();
                LoadReservedBooks();
                MessageBox.Show("Reservation marked as collected and sales recorded.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnCancelReservation_Click(object sender, EventArgs e)
        {
            if (dgvReservedBooks.CurrentRow == null) return;

            int id = (int)dgvReservedBooks.CurrentRow.Cells["Id"].Value;

            using (BookStoreContext db = new BookStoreContext())
            {
                var reservation = db.Reservations
                    .Include(r => r.ReservationItems)
                    .FirstOrDefault(r => r.Id == id);

                if (reservation == null) return;

                if (reservation.Status)
                {
                    MessageBox.Show("Cannot cancel a collected reservation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Remove reservation and related items
                db.ReservationItems.RemoveRange(reservation.ReservationItems);
                db.Reservations.Remove(reservation);
                db.SaveChanges();

                LoadReservedBooks();
                MessageBox.Show("Reservation canceled.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
