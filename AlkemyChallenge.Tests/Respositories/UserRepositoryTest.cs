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
    class UserRepositoryTest
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
            var user1 = new User() { Id = 1, Email = "test@mail.com", Password="123456"};
            var user2 = new User() { Id = 1, Email = "test2@mail.com", Password = "123456" };
            var user3 = new User() { Id = 1, Email = "test3@mail.com", Password = "123456" };


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
    }
}
