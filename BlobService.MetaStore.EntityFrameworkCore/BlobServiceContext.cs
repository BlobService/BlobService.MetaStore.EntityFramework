using BlobService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class BlobServiceContext<TContainerMeta, TBlobMeta> : DbContext
        where TContainerMeta : ContainerMeta
        where TBlobMeta : BlobMeta
    {
        public DbSet<TContainerMeta> ContainersMetadata { get; set; }
        public DbSet<TBlobMeta> BlobsMetadata { get; set; }

        public BlobServiceContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TContainerMeta>(c =>
            {
                c.HasIndex(x => x.Name).HasName("ContainerNameIndex").IsUnique();
                c.ToTable("BlobService_ContainersMetadata");
            });

            modelBuilder.Entity<TBlobMeta>(b =>
            {
                b.ToTable("BlobService_BlobsMetadata");
            });
        }
    }
}
