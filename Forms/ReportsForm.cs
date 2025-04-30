using System;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class ReportsForm : Form
    {
        private BookstoreDataDataContext db;

        private DateTimePicker dtpStartDate, dtpEndDate;
        private Button btnGenerateReport;
        private DataGridView dgvSalesReport;

        public ReportsForm()
        {
            this.Text = "Sales Report";
            this.Size = new System.Drawing.Size(800, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            db = new BookstoreDataDataContext();
            InitializeControls();
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

            this.Controls.Add(CreateLabel("Start Date:"));
            dtpStartDate = (DateTimePicker)AddControl(new DateTimePicker());

            this.Controls.Add(CreateLabel("End Date:"));
            dtpEndDate = (DateTimePicker)AddControl(new DateTimePicker());

            btnGenerateReport = new Button
            {
                Text = "Generate Report",
                Top = top + 10,
                Left = 160,
                Width = 180
            };
            btnGenerateReport.Click += btnGenerateReport_Click;
            this.Controls.Add(btnGenerateReport);

            dgvSalesReport = new DataGridView
            {
                Top = top + 60,
                Left = 20,
                Width = 740,
                Height = 200,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dgvSalesReport);
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            DateTime startDate = dtpStartDate.Value;
            DateTime endDate = dtpEndDate.Value;

            var salesReport = db.Sales
                .Where(s => s.DateSold >= startDate && s.DateSold <= endDate)
                .GroupBy(s => new { s.Book.Name, s.Book.AuthorFullName })
                .Select(g => new
                {
                    BookName = g.Key.Name,
                    Author = g.Key.AuthorFullName,
                    TotalSales = g.Sum(s => s.TotalPrice),
                    QuantitySold = g.Count()
                })
                .ToList();

            dgvSalesReport.DataSource = salesReport;
        }
    }
}
