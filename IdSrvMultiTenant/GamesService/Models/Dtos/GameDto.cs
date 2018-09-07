using System.ComponentModel.DataAnnotations;

namespace GamesService.Models.Dtos
{
    public class GameDto
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Console { get; set; }
        
        public string Type { get; set; }
    }
}