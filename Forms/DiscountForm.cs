using BookstoreManagement.Data;
using BookstoreManagement.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class DiscountForm : Form
    {
        private ComboBox cmbGenres;
        private NumericUpDown nudDiscount;
        private DateTimePicker dtpStartDate, dtpEndDate;
        private Button btnApplyDiscount, btnUpdate, btnDelete;
        private DataGridView dgvDiscounts;

        private int? selectedDiscountId = null;

        public DiscountForm()
        {
            this.Text = "Manage Discounts";
            this.Size = new Size(700, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            InitializeControls();
            LoadGenres();
            LoadDiscounts();
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
                    Width = 120
                };
            }

            Control AddControl(Control control)
            {
                control.Top = top;
                control.Left = 160;
                control.Width = 200;
                this.Controls.Add(control);
                top += 40;
                return control;
            }

            this.Controls.Add(CreateLabel("Genre:"));
            cmbGenres = (ComboBox)AddControl(new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList });

            this.Controls.Add(CreateLabel("Discount (%):"));
            nudDiscount = (NumericUpDown)AddControl(new NumericUpDown
            {
                Minimum = 0,
                Maximum = 100,
                DecimalPlaces = 2
            });

            this.Controls.Add(CreateLabel("Start Date:"));
            dtpStartDate = (DateTimePicker)AddControl(new DateTimePicker());

            this.Controls.Add(CreateLabel("End Date:"));
            dtpEndDate = (DateTimePicker)AddControl(new DateTimePicker());

            btnApplyDiscount = new Button
            {
                Text = "Add Discount",
                Top = top + 10,
                Left = 160,
                Width = 200
            };
            btnApplyDiscount.Click += btnApplyDiscount_Click;
            this.Controls.Add(btnApplyDiscount);

            btnUpdate = new Button
            {
                Text = "Update Discount",
                Top = top + 10,
                Left = 370,
                Width = 120
            };
            btnUpdate.Click += btnUpdate_Click;
            this.Controls.Add(btnUpdate);

            btnDelete = new Button
            {
                Text = "Delete Discount",
                Top = top + 10,
                Left = 500,
                Width = 120
            };
            btnDelete.Click += btnDelete_Click;
            this.Controls.Add(btnDelete);

            dgvDiscounts = new DataGridView
            {
                Top = top + 60,
                Left = 20,
                Width = 640,
                Height = 300,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false
            };

            dgvDiscounts.CellClick += dgvDiscounts_CellClick;

            this.Controls.Add(dgvDiscounts);
        }

        private void LoadGenres()
        {
            using (BookStoreContext db = new BookStoreContext())
            {
                var genres = db.Genres.Select(g => new { g.Id, g.Name }).ToList();
                cmbGenres.DataSource = genres;
                cmbGenres.DisplayMember = "Name";
                cmbGenres.ValueMember = "Id";
            }
        }

        private void LoadDiscounts()
        {
            using (BookStoreContext db = new BookStoreContext())
            {
                var discounts = db.Discounts
                    .Select(d => new
                    {
                        d.Id,
                        Genre = d.Genre.Name,
                        d.Percentage,
                        d.StartDate,
                        d.EndDate,
                        d.GenreId
                    })
                    .ToList();

                dgvDiscounts.DataSource = discounts;
                dgvDiscounts.Columns["Id"].Visible = false;
                dgvDiscounts.Columns["GenreId"].Visible = false;
            }

            selectedDiscountId = null;
            btnApplyDiscount.Enabled = true;
        }

        private void btnApplyDiscount_Click(object sender, EventArgs e)
        {
            using (var db = new BookStoreContext())
            {
                int genreId = (int)cmbGenres.SelectedValue;
                decimal discount = nudDiscount.Value;
                DateTime start = dtpStartDate.Value;
                DateTime end = dtpEndDate.Value;

                if (start > end)
                {
                    MessageBox.Show("Start date must be before end date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var newDiscount = new Discount
                {
                    GenreId = genreId,
                    Percentage = discount,
                    StartDate = start,
                    EndDate = end
                };

                db.Discounts.Add(newDiscount);
                db.SaveChanges();
                MessageBox.Show("Discount added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadDiscounts();
            }
        }

        private void dgvDiscounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvDiscounts.Rows[e.RowIndex];
                selectedDiscountId = (int)row.Cells["Id"].Value;
                cmbGenres.SelectedValue = (int)row.Cells["GenreId"].Value;
                nudDiscount.Value = Convert.ToDecimal(row.Cells["Percentage"].Value);
                dtpStartDate.Value = Convert.ToDateTime(row.Cells["StartDate"].Value);
                dtpEndDate.Value = Convert.ToDateTime(row.Cells["EndDate"].Value);

                btnApplyDiscount.Enabled = false;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedDiscountId == null)
            {
                MessageBox.Show("Please select a discount to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (var db = new BookStoreContext())
            {
                var discount = db.Discounts.Find(selectedDiscountId.Value);
                if (discount != null)
                {
                    discount.GenreId = (int)cmbGenres.SelectedValue;
                    discount.Percentage = nudDiscount.Value;
                    discount.StartDate = dtpStartDate.Value;
                    discount.EndDate = dtpEndDate.Value;

                    if (discount.StartDate > discount.EndDate)
                    {
                        MessageBox.Show("Start date must be before end date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    db.SaveChanges();
                    MessageBox.Show("Discount updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDiscounts();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedDiscountId == null)
            {
                MessageBox.Show("Please select a discount to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete this discount?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            using (var db = new BookStoreContext())
            {
                var discount = db.Discounts.Find(selectedDiscountId.Value);
                if (discount != null)
                {
                    db.Discounts.Remove(discount);
                    db.SaveChanges();
                    MessageBox.Show("Discount deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDiscounts();
                }
            }
        }
    }
}
