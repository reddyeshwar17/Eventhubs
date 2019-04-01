using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPIDemo1.Models
{
    public class PersonalDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AutoId { get; set; }

        [StringLength(20, MinimumLength = 4, ErrorMessage = "Must be at least 4 characters long.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please write your LastName")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public int? Age { get; set; }

        [Display(Name = "Is Active?")]
        public bool? Active { get; set; }
    }
}