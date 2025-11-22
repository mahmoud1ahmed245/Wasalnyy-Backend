using AutoMapper;
using Stripe;
using Stripe.Checkout;
using Wasalnyy.BLL.DTO.Payment;
using Wasalnyy.DAL.Repo.Implementation;

namespace Wasalnyy.BLL.Service.Implementation
{
	public class PaymentService : IPaymentService
	{
		private readonly IConfiguration config;
		private readonly IPaymentGetwayRepo paymentGetwayRepo;
        private readonly IMapper _mapper;
        public PaymentService(IConfiguration config, IPaymentGetwayRepo  paymentGetwayRepo, IMapper mapper)
		{
			this.config = config;
			StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
			this.paymentGetwayRepo = paymentGetwayRepo;
            this._mapper= mapper;
        }
		public async Task<string> CreatePaymentSession(decimal amount, string currency, string successUrl, string cancelUrl)
		{
			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string> { "card" },
				LineItems = new List<SessionLineItemOptions>
			{
				new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						UnitAmount = (long)(amount * 100), // amount in cents
                        Currency = currency,
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = "Ride Payment",
						},
					},
					Quantity = 1,
				},
			},
				Mode = "payment",
				SuccessUrl = successUrl,
				CancelUrl = cancelUrl
			};

			var service = new SessionService();
			var session = await service.CreateAsync(options);
			return session.Url;
		}

		public async Task<RiderPaymentSuccessResponse> HandleRiderPayment(RiderPaymentDetailsDTO paymentDetails)
		{
			// Map DTO to entity
			var paymentEntity = _mapper.Map<GatewayPayment>(paymentDetails);

			try 
			{
                // Add payment using repository
                var success = await paymentGetwayRepo.AddPaymentAsync(paymentEntity);

            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return new RiderPaymentSuccessResponse(false, $"An error occurred while processing payment: {innerMessage}");
            }
            return new RiderPaymentSuccessResponse(true, "Payment processed successfully."); ;

        }

    }
}
