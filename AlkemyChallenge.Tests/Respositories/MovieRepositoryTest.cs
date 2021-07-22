using AlkemyChallenge.Data;
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
            var movie1 = new Movie() { Id = 1, Title = "Pirates of the Caribbean", Image = "image.jpg", CreatedAt = DateTime.Parse("2021-04-28"), Rating = 5 };
            var movie2 = new Movie() { Id = 2, Title = "Pirates of the Caribbean", Image = "image.jpg", CreatedAt = DateTime.Parse("2021-04-28"), Rating = 5 };

            using (var context = new AppDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Movies.Add(movie1);
                context.Movies.Add(movie2);

                context.SaveChanges();
            }
        }

        [Fact]
        public async void GetById_ReturnsMovieWithId1()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);

                var movie = await repository.GetById(1);

                Console.WriteLine(movie);
                Assert.Equal(1, movie.Id);
            }
        }
        [Fact]
        public async void GetAll_ReturnsMovies()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new MovieRepository(context);

                var movies = await repository.GetAll();

                Assert.Equal(2, movies.Count());
            }
        }
    }
}
