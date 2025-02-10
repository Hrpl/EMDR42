using EMDR42.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMDR42.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserProfileEntity> UserProfile { get; set; }
    public DbSet<QualificationEntity> Qualifications { get; set; }
    public DbSet<ContactEntity> Contacts { get; set; }
    public DbSet<SessionEntity> Sessions { get; set; }
    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<TherapyEntity> Therapies { get; set; }
    public DbSet<FeedbackEntity> Feedbacks { get; set; }
    public DbSet<RoleEntity> Roles { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.Migrate();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
        base.OnConfiguring(optionsBuilder);
    }
}
