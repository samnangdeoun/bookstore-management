using System;
using System.Linq;
using System.Windows.Forms;

namespace BookstoreManagement.Forms
{
    public partial class ReportsForm : Form
    {
        private BookstoreDataDataContext db;

        public ReportsForm()
        {
            InitializeComponent();
            db = new BookstoreDataDataContext();
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
