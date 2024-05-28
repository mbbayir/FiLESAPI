namespace FİLESAPI.Dtos
{
    public class Folderfilles
    {
        public int Id { get; set; }

        public string FolderName { get; set; }

        public FilliesDto[] Fillies { get; set; }
    }
}
