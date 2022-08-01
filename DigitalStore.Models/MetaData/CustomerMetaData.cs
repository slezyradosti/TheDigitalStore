using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models.MetaData
{
    public class CustomerMetaData
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Mid Name")]
        public string MidName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(10, ErrorMessage = "Please enter a value less than 10 characters long.")]
        public string PhoneNumber;
    }
}