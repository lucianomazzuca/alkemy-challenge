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

        public override async Task<Character> GetById(int id)
        {
            var character = await _context.Characters
                .Include(character => character.Movies)
                .FirstOrDefaultAsync(m => m.Id == id);

            return character;
        }
    }
}
