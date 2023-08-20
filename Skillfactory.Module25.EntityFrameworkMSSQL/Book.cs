using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int YearOfPublishing { get; set; }

        /// <summary>
        /// Id of the User, who borrowed this book object.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Record of a user, who borrowed book, is null if book is returned to library.
        /// </summary>
        public User? BorrowedBy { get; set; }

        /// <summary>
        /// List of credited Authors.
        /// </summary>
        public List<Author> AuthorsList { get; set; }

        /// <summary>
        /// Id of the book's category.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Record of the book's category.
        /// </summary>
        public Category Category { get; set; }

        public override string ToString()
        {
            string Authors = AuthorsList.Count > 1? String.Join(", ", AuthorsList): AuthorsList[0].ToString();
            return $"{Authors}. {Title}. - {YearOfPublishing}.";
        }
    }
}
