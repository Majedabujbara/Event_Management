using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EventManger.Core.Domain.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Missing Person's Name")]
        public string PersonName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Missing Email")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",ErrorMessage = "Please enter a valid email (e.g., user@example.com)")]
        [Remote(action: "IsEmailAlreadyRegistered", controller: "Account", ErrorMessage = "Email is already in use")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number missing")]
        [RegularExpression(@"^(\+962|0)(7[789]\d{7})$", ErrorMessage = "Invalid phone number format. Ensure it starts with +962 or 07.")]
        public string Phone { get; set; } = string.Empty; // Updated property name to `Phone`.

        [Required(ErrorMessage = "Missing password")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Missing Password Confirmation Field")]
        [Compare("Password", ErrorMessage = "Should match with the first password field")]
        public string ConfirmationPassword { get; set; } = string.Empty;
    }
}