using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreManagement.Models
{
    public class BookStat
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int SoldCount { get; set; }
        public DateTime LastSoldAt { get; set; }
        
        public virtual Book Book { get; set; }
    }
}
