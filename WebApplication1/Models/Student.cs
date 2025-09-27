using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Student
    {
        public int Id { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        // Common address attributes
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        // Common contact information
        [Phone(ErrorMessage = "Please enter a valid mobile phone number.")]
        [StringLength(20, ErrorMessage = "Mobile phone number cannot be longer than 20 characters.")]
        [Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid home phone number.")]
        [StringLength(20, ErrorMessage = "Home phone number cannot be longer than 20 characters.")]
        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; } = string.Empty;

        [Display(Name = "Emergency Contact Name")]
        public string EmergencyContactName { get; set; } = string.Empty;

        [Display(Name = "Emergency Contact Name")]
        public string EmergencyContactPhone { get; set; } = string.Empty;

        // Navigation property for the list of courses
        public List<Course> Courses { get; set; } = new();

        public string FullName => $"{FirstName} {LastName}";
    }
}
