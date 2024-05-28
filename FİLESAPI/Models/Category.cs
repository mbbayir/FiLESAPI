
namespace FİLESAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Updated { get; internal set; }
        public DateTime Created { get; internal set; }
        public bool IsActive { get; internal set; }
    }
}