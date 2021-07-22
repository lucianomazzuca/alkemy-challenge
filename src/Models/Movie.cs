using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlkemyChallenge.Models
{
    public class Movie : IModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public int Rating { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public ICollection<Character> Characters { get; set; }
        public ICollection<Genre> Genres { get; set; }
    }
}
