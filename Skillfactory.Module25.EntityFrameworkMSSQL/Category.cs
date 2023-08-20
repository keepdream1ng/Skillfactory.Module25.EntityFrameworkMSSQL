using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Book> BooksInCategory { get; set; }
    }
}
