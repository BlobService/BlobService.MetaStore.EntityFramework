using BlobService.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class BlobMeta : IBlobMeta
    {
        [Key]
        public string Id { get; set; }
        public string OrigFileName { get; set; }
        public int SizeInBytes { get; set; }
        public string MimeType { get; set; }
        public string StorageSubject { get; set; }
        public string ContainerId { get; set; }
        public virtual ContainerMeta Container { get; set; }
    }
}
