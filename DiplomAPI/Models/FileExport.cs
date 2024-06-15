namespace DiplomAPI.Models
{
    public class FileExport
    {
        public int FileId { get; set; }
        public string? FileName { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserLogin { get; set; } = null!;
        public int? GroupNumber { get; set; }
        public byte[]? FileData { get; set; }
    }
}
