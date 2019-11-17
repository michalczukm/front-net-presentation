using System.ComponentModel.DataAnnotations;

namespace FrontNet.Playground
{
    public class PersonModel
    {
        [Required]
        [Range(18, 60, ErrorMessage = "Either too old or too young :(")]
        public int Age { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(15)]
        public string FullName { get; set; }

        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string RepeatPassword { get; set; }
    }
}
