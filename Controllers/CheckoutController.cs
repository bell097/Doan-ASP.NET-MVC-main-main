using Doan_ASP.NET_MVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doan_ASP.NET_MVC.Controllers
{
    public class CheckoutController : Controller
    {
        private ShopModelContext db = new ShopModelContext();
        // GET: Checkout
        [Authorize]
        public ActionResult Checkout()
        {
           
            return View();
           
        }
        public ActionResult Order()
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            return PartialView("Order",giohang);
        } 

        public ActionResult thanhtoan(FormCollection f)
        {
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            long tongtien = giohang.Sum(m => m.ThanhTien);
            Bill bill = new Bill();
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            bill.user_id = user.Id;
            bill.name = f["firstname"] +" "+ f["lastname"] ;
            bill.phone = f["phone"];
            DateTime dt = DateTime.Now;
            String date1 = dt.ToString("yyyy/MM/dd");
            bill.date = DateTime.ParseExact(date1, "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            bill.address = f["address"];
            bill.total = tongtien;
            bill.payment = f["payment"];
            db.Bills.Add(bill);
            db.SaveChanges();
            foreach (var item in giohang)
            {
                BillDetail billDetail = new BillDetail();
                billDetail.product_id = item.SanPhamID;
                billDetail.soluong = item.SoLuong;
                billDetail.price = item.DonGia;
                billDetail.bill_id = bill.bill_id;
                db.BillDetails.Add(billDetail);
                db.SaveChanges();
            }
          
            Session["giohang"] = null;
            return RedirectToAction("Index", "Home");
        }

       
        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            List<CartItem> giohang = (List<CartItem>)Session["giohang"] ;
            //getting the apiContext
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal
                //Payer Id will be returned when payment proceeds or click to pay
                string payerId = Request.Params["PayerID"];

                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist
                    //it is returned by the create function call of the payment class

                    // Creating a payment
                    // baseURL is the url on which paypal sendsback the data.
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority +
                                "/Checkout/PaymentWithPayPal?";

                    //here we are generating guid for storing the paymentID received in session
                    //which will be used in the payment execution

                    var guid = Convert.ToString((new Random()).Next(100000));

                    //CreatePayment function gives us the payment approval url
                    //on which payer is redirected for paypal account payment

                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);

                    //get links returned from paypal in response to Create function call

                    var links = createdPayment.links.GetEnumerator();

                    string paypalRedirectUrl = string.Empty;

                    while (links.MoveNext())
                    {
                        Links link = links.Current;

                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment
                            paypalRedirectUrl = link.href;
                        }
                    }

                    // saving the paymentID in the key guid
                    Session.Add(guid, createdPayment.id);

                    return Redirect(paypalRedirectUrl);
                }
                else
                {

                    // This function exectues after receving all parameters for the payment

                    var guid = Request.Params["guid"];

                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    //If executed payment failed then we will show payment failure message to user
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        
                        return View("FailureView");
                    }
                }
            }
            catch (Exception ex)
            {
               
                PaypalLogger.Log("Error: " + ex.Message);
                return View("FailureView");
            }

            //on successful payment, show success page to user.
            Session.Remove("giohang");
            return View("SuccessView");
           
        }

        private Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {

            //create itemlist and add item objects to it
            var listItems = new ItemList() { items = new List<Item>()};

            //Adding Item Details like name, currency, price etc
            List<CartItem> giohang = Session["giohang"] as List<CartItem>;
            foreach (var cart in giohang) {
                listItems.items.Add(new Item()
                {
                    name = cart.TenSanPham,
                    currency = "USD",
                    price = (cart.DonGia/23000).ToString(),
                    quantity = cart.SoLuong.ToString(),
                    sku = "sku"
                });
            }
            var payer = new Payer() { payment_method = "paypal" };

            // Configure Redirect Urls here with RedirectUrls object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // Adding Tax, shipping and Subtotal details
            var details = new Details()
            {
                tax = "0",
                shipping = "1",
                subtotal = (giohang.Sum(m => m.ThanhTien) / 23000).ToString()
            };

            //Final amount with details
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(), // Total must be equal to sum of tax, shipping and subtotal.
                details = details
            };

            var transactionList = new List<Transaction>();
            // Adding description about the transaction
            transactionList.Add(new Transaction()
            {
                description = "Hieu Testing Transaction description",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = listItems
            });


            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            // Create a payment using a APIContext
            return payment.Create(apiContext);
        }

    }
}