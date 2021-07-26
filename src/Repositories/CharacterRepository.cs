using AlkemyChallenge.Data;
using AlkemyChallenge.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlkemyChallenge.Repositories
{
    public class CharacterRepository : BaseRepository<Character>
    {
        public CharacterRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Character> GetAllWith(string name, int? age = null, int? movieId = null, int? weight = null)
        {
            IQueryable<Character> characters = _context.Characters;

            if (name != null)
            {
                characters = characters.Where(c => c.Name.Contains(name));
            }

            if (age != null)
            {
                characters = characters.Where(c => c.Age == age);
            }

            if (movieId != null)
            {
                characters = characters.Where(c => c.Movies.Any(m => m.Id == movieId));

                // Projection 
                //characters = characters.Include(characters => characters.Movies)
                //    .Select(c => new Character
                //    {
                //        Id = c.Id,
                //        Age = c.Age,
                //        Image = c.Image,
                //        Name = c.Name,
                //        Story = c.Story,
                //        Weight = c.Weight,
                //        Movies = (ICollection<Movie>)c.Movies.Where(m => m.Id == movieId)
                //    });
            }

            if (weight != null)
            {
                characters = characters.Where(c => c.Weight== weight);
            }

            return characters;
        }

        public override async Task<Character> GetById(int id)
        {
            var character = await _context.Characters
                .Include(character => character.Movies)
                .FirstOrDefaultAsync(m => m.Id == id);

            return character;
        }
    }
}
