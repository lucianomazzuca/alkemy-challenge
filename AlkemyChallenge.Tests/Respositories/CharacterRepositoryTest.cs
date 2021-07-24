using AlkemyChallenge.Data;
using AlkemyChallenge.Exceptions;
using AlkemyChallenge.Models;
using AlkemyChallenge.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AlkemyChallenge.Tests.Respositories
{
    public class CharacterRepositoryTest
    {
        public CharacterRepositoryTest()
        {
            ContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("Test")
            .Options;

            Seed();
        }

        protected DbContextOptions<AppDbContext> ContextOptions { get; }

        private void Seed()
        {
            var movie1 = new Movie() { Id = 1, Title = "Pirates of the Caribbean", Image = "image.jpg", CreatedAt = DateTime.Parse("2004-04-28"), Rating = 5 };
            var movie2 = new Movie() { Id = 2, Title = "Memento", Image = "image.jpg", CreatedAt = DateTime.Parse("2003-04-28"), Rating = 5 };
            var movie3 = new Movie() { Id = 3, Title = "Sicario", Image = "image.jpg", CreatedAt = DateTime.Parse("2015-04-28"), Rating = 5 };

            var character1 = new Character() { Id = 1, Name = "Cooper", Age = 32, Image = "image.jpg", Story = "lorem ipsum", Weight = 80 };
            var character2 = new Character() { Id = 2, Name = "Test", Age = 32, Image = "image.jpg", Story = "lorem ipsum", Weight = 80 };
            var character3 = new Character() { Id = 3, Name = "Batman", Age = 32, Image = "image.jpg", Story = "lorem ipsum", Weight = 80 };


            using (var context = new AppDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Movies.Add(movie1);
                context.Movies.Add(movie2);
                context.Movies.Add(movie3);

                context.Characters.Add(character1);
                context.Characters.Add(character2);
                context.Characters.Add(character3);

                context.SaveChanges();
            }
        }

        [Fact]
        public async void Add_NewCharacter()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new CharacterRepository(context);
                var character = new Character() { Name = "Joker", Age = 32, Image = "image.jpg", Story = "lorem ipsum", Weight = 80 };

                await repository.Add(character);
                var characters = context.Characters.ToList();

                Assert.Equal(4, characters.Count);
                Assert.Equal(4, characters[3].Id);
                Assert.Equal("Joker", characters[3].Name);
            }
        }

        [Fact]
        public async void Edit_ExistingCharacter_Ok()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new CharacterRepository(context);
                var character = new Character() { Id = 4, Name = "Joker", Age = 32, Image = "image.jpg", Story = "lorem ipsum", Weight = 80 };
                await repository.Add(character);

                character.Name = "Test";
                await repository.Update(character);
                var characters = context.Characters.ToList();

                Assert.Equal("Test", characters[3].Name);
            }
        }

        [Fact]
        public async void GetAll_Returns()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new CharacterRepository(context);

                var characters = await repository.GetAll();

                Assert.Equal(3, characters.Count());
            }
        }

        [Fact]
        public async void GetById_ReturnsMovie()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new CharacterRepository(context);

                var character = await repository.GetById(3);

                Assert.Equal(3, character.Id);
            }
        }


        [Fact]
        public async void Delete_ok()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new CharacterRepository(context);

                await repository.Delete(3);
                var characters = await repository.GetAll();


                Assert.Equal(2, characters.Count());
            }
        }

        [Fact]
        public async void Delete_ThrowsErrorOnNotFound()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new CharacterRepository(context);

                await Assert.ThrowsAsync<RecordNotFoundException>(() => repository.Delete(4));
            }
        }
    }
}
