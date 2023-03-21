#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;
namespace activites.Models;
public class MyContext : DbContext 
{   
    public MyContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users { get; set; } 
    public DbSet<Event> Events { get; set; } 
    public DbSet<Participant> Participants { get; set; } 
  //add models here if needed
}