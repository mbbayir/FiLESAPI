namespace FİLESAPI.Dtos
{
    public class EFolderFileDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long Size { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
