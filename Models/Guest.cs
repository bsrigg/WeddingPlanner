using System;
using System.ComponentModel.DataAnnotations;

namespace WeddingPlanner.Models
{
    public class Guest
    {
        [Key]
        public int GuestId { get; set; }
        public int UserId { set; get; }
        public int WeddingId { set; get; }
        public User User { get; set; }
        public Wedding Wedding { get; set; }
    }
    
}