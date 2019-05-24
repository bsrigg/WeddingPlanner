using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId { get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Partner's Name must be at least 2 characters in length!")]
        public string Partner1{ get; set; }
        [Required]
        [MinLength(2, ErrorMessage = "Partner's Name must be at least 2 characters in length!")]
        public string Partner2 { get; set; }
        [Required]
        public DateTime CeremonyDate { get; set; }
        public int PlannerId { get; set; }
        public List<Guest> Event { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}