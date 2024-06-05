using System.ComponentModel.DataAnnotations;

namespace FİLESAPI.Models
{
    public class Folder
    {

        public int Id { get; set; }
        public int ParentFolderId { get; set; }

        public string FolderName { get; set; }
        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
        public List<Fillies> Files { get; set; }
        public List<Folder> SubFolders { get; set; }

    }
}
