using BookstoreManagement.Data;
using System;
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
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
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
                var reservedBooks = db.Reservations.Select(r => new
                {
                    r.Id,
                    Book = r.Book.Name,
                    CustomerName = r.CustomerName,
                    r.ReservedAt
                }).ToList();

                dgvReservedBooks.DataSource = reservedBooks;
            }
        }

        private void btnMarkAsCollected_Click(object sender, EventArgs e)
        {
            using (BookStoreContext db = new BookStoreContext())
            {
                if (dgvReservedBooks.CurrentRow != null)
                {
                    int id = (int)dgvReservedBooks.CurrentRow.Cells["Id"].Value;
                    var reservation = db.Reservations.FirstOrDefault(r => r.Id == id);

                    if (reservation != null)
                    {
                        db.Reservations.Remove(reservation);
                        db.SaveChanges();
                        LoadReservedBooks();
                        MessageBox.Show("Reservation marked as collected.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                }
            }

        private void btnCancelReservation_Click(object sender, EventArgs e)
        {
            using (BookStoreContext db = new BookStoreContext())
            {
                if (dgvReservedBooks.CurrentRow != null)
                {
                    int id = (int)dgvReservedBooks.CurrentRow.Cells["Id"].Value;
                    var reservation = db.Reservations.FirstOrDefault(r => r.Id == id);

                    if (reservation != null)
                    {
                        db.Reservations.Remove(reservation);
                        db.SaveChanges();
                        LoadReservedBooks();
                        MessageBox.Show("Reservation canceled.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}
