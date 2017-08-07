using BlobService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class BlobServiceContext : DbContext
    {
        public DbSet<ContainerMeta> ContainersMetadata { get; set; }
        public DbSet<BlobMeta> BlobsMetadata { get; set; }

        public BlobServiceContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContainerMeta>(c =>
            {
                c.HasIndex(x => x.Name).HasName("ContainerNameIndex").IsUnique();
                c.ToTable("BlobService_ContainersMetadata");
            });

            modelBuilder.Entity<BlobMeta>(b =>
            {
                b.ToTable("BlobService_BlobsMetadata");
            });
        }
    }
}