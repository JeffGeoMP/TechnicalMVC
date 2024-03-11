using System.ComponentModel.DataAnnotations;

namespace TechnicalMVC.Models.Entities
{
    public class AttributesLog
    {
        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

    }
}
