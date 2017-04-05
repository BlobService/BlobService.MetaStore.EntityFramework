using BlobService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class ContainerMeta : IContainerMeta
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<IBlobMeta> Blobs { get; set; }
    }
}
