using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreManagement.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AuthorName { get; set; }
        public int Pages { get; set; }

        [Required]
        public int GenreId { get; set; }
        public DateTime DatePublished { get; set; }
        public string PublisherName { get; set; }

        [Column(TypeName = "decimal")]
        public decimal PrimeCost { get; set; }

        [Column(TypeName = "decimal")]
        public decimal SalePrice { get; set; }
        public bool IsSequel { get; set; }

        [ForeignKey("GenreId")]
        public virtual Genre Genre { get; set; }

        public Book() { }
    }
}
