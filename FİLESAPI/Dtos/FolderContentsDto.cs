namespace FİLESAPI.Dtos
{
    public class FolderContentsDto
    {
        public string FolderName { get; set; }
        public List<FilliesDto> Files { get; set; }
        public List<FolderDto> SubFolders { get; set; }
    }
}
