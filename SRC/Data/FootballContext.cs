using Microsoft.EntityFrameworkCore;
using QuanLySanBong.Models;

namespace QuanLySanBong.Data
{
    public class FootballContext : DbContext
    {
        public FootballContext(DbContextOptions<FootballContext> options) : base(options) { }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetail { get; set; }
        public virtual DbSet<PlayGround> PlayGround { get; set; }
        public virtual DbSet<SubYard> SubYard { get; set; }
        public virtual DbSet<YardDetail> YardDetail { get; set; }
        public virtual DbSet<TimeSlot> TimeSlot { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CartDetail> CartDetail { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>().ToTable(nameof(Invoice));
            modelBuilder.Entity<InvoiceDetail>().ToTable(nameof(InvoiceDetail));
            modelBuilder.Entity<PlayGround>().ToTable(nameof(PlayGround));
            modelBuilder.Entity<SubYard>().ToTable(nameof(SubYard));
            modelBuilder.Entity<YardDetail>().ToTable(nameof(YardDetail));
            modelBuilder.Entity<TimeSlot>().ToTable(nameof(TimeSlot));
            modelBuilder.Entity<User>().ToTable(nameof(User));
            modelBuilder.Entity<Cart>().ToTable(nameof(Cart));
            modelBuilder.Entity<CartDetail>().ToTable(nameof(CartDetail));
        }
    }
}