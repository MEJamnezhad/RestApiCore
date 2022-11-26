using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RestApiCore.Model
{
    public class Article
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Text { get; set; }

        public string Picture { get; set; }
        public string DocumentPath { get; set; }


        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public int Visit { get; set; }

        public DateTime CreationDate { get; set; }
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
