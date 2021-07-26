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
    public class UserRepositoryTest
    {
        public UserRepositoryTest()
        {
            ContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("Test")
            .Options;

            Seed();
        }

        protected DbContextOptions<AppDbContext> ContextOptions { get; }

        private void Seed()
        {
            var user1 = new User() { Id = 1, Email = "test@mail.com", Password = "123456" };
            var user2 = new User() { Id = 2, Email = "test2@mail.com", Password = "123456" };
            var user3 = new User() { Id = 3, Email = "test3@mail.com", Password = "123456" };


            using (var context = new AppDbContext(ContextOptions))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                context.Users.Add(user1);
                context.Users.Add(user2);
                context.Users.Add(user3);

                context.SaveChanges();
            }
        }

        [Fact]
        public async void Add_NewUser()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new UserRepository(context);
                var user = new User() { Email = "user@mail.com", Password = "12345" };

                await repository.Add(user);
                var users = context.Users.ToList();

                Assert.Equal(4, users.Count);
                Assert.Equal(4, users[3].Id);
                Assert.Equal("user@mail.com", users[3].Email);
            }
        }

        [Fact]
        public async void GetByEmail_ReturnsUser()
        {
            using (var context = new AppDbContext(ContextOptions))
            {
                var repository = new UserRepository(context);
                var user1 = new User() { Email = "user@mail.com", Password = "12345" };
                await repository.Add(user1);

                var user = await repository.GetByEmail("user@mail.com");

                Assert.Equal(4, user.Id);
                Assert.Equal("user@mail.com", user.Email);
            }
        }
    }
}
