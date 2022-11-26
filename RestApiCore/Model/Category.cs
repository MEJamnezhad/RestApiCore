using Microsoft.Build.Framework;

namespace RestApiCore.Model
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreationDate { get; set; }

       // public virtual ICollection<Article>? Articles { get; set; }
        public Category()
        {
            IsActive = true;
            CreationDate = DateTime.Now;
        }
    }
}
