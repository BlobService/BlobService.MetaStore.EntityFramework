using Microsoft.EntityFrameworkCore;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class BlobServiceContext : DbContext
    {
        public DbSet<Container> Containers { get; set; }
        public DbSet<Blob> Blobs { get; set; }
        public DbSet<BlobMetaData> BlobMetaDatas { get; set; }

        public BlobServiceContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Container>(c =>
            {
                c.HasIndex(x => x.Name).HasName("ContainerNameIndex").IsUnique();
                c.ToTable("BlobService_Containers");
            });

            modelBuilder.Entity<Blob>(b =>
            {
                b.ToTable("BlobService_Blobs");
            });

            modelBuilder.Entity<BlobMetaData>(b =>
            {
                b.ToTable("BlobService_BlobsMetaDatas");
            });

        }
    }
}