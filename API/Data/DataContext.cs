using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.LikedUserId }); //Задает свойства, которые составляют
                                                                     //первичный ключ для этого типа сущности.

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser) //Настраивает связь, в которой этот тип сущности
                                           //имеет ссылку, указывающую на один экземпляр другого типа в связи.
                .WithMany(l => l.LikedUsers) //Настраивает это отношение "один ко многим".
                .HasForeignKey(s => s.SourceUserId) // Задаёт внешний ключ
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UserLike>()
                .HasOne(s => s.LikedUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
