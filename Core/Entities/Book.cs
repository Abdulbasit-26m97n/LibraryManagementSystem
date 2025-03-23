using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Core.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int PublicationYear { get; set; }
        public bool IsAvailable { get; set; }

        public DateTime AddedDate { get; set; }
        public DateTime LastIssuedDate { get; set; }
        public string InternalNote { get; set; }
        public int TotalCopies { get; set; }
        public int IssuedCopies { get; set; }
    }
}
