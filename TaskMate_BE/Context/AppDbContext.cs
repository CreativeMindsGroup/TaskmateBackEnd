using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskMate.Configurations;
using TaskMate.Entities;

namespace TaskMate.Context;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SliderConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Workspace> Workspaces { get; set; }
    public DbSet<Boards> Boards { get; set; }
    public DbSet<CardList> CardLists { get; set; }
    public DbSet<Card> Cards { get; set; }
    public DbSet<WorkspaceUser> WorkspaceUsers { get; set; }
    public DbSet<UserBoards> UserBoards { get; set; }
    public DbSet<Comment> Comments { get; set; }
}