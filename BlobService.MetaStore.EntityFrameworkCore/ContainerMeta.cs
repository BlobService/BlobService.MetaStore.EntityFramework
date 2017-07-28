using BlobService.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class ContainerMeta : IContainerMeta
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<BlobMeta> Blobs { get; set; } = new HashSet<BlobMeta>();
    }
}
