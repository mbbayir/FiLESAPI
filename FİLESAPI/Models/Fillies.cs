namespace FİLESAPI.Models
{
    public class Fillies
    {
        public int Id { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int FileSize { get; set; }

        public DateTime Updated { get; set; }

        public DateTime Created { get; set; }
        
        public int FolderId { get; set; }

        public Folder Folder { get; set; }
        public string UserID { get; set; }
        public AppUser User { get; set; }
    }
}
