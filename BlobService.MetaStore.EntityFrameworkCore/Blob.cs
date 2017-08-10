using BlobService.Core.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class Blob : IBlob
    {
        [Key]
        public string Id { get; set; }
        public string OrigFileName { get; set; }
        public int SizeInBytes { get; set; }
        public string MimeType { get; set; }
        public string StorageSubject { get; set; }
        public string ContainerId { get; set; }
        public virtual Container Container { get; set; }
        public virtual IEnumerable<BlobMetaData> MetaData { get; set; } = new HashSet<BlobMetaData>();

        [NotMapped]
        IEnumerable<IBlobMetaData> IBlob.MetaData { get => MetaData; }
    }
}
