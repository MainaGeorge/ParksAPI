using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.DTOs
{
    public class TrailDto
    {

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int NationalParkId { get; set; }

        public NationalParkDto NationalParkDto { get; set; }

        [Required]
        public double Distance { get; set; }

        [Required]
        public double Elevation { get; set; }

        public DifficultyLevel DifficultyLevel { get; set; }

    }
}
