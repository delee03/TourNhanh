namespace TourNhanh.ViewModel
{
	public class VnPaymentResponseModel
	{
		public bool Success { get; set; }
		public string? PaymentMethod { get; set; }
		public string? BookingDesc { get; set; }
		public string? BookingId { get; set; }
		public string? PaymentId { get; set; }
		public string? TransactionId { get; set; }
		public string? Token { get; set; }
		public string? VnPayResponseCode { get; set; }

		public string? PaymentBackReturnUrl { get; set; }

	}
	public class VnPaymentRequestModel
	{
		public int BookingId { get; set; }
		public string? FullName { get; set; }
		public string? Desc { get; set; }
		public double Amount { get; set; }
		public DateTime CreatedDate { get; set; }
		public string? PaymentBackReturnUrl { get; set; }
	}
}
