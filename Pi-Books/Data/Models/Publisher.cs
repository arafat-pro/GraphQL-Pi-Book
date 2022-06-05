using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pi_Books.Data.Models
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int? TaxIdentificationNo { get; set; }

        //Navigation Properties
        //public int? BookId { get; set; }
        public List<Book> Books { get; set; }
    }
}