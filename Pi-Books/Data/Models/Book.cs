using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HotChocolate;

namespace Pi_Books.Data.Models
{
    [GraphQLDescription("A written or printed work consisting of pages glued or sewn together along one side and bound in covers.")]
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        [GraphQLDescription("The Popularity of the book in the scale of 1 to 10.")]
        public int? Rating { get; set; }
        public string Genre { get; set; }
        //public string Author { get; set; }
        public string CoverUrl { get; set; }
        public DateTime DateAdded { get; set; }

        //Navigation Properties
        public int PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        public List<BookAuthor> BookAuthors { get; set; }
    }
}
