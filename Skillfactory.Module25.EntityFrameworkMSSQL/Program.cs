namespace Skillfactory.Module25.EntityFrameworkMSSQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var db = new AppContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.SaveChanges();
            }
            Console.WriteLine("Finished.");
        }
    }
}