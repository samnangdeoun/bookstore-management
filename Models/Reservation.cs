using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreManagement.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReservedAt { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<ReservationItem> ReservationItems { get; set; }
        public decimal TotalAmount => ReservationItems?.Sum(item => item.TotalPrice) ?? 0;
    }

}
