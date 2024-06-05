using Microsoft.AspNetCore.Http;

namespace FİLESAPI.Dtos
{
    public class FolderDto
    {
        public int Id { get; set; }
        public string FolderName { get; set; }
        public string ParentFolderName { get; set; }
        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }

    }
}
