using System.ComponentModel.DataAnnotations;

namespace ServiceLayer
{
    public class Category : BaseEntity<Category>, INonDeletable, INonActive
    {

        [Required]
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;


        // public virtual ICollection<Article>? Articles { get; set; }
        //public Category()
        //{
        //    IsDeleted= false;
        //    IsActive = true;
        //}
    }
}
