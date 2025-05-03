using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookstoreManagement.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GenreId { get; set; }

        [Column(TypeName = "decimal")]
        public decimal Percentage { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }
    }
}
