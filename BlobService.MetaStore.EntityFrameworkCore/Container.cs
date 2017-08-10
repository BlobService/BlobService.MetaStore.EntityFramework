using BlobService.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class Container : IContainer
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Blob> Blobs { get; set; } = new HashSet<Blob>();
    }
}
