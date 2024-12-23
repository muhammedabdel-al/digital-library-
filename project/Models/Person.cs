using System.ComponentModel.DataAnnotations;

namespace project.Models
{
    public class Person
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Discription is required")]
        public string Discription { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string PhoneNumber { get; set; }
        public string imgfile { get; set; }

        public string Role { get; set; }

        [Range(0, 120, ErrorMessage = "Age must be between 0 and 120")]
        public int Age { get; set; }
    }
}
