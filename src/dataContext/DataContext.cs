using entities;
using Microsoft.EntityFrameworkCore;

namespace dataContext;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<TaskEntity> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserEntity>()
            .HasMany(user => user.Tasks)
            .WithOne(task => task.User)
            .HasForeignKey(task => task.UserId); // Adicione esta linha se você tiver uma propriedade UserId em TaskEntity

        //to change the name of table.
        // builder.Entity<IdentityUser>().ToTable("Users");

        // Mudar nome da Propriedade
        // builder.Entity<IdentityUser>(  
        //     iu => iu.Property(c => c.Email).HasColumnName("UserEmail")  
        //     );  
    }
}