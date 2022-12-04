//using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServiceLayer
{
    //  [Index(nameof(Email), IsUnique = true)]
    public class User : BaseEntity<User>, INonDeletable
    {
          
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimum length is 8 charecter")]
        public string Password { get; set; }

        public Role Role { get; set; }
        public DateTime BirthDay { get; set; }
        public string? Mobile { get; set; }
        public bool IsActie { get; set; }
        public bool IsDeleted { get; set; }

        // If you don't want Articles data to appear in the resulting JSON
        [JsonIgnore]
        public virtual ICollection<Article>? Articles { get; set; }


        public User()
        {
            IsDeleted = false;
            IsActie = true;
            CreationDate= DateTime.Now;
        }
    }
}
