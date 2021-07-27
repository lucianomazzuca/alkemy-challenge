using AlkemyChallenge.DTOs.Character;
using AlkemyChallenge.DTOs.Genre;
using AlkemyChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlkemyChallenge.DTOs.Movie
{
    public class MovieDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int Rating { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public ICollection<CharacterReadDto> Characters { get; set; }
        public ICollection<GenreReadDto> Genres { get; set; }
    }
}
