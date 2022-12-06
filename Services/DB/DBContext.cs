using Microsoft.EntityFrameworkCore;
using Models;

namespace Services
{
    public partial class CriptoAlgorithmsDBContext : DbContext
    {
        public virtual DbSet<ClassificationByNumberOfKeys> classificationByNumberOfKeys { get; set; }
        public virtual DbSet<ClassificationBySpecOfInformProcess> classificationBySpecOfInformProcesses { get; set; }
        public virtual DbSet<ReplacementMode> replacementModes { get; set; }
        public virtual DbSet<TableOfCharacteristics> tableOfCharacteristics { get; set; }
        public virtual DbSet<CryptoAlgorithmsTable> cryptoAlgorithmsTables { get; set; }

        public CriptoAlgorithmsDBContext()
        {
            var flds = typeof(CriptoAlgorithmsDBContext).GetFields(System.Reflection.BindingFlags.Public);
            foreach (var fld in flds)
            {
                if(fld == null) { throw new System.Exception("Пустая ссылка на таблицу бд CriptoAlgDB.db"); }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=CriptoAlgDB.db");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
