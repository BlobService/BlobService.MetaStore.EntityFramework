using BlobService.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Store.EntityFramework
{
    public class BlobServiceContext : DbContext
    {
        public DbSet<ContainerMeta> ContainersMetaData { get; set; }
        public DbSet<BlobMeta> BlobsMetaData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var containerMetaEntity = modelBuilder.Entity<ContainerMeta>();

            // TODO create correct mappings
            containerMetaEntity.ToTable("ContainersMetaData");
            containerMetaEntity.HasKey(x => x.Id).HasName("PK_Id");
            containerMetaEntity.HasIndex(x => x.Name).HasName("IX_Name").IsUnique(true);
            containerMetaEntity.HasMany<BlobMeta>().WithOne();

            var blobMetaEntity = modelBuilder.Entity<BlobMeta>();
            blobMetaEntity.HasKey(x => x.Id).HasName("PK_Id");
            blobMetaEntity.HasOne<ContainerMeta>().WithOne().HasForeignKey("ContainerId");




        }
    }
}
