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
        public int BookId { get; set; }
        public string CustomerName { get; set; }
        public DateTime ReservedAt { get; set; }

        // Navigation property for related Book entity
        public virtual Book Book { get; set; }
    }
}
