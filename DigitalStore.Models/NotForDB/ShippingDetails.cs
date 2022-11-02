using System.ComponentModel.DataAnnotations;

namespace DigitalStore.Models.NotForDB
{
    public class ShippingDetails
    {
        public string Name { get; set; }

        [Required(ErrorMessage = "Set address for delivery")]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }

        [Required(ErrorMessage = "Set city")]
        public string City { get; set; }
        [Required(ErrorMessage = "Set country")]
        public string Country { get; set; }
        public bool GiftWrap { get; set; }
    }
}
