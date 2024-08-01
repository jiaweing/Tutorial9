using Braintree;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;

namespace Web.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly Services.BraintreeService _braintreeService;

        public PaymentController(Services.BraintreeService braintreeService)
        {
            _braintreeService = braintreeService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var gateway = _braintreeService.GetGateway();
            var clientToken = await gateway.ClientToken.GenerateAsync();
            var model = new PaymentModel { ClientToken = clientToken };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string paymentMethodNonce, decimal amount)
        {
            var request = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = paymentMethodNonce,
            };

            var gateway = _braintreeService.GetGateway();
            var result = await gateway.Transaction.SaleAsync(request);

            if (result.Target != null && result.Target.Status == TransactionStatus.AUTHORIZED)
            {
                return RedirectToAction("Success");
            }
            else
            {
                foreach (var error in result.Errors.DeepAll())
                {
                    Console.WriteLine(error.Message);
                }

                return RedirectToAction("Failure");
            }
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Failure()
        {
            return View();
        }
    }
}