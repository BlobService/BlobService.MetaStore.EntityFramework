using BlobService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class BlobServiceContext<TContainerMeta, TBlobMeta> : DbContext
        where TContainerMeta : class, IContainerMeta
        where TBlobMeta : class, IBlobMeta
    {
        public DbSet<TContainerMeta> ContainersMetadata { get; set; }
        public DbSet<TBlobMeta> BlobsMetadata { get; set; }

        public BlobServiceContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TContainerMeta>(c =>
            {
                c.HasKey(x => x.Id);
                c.HasIndex(x => x.Name).HasName("ContainerNameIndex").IsUnique();
                c.Property(x => x.Name).HasMaxLength(256);
                c.ToTable("BlobService_ContainersMetadata");
                c.HasMany(x => x.Blobs).WithOne().HasForeignKey(y => y.ContainerId).IsRequired();
            });

            modelBuilder.Entity<TBlobMeta>(b =>
            {
                b.HasKey(x => x.Id);
                b.Property(x => x.MimeType).HasMaxLength(256);
                b.Property(x => x.OrigFileName).HasMaxLength(256);
                b.Property(x => x.StorageSubject).HasMaxLength(256);
                b.ToTable("BlobService_BlobsMetadata");
            });
        }
    }
}
