using System.ComponentModel.DataAnnotations;
using Core.Constants;
using Domain.Common.Configurations;

namespace Domain.Entities
{
    public class FileStore : Entity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(StringLengthConstants.LengthSm)]
        public string Name { get; set; }
        [StringLength(StringLengthConstants.LengthSm)]
        public string FileName { get; set; }
        [StringLength(StringLengthConstants.LengthSm)]
        public string UniqueFileName { get; set; }
        [Required]
        [StringLength(8)]
        public string FileExtension { get; set; }
        [Required]
        [StringLength(StringLengthConstants.LengthXs)]
        public string ContentType { get; set; }
        public long SizeInBytes { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string ProjectPath { get; set; }

    }
}
