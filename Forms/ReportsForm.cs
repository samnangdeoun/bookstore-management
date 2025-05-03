using BookstoreManagement.Data;
using System;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class ReportsForm : Form
    {
        private DateTimePicker dtpStartDate, dtpEndDate;
        private Button btnGenerateReport, btnExportExcel;
        private DataGridView dgvSalesReport;

        public ReportsForm()
        {
            this.Text = "Sales Report";
            this.Size = new System.Drawing.Size(800, 450);
            this.StartPosition = FormStartPosition.CenterScreen;
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
                Left = 20,
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

            btnExportExcel= new Button
            {
                Text = "Export Report",
                Top = top + 10,
                Left = 220,
                Width = 180

            };
            btnExportExcel.Click += btnExport_Click;
            this.Controls.Add(btnExportExcel);
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            using (BookStoreContext db = new BookStoreContext())
            {
                DateTime startDate = dtpStartDate.Value.Date;
                DateTime endDate = dtpEndDate.Value.Date.AddDays(1).AddTicks(-1); // include full day

                if (endDate < startDate)
                {
                    MessageBox.Show("End Date must be after Start Date.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var salesReport = db.Sales
                    .Where(s => s.DateSold >= startDate && s.DateSold <= endDate)
                    .GroupBy(s => new { s.Book.Name, s.Book.AuthorName })
                    .Select(g => new
                    {
                        BookName = g.Key.Name,
                        Author = g.Key.AuthorName,
                        DiscountAmount = g.Sum(s => s.DiscountAmount),
                        QuantitySold = g.Sum(s => s.Quantity),
                        TotalSales = g.Sum(s => s.TotalPrice)
                    })
                    .ToList();

                dgvSalesReport.DataSource = salesReport;

                if (dgvSalesReport.Columns["TotalSales"] != null)
                {
                    dgvSalesReport.Columns["TotalSales"].DefaultCellStyle.Format = "C2"; // currency format
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dgvSalesReport.DataSource == null)
            {
                MessageBox.Show("Please generate a report first.");
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Workbook|*.xlsx",
                Title = "Save Sales Report"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var dt = new System.Data.DataTable();

                    // Add columns
                    foreach (DataGridViewColumn column in dgvSalesReport.Columns)
                    {
                        dt.Columns.Add(column.HeaderText);
                    }

                    // Add rows
                    foreach (DataGridViewRow row in dgvSalesReport.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            var data = row.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString()).ToArray();
                            dt.Rows.Add(data);
                        }
                    }

                    using (var workbook = new ClosedXML.Excel.XLWorkbook())
                    {
                        workbook.Worksheets.Add(dt, "SalesReport");
                        workbook.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Report exported successfully!", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

    }
}
