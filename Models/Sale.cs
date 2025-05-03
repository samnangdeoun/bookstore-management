using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreManagement.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal")]
        public decimal TotalPrice { get; set; }

        [Column(TypeName = "decimal")]
        public decimal DiscountAmount { get; set; }

        public DateTime DateSold { get; set; }

        public virtual Book Book { get; set; }
    }
}
