namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill {Id = 1, Name="Wind", Damage = 3},
                new Skill {Id = 2, Name="Ice", Damage = 5},
                new Skill {Id = 3, Name="Fire", Damage = 10},
                new Skill {Id = 4, Name="Telekinesis", Damage = 20}
            );
        }

        public DbSet<Character> Characters => Set<Character>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Weapon> Weapons => Set<Weapon>();
        public DbSet<Skill> Skills => Set<Skill>();

    }
}