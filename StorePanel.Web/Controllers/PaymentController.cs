using Dto.Other;
using Dto.Payment;
using Dto.Response.Payment;
using Microsoft.AspNetCore.Mvc;
using StorePanel.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZarinPal.Class;

namespace StorePanel.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly Payment _payment;
        private readonly Authority _authority;
        private readonly Transactions _transactions;
        private readonly IInvoicesRepository _invoicesRepository;
        private readonly ICustomersRepository _customersRepository;
        private readonly IBasicdefinitionsRepository _basicdefinitionsRepository;

        public PaymentController(IInvoicesRepository invoicesRepository
        ,ICustomersRepository customersRepository
        ,IBasicdefinitionsRepository basicdefinitionsRepository)
        {
            var expose = new Expose();
            _payment = expose.CreatePayment();
            _authority = expose.CreateAuthority();
            _transactions = expose.CreateTransactions();
            _invoicesRepository = invoicesRepository;
            _customersRepository = customersRepository;
            _basicdefinitionsRepository = basicdefinitionsRepository;
        }
        /// <summary>
        /// ﻓﺮﺍﻳﻨﺪ ﺧﺮﻳﺪ
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> PaymentRequest(int id)
        {

            var payment = await _invoicesRepository.GetById(id);
            var amount = (int)payment.TotalPrice * 10; // Toman to rial
            var customer = _customersRepository.GetCustomer(payment.CustomerId);
            //var refererStr = referer != null ? $"?referer={referer}" : null;
            string description = $"پرداخت فاکتور شماره {payment.Id} ";
            var BasicDefinitions = await _basicdefinitionsRepository.GetById(1);
            var result = await _payment.Request(new DtoRequest()
            {
                Mobile = customer.User.PhoneNumber,
                //CallbackUrl = $"{baseUrl}/Payment/PaymentResponse/{payment.Id}{refererStr}",
                CallbackUrl = $"http://localhost:10759/Payment/PaymentResponse/{payment.Id}",
                Description = description,
                Email = customer.User.Email,
                Amount = amount,
                MerchantId = BasicDefinitions.ZarinPalApi
            }, ZarinPal.Class.Payment.Mode.zarinpal);
            return Redirect($"https://zarinpal.com/pg/StartPay/{result.Authority}");
        }
        /// <summary>
        /// بازگشت از درگاه
        /// </summary>
        /// <param name="authority"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<IActionResult> PaymentResponse(int id, string authority, string status)
        {

            var invoices = await _invoicesRepository.GetById(id);
            var amount = (int)invoices.TotalPrice * 10;
            var BasicDefinitions = await _basicdefinitionsRepository.GetById(1);
            var verification = await _payment.Verification(new DtoVerification
            {
                Amount = amount,
                MerchantId = BasicDefinitions.ZarinPalApi,
                Authority = authority
            }, Payment.Mode.sandbox);

            var result = await ProccessPayment(invoices, verification);

            ViewBag.result = result;

            return View(invoices);
        }

        public async Task<bool> ProccessPayment(Core.Models.Invoice payment, Verification verification)
        {
            var validation = false;
            // success
            if (verification.Status == 100)
            {
                validation = true;
                payment.IsPayed = true;
            }
            else
            {
                payment.IsPayed = false;
            }

            await _invoicesRepository.Update(payment);
            return validation;
        }
        /// <summary>
        /// ﻓﺮﺍﻳﻨﺪ ﺧﺮﻳﺪ ﺑﺎ ﺗﺴﻮﻳﻪ ﺍﺷﺘﺮﺍﻛﻲ 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> RequestWithExtra()
        {
            var BasicDefinitions = await _basicdefinitionsRepository.GetById(1);
            var result = await _payment.Request(new DtoRequestWithExtra()
            {
                Mobile = "09121112222",
                CallbackUrl = "http://localhost:44310/home/validate",
                Description = "توضیحات",
                Email = "farazmaan@outlook.com",
                Amount = 1000000,
                MerchantId = BasicDefinitions.ZarinPalApi,
                AdditionalData = "{\"Wages\":{\"zp.1.1\":{\"Amount\":120,\"Description\":\" ﺗﻘﺴﻴﻢ \"}, \" ﺳﻮﺩ ﺗﺮﺍﻛﻨﺶ zp.2.5\":{\"Amount\":60,\"Description\":\" ﻭﺍﺭﻳﺰ \"}}} "
            }, ZarinPal.Class.Payment.Mode.zarinpal);
            return Redirect($"https://zarinpal.com/pg/StartPay/{result.Authority}");
        }

        /// <summary>
        /// ﺩﺭ ﺭﻭﺵ ﺍﻳﺠﺎﺩ ﺷﻨﺎﺳﻪ ﭘﺮﺩﺍﺧﺖ ﺑﺎ ﻃﻮﻝ ﻋﻤﺮ ﺑﺎﻻ ﻣﻤﻜﻦ ﺍﺳﺖ ﺣﺎﻟﺘﻲ ﭘﻴﺶ ﺁﻳﺪ ﻛﻪ ﺷﻤﺎ ﺑﻪ ﺗﻤﺪﻳﺪ ﺑﻴﺸﺘﺮ ﻃﻮﻝ ﻋﻤﺮ ﻳﻚ ﺷﻨﺎﺳﻪ ﭘﺮﺩﺍﺧﺖ ﻧﻴﺎﺯ ﺩﺍﺷﺘﻪ ﺑﺎﺷﻴﺪ
        /// ﺩﺭ ﺍﻳﻦ ﺻﻮﺭﺕ ﻣﻲ ﺗﻮﺍﻧﻴﺪ ﺍﺯ ﻣﺘﺪ زیر ﺍﺳﺘﻔﺎﺩﻩ ﻧﻤﺎﻳﻴﺪ 
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> RefreshAuthority()
        {
            var BasicDefinitions = await _basicdefinitionsRepository.GetById(1);
            var refresh = await _authority.Refresh(new DtoRefreshAuthority
            {
                Authority = "",
                ExpireIn = 1,
                MerchantId = BasicDefinitions.ZarinPalApi
            }, Payment.Mode.zarinpal);
            return View();
        }

        /// <summary>
        /// ﻣﻤﻜﻦ ﺍﺳﺖ ﺷﻤﺎ ﻧﻴﺎﺯ ﺩﺍﺷﺘﻪ ﺑﺎﺷﻴﺪ ﻛﻪ ﻣﺘﻮﺟﻪ ﺷﻮﻳﺪ ﭼﻪ ﭘﺮﺩﺍﺧﺖ ﻫﺎﻱ ﺗﻮﺳﻂ ﻭﺏ ﺳﺮﻭﻳﺲ ﺷﻤﺎ ﺑﻪ ﺩﺭﺳﺘﻲ ﺍﻧﺠﺎﻡ ﺷﺪﻩ ﺍﻣﺎ ﻣﺘﺪ  ﺭﻭﻱ ﺁﻧﻬﺎ ﺍﻋﻤﺎﻝ ﻧﺸﺪﻩ
        /// ، ﺑﻪ ﻋﺒﺎﺭﺕ ﺩﻳﮕﺮ ﺍﻳﻦ ﻣﺘﺪ ﻟﻴﺴﺖ ﭘﺮﺩﺍﺧﺖ ﻫﺎﻱ ﻣﻮﻓﻘﻲ ﻛﻪ ﺷﻤﺎ ﺁﻧﻬﺎ ﺭﺍ ﺗﺼﺪﻳﻖ ﻧﻜﺮﺩﻩ ﺍﻳﺪ ﺭﺍ ﺑﻪ PaymentVerification ﺷﻤﺎ ﻧﻤﺎﻳﺶ ﻣﻲ ﺩﻫﺪ.
        /// </summary>
        /// <returns></returns>

        public async Task<IActionResult> Unverified()
        {
            var BasicDefinitions = await _basicdefinitionsRepository.GetById(1);
            var refresh = await _transactions.GetUnverified(new DtoMerchant
            {
                MerchantId = BasicDefinitions.ZarinPalApi
            }, Payment.Mode.zarinpal);
            return View();
        }
    }
}
