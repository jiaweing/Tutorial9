using Braintree;

namespace Web.Services
{
    public class BraintreeService
    {
        private readonly IBraintreeGateway _gateway;

        public BraintreeService(IConfiguration configuration)
        {
            var environment = configuration["Web:Braintree:Environment"];
            var merchantId = configuration["Web:Braintree:MerchantId"];
            var publicKey = configuration["Web:Braintree:PublicKey"];
            var privateKey = configuration["Web:Braintree:PrivateKey"];

            _gateway = new BraintreeGateway(environment, merchantId, publicKey, privateKey);
        }

        public IBraintreeGateway GetGateway()
        {
            return _gateway;
        }
    }
}
