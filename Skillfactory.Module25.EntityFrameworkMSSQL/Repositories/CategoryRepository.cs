using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skillfactory.Module25.EntityFrameworkMSSQL.Repositories
{
    public class CategoryRepository : BaseRepository
    {
        public int CreateCategory(string categoryName)
        {
            DbContext.Categories.Add(new Category { Name = categoryName});
            return DbContext.SaveChanges();
        }
        public Category GetCategoryById(int id)
        {
            return DbContext.Categories.SingleOrDefault(c => c.Id == id);
        }

        public List<Category> GetAllCategories()
        {
            return DbContext.Categories.ToList();
        }

        public int DeleteCategory(int id)
        {
            int result;
            using (var transactionContext = DbContext.Database.BeginTransaction())
            {
                var query = from book in DbContext.Books
                            where book.CategoryId == id
                            select book;
                foreach (Book book in query.ToList())
                {
                    book.CategoryId = null;
                    book.Category = null;
                }
                DbContext.Categories.Remove(GetCategoryById(id));
                result = DbContext.SaveChanges();
                transactionContext.Commit();
            }
            return result;
        }

        public int UpdateCategoryName(int categoryId, string categoryNewName)
        {
            GetCategoryById(categoryId).Name = categoryNewName;
            return DbContext.SaveChanges();
        }
    }
}
