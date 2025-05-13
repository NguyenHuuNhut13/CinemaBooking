namespace CinemaBooking.ViewModels
{
    public class PaymentWithQRViewModel
    {
        public string BankKey { get; set; }
        public string AccountNumber { get; set; }
        public int Amount { get; set; }
        public string Purpose { get; set; }
        public string QRContent { get; set; } // Stores the generated QR code content
    }

}
