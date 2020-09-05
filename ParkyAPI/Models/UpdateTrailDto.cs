using System;
using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models
{
    public class UpdateTrailDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int NationalParkId { get; set; }


        [Required]
        [Range(1, 10000)]
        public double Distance { get; set; }

        [Required]
        public DifficultyLevel DifficultyLevel { get; set; }

    }
}