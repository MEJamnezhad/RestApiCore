//using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ServiceLayer
{
    //  [Index(nameof(Email), IsUnique = true)]
    public class User : BaseEntity<User>, INonDeletable, INonActive
    {

        [Required]
        public string FirstName { get; set; }=string.Empty;
        [Required]
        public string LastName { get; set; }= string.Empty;


        [Required(ErrorMessage = "Please enter email")]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Minimum length is 8 charecter")]
        public string Password { get; set; } = string.Empty;

        public Role Role { get; set; }
        public DateTime BirthDay { get; set; }= DateTime.Now;
        public string? Mobile { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        // If you don't want Articles data to appear in the resulting JSON
        [JsonIgnore]
        public virtual ICollection<Article>? Articles { get; set; }





        #region Helper
     

        #endregion

    }
}
