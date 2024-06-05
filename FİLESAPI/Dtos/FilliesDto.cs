namespace FİLESAPI.Dtos
{
    public class FilliesDto
    {
        public int Id { get; set; }
        public int FolderId { get; set; }
        public string UserID { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }

        public DateTime Updated { get; set; }

        public DateTime Created { get; set; }
    }
}
