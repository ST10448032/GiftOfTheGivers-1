using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class Donation
    {
        public int DonationID { get; set; }

        [Required]
        [Range(1, 100000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; } = "ZAR";

        [Required]
        public string DonationType { get; set; } = "One-Time";

        public DateTime DonationDate { get; set; } = DateTime.Now;

        public bool IsAnonymous { get; set; }

        // Stores the Identity User who made the donation
        public string? UserID { get; set; }

        // Unique donation reference number
        public string? ReferenceNumber { get; set; }
    }
}