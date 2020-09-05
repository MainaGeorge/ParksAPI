using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Models.DTOs
{
    public class CreateTrailDto
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        [Required]
        public double Distance { get; set; }

        public DifficultyLevel DifficultyLevel { get; set; }
    }
}
