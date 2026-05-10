using Microsoft.EntityFrameworkCore;
using MinimalPersonApi.Models;

namespace MinimalPersonApi.Data;

public class PersonDbContext : DbContext
{
    public DbSet<Person> Persons => Set<Person>();

    public PersonDbContext(DbContextOptions<PersonDbContext> options)
        : base(options) { }
}
