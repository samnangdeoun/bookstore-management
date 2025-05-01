using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class DiscountForm : Form
    {
        private BookStoreDBDataContext db;

        private ComboBox cmbGenres;
        private NumericUpDown nudDiscount;
        private DateTimePicker dtpStartDate, dtpEndDate;
        private Button btnApplyDiscount;

        public DiscountForm()
        {
            this.Text = "Apply Discount";
            this.Size = new Size(400, 300);
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
                    Width = 120
                };
            }

            Control AddControl(Control control)
            {
                control.Top = top;
                control.Left = 160;
                control.Width = 180;
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
                Text = "Apply Discount",
                Top = top + 10,
                Left = 160,
                Width = 180
            };
            btnApplyDiscount.Click += btnApplyDiscount_Click;
            this.Controls.Add(btnApplyDiscount);
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

            if (startDate > endDate)
            {
                MessageBox.Show("Start date must be before end date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
