using System.Data.Entity;

namespace TaskQueueAndCancellationPractise.DB
{
    public class ExamManagementDbContext : DbContext
    {
        public const string SchemaName = "ExamManagement";

        public ExamManagementDbContext()
            : base("SqlCeTest")
        {
            base.Configuration.ProxyCreationEnabled = false;
        }

        public ExamManagementDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClassExamedEntity>().ToTable("ClassExamed", SchemaName);
        }

        public DbSet<ClassExamedEntity> ClassExamed { get; set; }
    }
}
