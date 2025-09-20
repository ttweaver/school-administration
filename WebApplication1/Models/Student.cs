using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
		public DateTime DateOfBirth { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;
		public int? ClassroomId { get; set; }
        public Course? Course { get; set; }

        // Common address attributes
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;

        // Common contact information
        [Phone(ErrorMessage = "Please enter a valid mobile phone number.")]
        [StringLength(20, ErrorMessage = "Mobile phone number cannot be longer than 20 characters.")]
        public string MobilePhone { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Please enter a valid home phone number.")]
        [StringLength(20, ErrorMessage = "Home phone number cannot be longer than 20 characters.")]
        public string HomePhone { get; set; } = string.Empty;
        public string EmergencyContactName { get; set; } = string.Empty;
        public string EmergencyContactPhone { get; set; } = string.Empty;
    }
}
