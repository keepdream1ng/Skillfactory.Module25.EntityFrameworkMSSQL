using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkillFactory.Module16TestsViaNUnit.Tests
{
    public class DataGenerator
    {
        public Faker<Author> authorFaker;
        public Faker<User> userFaker;
        public Faker<Book> bookFaker;

        public DataGenerator()
        {
            Randomizer.Seed = new Random(111);
            authorFaker = new Faker<Author>()
                .RuleFor(a => a.FirstName, f => f.Name.FirstName())
                .RuleFor(a => a.LastName, f => f.Name.LastName())
                ;
            userFaker = new Faker<User>()
                .RuleFor(u => u.Name, f => f.Name.FullName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                ;
            bookFaker = new Faker<Book>()
                .RuleFor(b => b.Title, f => f.Lorem.Sentence(1, 4))
                .RuleFor(b => b.YearOfPublishing, f => f.Random.Int(1900, 2023))
                .RuleFor(b => b.Category, f => f.PickRandom(FakeData.Categories))
                .RuleFor(b => b.Authors, f => f.Make(f.Random.Int(1, 3), () => f.PickRandom(FakeData.Authors)))
                .RuleFor(b => b.BorrowedBy, f => f.PickRandom(FakeData.Users).OrNull(f, 0.7f))
                ;
        }
    }
}
