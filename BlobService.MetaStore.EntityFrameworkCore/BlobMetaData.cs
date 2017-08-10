using BlobService.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlobService.MetaStore.EntityFrameworkCore
{
    public class BlobMetaData : IBlobMetaData
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public string BlobId { get; set; }

        [ForeignKey("BlobId")]
        public virtual Blob Blob { get; set; }
    }
}
