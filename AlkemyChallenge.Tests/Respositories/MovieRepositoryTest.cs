using AlkemyChallenge.Data;
using AlkemyChallenge.Exceptions;
using AlkemyChallenge.Models;
using AlkemyChallenge.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AlkemyChallenge.Tests.Respositories
{
    public class MovieRepositoryTest
    {
        public MovieRepositoryTest()
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

            var genre1 = new Genre() { Id = 1, Image = "image.jpg", Name = "Action" };

            movie2.Genres.Add(genre1);

            using (var context = new AppDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Movies.Add(movie1);
                context.Movies.Add(movie2);
                context.Movies.Add(movie3);

                context.Characters.Add(character1);
                context.Characters.Add(character2);

                context.Genres.Add(genre1);

                context.SaveChanges();
            }
        }

        [Fact]
        public async void Add_NewMovie()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);
                var movie1 = new Movie() { Id = 4, Title = "Interstellar", Image = "image.jpg", CreatedAt = DateTime.Parse("2013-04-28"), Rating = 5 };

                await repository.Add(movie1);
                var movies = context.Movies.ToList();

                Assert.Equal(4, movies.Count);
                Assert.Equal(4, movies[3].Id);
            }
        }

        [Fact]
        public async void Add_testChar()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);
                var movie1 = new Movie() { Id = 4, Title = "Interstellar", Image = "image.jpg", CreatedAt = DateTime.Parse("2013-04-28"), Rating = 5 };
                var character1 = new Character() { Id = 1, Name = "Cooper", Age = 32, Image = "image.jpg", Story = "lorem ipsum", Weight = 80 };
                movie1.Characters.Add(character1);

                await repository.Add(movie1);
                var movies = context.Movies
                    .Include(movies => movies.Characters)
                    .ToList();

                Assert.Equal(4, movies.Count);
                Assert.Equal(4, movies[3].Id);
                Assert.Equal(1, movies[3].Characters.Count);
            }
        }

        [Fact]
        public async void AddWith_NewMovie_WithCharacter()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);
                var movie1 = new Movie() { Id = 4, Title = "Interstellar", Image = "image.jpg", CreatedAt = DateTime.Parse("2013-04-28"), Rating = 5 };

                var characterIds = new[] { 1 };

                await repository.AddWith(movie1, characterIds);
                var movies = context.Movies
                    .Include(movies => movies.Characters)
                    .ToList();

                Assert.Equal(4, movies.Count);
                Assert.Equal(4, movies[3].Id);
                Assert.Equal(1, movies[3].Characters.Count);
            }
        }

        [Fact]
        public async void AddWith_NewMovie_Genre()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);
                var movie1 = new Movie() { Id = 4, Title = "Interstellar", Image = "image.jpg", CreatedAt = DateTime.Parse("2013-04-28"), Rating = 5 };
                var genresIds = new[] { 1 };

                await repository.AddWith(movie1, null, genresIds);
                var movies = context.Movies
                    .Include(movies => movies.Genres)
                    .ToList();

                Assert.Equal(4, movies.Count);
                Assert.Equal(4, movies[3].Id);
                Assert.Equal(2, movies[3].Genres.Count);
            }
        }

        [Fact]
        public async void Update_ExistingMovie()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);
                var movie1 = new Movie() { Id = 4, Title = "Interstellar", Image = "image.jpg", CreatedAt = DateTime.Parse("2013-04-28"), Rating = 5 };
                await repository.Add(movie1);

                movie1.Title = "Batman";
                await repository.Update(movie1);
                var movies = context.Movies
                    .Include(movies => movies.Genres)
                    .ToList();

                Assert.Equal("Batman", movies[3].Title);
            }

        }

        [Fact]
        public async void Update_WithCharacter()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);
                var movie1 = new Movie() { Id = 4, Title = "Interstellar", Image = "image.jpg", CreatedAt = DateTime.Parse("2013-04-28"), Rating = 5 };
                var character1 = new Character() { Id = 1, Name = "Cooper", Age = 32, Image = "image.jpg", Story = "lorem ipsum", Weight = 80 };
                var character2 = new Character() { Id = 2, Name = "Test", Age = 32, Image = "image.jpg", Story = "lorem ipsum", Weight = 80 };
                movie1.Characters.Add(character1);
                await repository.Add(movie1);
                movie1.Characters.Remove(character1);
                movie1.Characters.Add(character2);

                await repository.Update(movie1);
                var movies = context.Movies
                    .Include(movies => movies.Genres)
                    .ToList();
                var charactersRelated = movies[3].Characters.ToList<Character>();

                Assert.Equal(2, charactersRelated[0].Id);
            }
        }

        [Fact]
        public async void Update_CharactersFromMovie()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);
                var movie1 = new Movie() { Id = 4, Title = "Interstellar", Image = "image.jpg", CreatedAt = DateTime.Parse("2013-04-28"), Rating = 5 };
                var characterIds = new[] { 1 };
                await repository.AddWith(movie1, characterIds, null);
                var newCharacterIds = new[] { 2 };

                await repository.UpdateWith(movie1, newCharacterIds);
                var movies = context.Movies
                    .Include(movies => movies.Genres)
                    .ToList();
                var charactersRelated = movies[3].Characters.ToList<Character>();

                Assert.Equal(2, charactersRelated[0].Id);
            }
        }

        [Fact]
        public async void UpdateWith_CharactersFromMovie()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);
                var movie1 = new Movie() { Id = 4, Title = "Interstellar", Image = "image.jpg", CreatedAt = DateTime.Parse("2013-04-28"), Rating = 5 };
                var characterIds = new[] { 1 };
                await repository.AddWith(movie1, characterIds, null);
                var newCharacterIds = new[] { 2 };

                await repository.UpdateWith(movie1, newCharacterIds);
                var movies = context.Movies
                    .Include(movies => movies.Genres)
                    .ToList();
                var charactersRelated = movies[3].Characters.ToList<Character>();

                Assert.Equal(2, charactersRelated[0].Id);
            }
        }

        [Fact]
        public async void GetById_ReturnsMovie_WithCharactersAndGenres()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);
                var movie1 = new Movie() { Id = 4, Title = "Interstellar", Image = "image.jpg", CreatedAt = DateTime.Parse("2013-04-28"), Rating = 5 };
                var genresIds = new[] { 1 };
                var charactersIds = new[] { 1 };
                await repository.AddWith(movie1, charactersIds, genresIds);

                var movie = await repository.GetById(4);

                Assert.Equal(4, movie.Id);
                Assert.Equal(1, movie.Characters.Count);
                Assert.Equal(1, movie.Genres.Count);
            }
        }

        [Fact]
        public async void GetAll_ReturnsMovies()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);

                var movies = await repository.GetAll();

                Assert.Equal(3, movies.Count());
            }
        }

        [Fact]
        public async void Delete_ok()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);

                await repository.Delete(3);
                var movies = await repository.GetAll();


                Assert.Equal(2, movies.Count());
            }
        }

        [Fact]
        public async void Delete_ThrowsErrorOnNotFound()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);

                await Assert.ThrowsAsync<RecordNotFoundException>(() => repository.Delete(4));
            }
        }

        [Fact]
        public void GetAllWith_Title()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);

                var data = repository.GetAllWith("Memento", null, null);
                var movies = data.ToList();

                Assert.Single(movies);
                Assert.Equal(2, movies[0].Id);
                Assert.Equal("Memento", movies[0].Title);
            }
        }

        [Fact]
        public void GetAllWith_Genre()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);

                var data = repository.GetAllWith(null, null, 1);
                var movies = data.ToList();

                Assert.Single(movies);
                Assert.Equal(2, movies[0].Id);
                Assert.Equal("Memento", movies[0].Title);
            }
        }
    }
}
