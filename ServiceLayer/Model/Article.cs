using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ServiceLayer
{
    public class Article : BaseEntity<Article> , INonDeletable
    {
       
        [Required]
        public string Title { get; set; }=String.Empty;
        [Required]
        public string Text { get; set; }=String.Empty;

        public string Picture { get; set; } = String.Empty;
        public string DocumentPath { get; set; }=String.Empty;


        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public int Visit { get; set; }

        public int CategoryId { get; set; }

        // If you don't want Category data to appear in the resulting JSON
        [JsonIgnore]
        public virtual Category Category { get; set; }

        public int WriterId { get; set; }

        // If you don't want User data to appear in the resulting JSON
        [JsonIgnore]
        public virtual User Writer { get; set; }

        public Article()
        {
            IsDeleted = false;
            IsActive = true;
            Visit = 0;
            CreationDate = DateTime.Now;
        }

    }
}
