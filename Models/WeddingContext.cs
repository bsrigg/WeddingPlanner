using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;
using System.Linq;

namespace WeddingPlanner.Models
{
    public class WeddingContext : DbContext
    {
        public WeddingContext(DbContextOptions options) : base(options) { }
        public DbSet<Wedding> Weddings { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Guest> Guests { get; set; }
        public int Create(User u)
        {
            Users.Add(u);
            SaveChanges();
            return u.UserId;
        }

        public User GetUserByEmail(string email)
        {
            return Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetUserById(int UserId)
        {
            return Users.FirstOrDefault(u => u.UserId == UserId);
        }
        public void Create(Wedding w)
        {
            Add(w);
            SaveChanges();
        }
        public void Create(Guest g)
        {
            Add(g);
            SaveChanges();
        }
        public Wedding GetWeddingById(int WeddingId)
        {
            return Weddings.Where(w => w.WeddingId == WeddingId).FirstOrDefault();
        }
        public void Remove(int WId, int GId)
        {
            Guest Invitee = Guests.Where(w => w.WeddingId == WId).Where(g => g.UserId == GId).FirstOrDefault();
            Remove(Invitee);
            SaveChanges();
        }

    }
}