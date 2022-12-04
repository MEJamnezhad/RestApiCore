using System.ComponentModel.DataAnnotations;

namespace ServiceLayer
{
    public class Category : BaseEntity<Category>, INonDeletable
    {

        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }


        // public virtual ICollection<Article>? Articles { get; set; }
        public Category()
        {
            IsDeleted= false;
            IsActive = true;
          //  CreationDate = DateTime.Now;
        }
    }
}
