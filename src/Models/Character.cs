﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlkemyChallenge.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int Age { get; set; }
        public int Weight { get; set; }
        public string Story { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}
