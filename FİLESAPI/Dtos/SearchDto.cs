namespace FİLESAPI.Dtos
{
    public class SearchDto
    {
        public string Query { get; set; }
        public bool IncludeFiles { get; set; } = true;
        public bool IncludeFolders { get; set; } = true;
    }

}
