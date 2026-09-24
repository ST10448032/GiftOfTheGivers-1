namespace GiftOfTheGivers.Helpers
{
    public static class DonationHelper
    {
        public static decimal CalculateTotalDonation(
            decimal amount,
            decimal additionalAmount = 0)
        {
            return amount + additionalAmount;
        }

        public static string FormatTaxCertificateNumber(int donationId)
        {
            return $"TAX-{donationId:D6}";
        }
    }
}
