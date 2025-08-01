using System.ComponentModel.DataAnnotations;

namespace PharmaMoov.Models.Attachment
{
    public class Attachment : APIBaseModel
    {
        [Key]
        public int AttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentFileName { get; set; }
        public string AttachmentUploadedFileName { get; set; }
        public string AttachmentType { get; set; }
        public string AttachmentServerPhysicalPath { get; set; }
        public string AttachmentExternalUrl { get; set; }
        public UploadTypes UType { get; set; }
    }
}
