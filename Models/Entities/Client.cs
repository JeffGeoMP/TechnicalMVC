﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechnicalMVC.Models.Entities
{
    public class Client : AttributesLog
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DPI { get; set; }

        [Required]
        public string NIT { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
