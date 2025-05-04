using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreManagement.Models
{
    public class ReservationItem
    {
        public int Id { get; set; }

        public int ReservationId { get; set; }
        public int BookId { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal")]
        public decimal TotalPrice { get; set; }

        // Navigation
        public virtual Reservation Reservation { get; set; }
        public virtual Book Book { get; set; }
    }
}
