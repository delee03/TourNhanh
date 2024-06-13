using TourNhanh.Services.VnPay.Helper;
using TourNhanh.ViewModel;

namespace TourNhanh.Services.VnPay
{
	public class VnPayService : IVnPayService
	{
		private IConfiguration _config;

		public VnPayService(IConfiguration config)
		{
			_config = config;
		}

		public string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model)
		{
			var tick = DateTime.Now.Ticks.ToString();

			var vnpay = new VnPayLibrary();
			vnpay.AddRequestData("vnp_Version", _config["VnPay:Version"] ?? string.Empty);
			vnpay.AddRequestData("vnp_Command", _config["VnPay:Command"] ?? string.Empty);
			vnpay.AddRequestData("vnp_TmnCode", _config["VnPay:TmnCode"] ?? string.Empty);
			vnpay.AddRequestData("vnp_Amount", (model.Amount * 100).ToString());

			vnpay.AddRequestData("vnp_CreateDate", model.CreatedDate.ToString("yyyyMMddHHmmss"));
			vnpay.AddRequestData("vnp_CurrCode", _config["VnPay:CurrCode"] ?? string.Empty);
			vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress(context) ?? string.Empty);
			vnpay.AddRequestData("vnp_Locale", _config["VnPay:Locale"] ?? string.Empty);

			vnpay.AddRequestData("vnp_OrderInfo", "Thanh toán cho đơn hàng:" + model.BookingId);
			vnpay.AddRequestData("vnp_OrderType", "other");
			vnpay.AddRequestData("vnp_ReturnUrl", _config["VnPay:PaymentBackReturnUrl"] ?? string.Empty);

			vnpay.AddRequestData("vnp_TxnRef", tick);

			var paymentUrl = vnpay.CreateRequestUrl(_config["VnPay:BaseUrl"] ?? string.Empty, _config["VnPay:HashSecret"] ?? string.Empty);

			return paymentUrl;
		}


		public VnPaymentResponseModel PaymentExecute(IQueryCollection collections)
		{
			var vnpay = new VnPayLibrary();
			foreach (var (key, value) in collections)
			{
				if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
				{
					vnpay.AddResponseData(key, value.ToString());
				}
			}

			var vnp_orderId = Convert.ToInt64(vnpay.GetResponseData("vnp_TxnRef"));
			var vnp_TransactionId = Convert.ToInt64(vnpay.GetResponseData("vnp_TransactionNo"));
			var vnp_SecureHash = collections.FirstOrDefault(p => p.Key == "vnp_SecureHash").Value.ToString();
			var vnp_ResponseCode = vnpay.GetResponseData("vnp_ResponseCode");
			var vnp_OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");

			bool checkSignature = vnpay.ValidateSignature(vnp_SecureHash ?? string.Empty, _config["VnPay:HashSecret"] ?? string.Empty);
			if (!checkSignature)
			{
				return new VnPaymentResponseModel
				{
					Success = false
				};
			}

			return new VnPaymentResponseModel
			{
				Success = true,
				PaymentMethod = "VnPay",
				BookingDesc = vnp_OrderInfo,
				BookingId = vnp_orderId.ToString(),
				TransactionId = vnp_TransactionId.ToString(),
				Token = vnp_SecureHash,
				VnPayResponseCode = vnp_ResponseCode
			};
		}
	}
}
