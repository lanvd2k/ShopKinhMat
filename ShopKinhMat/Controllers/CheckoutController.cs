using API_MoMo;
using API_NganLuong;
using Newtonsoft.Json.Linq;
using ShopBanDo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopBanDo.Controllers
{
    public class CheckoutController : BaseController
    {
        // GET: Checkout
        private const string SessionCart = "SessionCart";
        private ShopBanDoDbContext db = new ShopBanDoDbContext();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ActionResult Index()
        {
            var cart = Session[SessionCart];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            else if (cart == null)
            {
                ViewBag.statusCart = "Giỏ hàng trống";

            }
            return View(list);
        }

        [HttpPost]
        public ActionResult Index(modelOrder order)
        {
            Random rand = new Random((int)DateTime.Now.Ticks);
            int numIterations = 0;
            numIterations = rand.Next(1, 100000);
            DateTime time = DateTime.Now;
            string orderCode = XString.ToStringNospace(numIterations + "" + time);
            string sumOrder = Request["sumOrder"];
            string payment_method = Request["option_payment"];
            // Neu Ship COde
            if (payment_method.Equals("COD"))
            {
                // cap nhat thong tin sau khi dat hang thanh cong

                saveOrder(order, "COD", 2, orderCode);
                var cart = Session[SessionCart];
                var list = new List<CartItem>();
                ViewBag.cart = (List<CartItem>)cart;
                ViewBag.statusCod = 1;
                ViewBag.Methodpayment = "COD";
                Session["SessionCart"] = null;
                var listProductOrder = db.Orders.Where(m => m.Id == order.Id).FirstOrDefault();
                return View("oderComplete", listProductOrder);
            }
            //Neu Thanh toan MOMO
            else if (payment_method.Equals("MOMO"))
            {
                //request params need to request to MoMo system
                string endpoint = momoInfo.endpoint;
                string partnerCode = momoInfo.partnerCode;
                string accessKey = momoInfo.accessKey;
                string serectkey = momoInfo.serectkey;
                string orderInfo = momoInfo.orderInfo;
                string returnUrl = momoInfo.returnUrl;
                string notifyurl = momoInfo.notifyurl;

                string amount = sumOrder;
                string orderid = Guid.NewGuid().ToString();
                string requestId = Guid.NewGuid().ToString();
                string extraData = "";

                //Before sign HMAC SHA256 signature
                string rawHash = "partnerCode=" +
                    partnerCode + "&accessKey=" +
                    accessKey + "&requestId=" +
                    requestId + "&amount=" +
                    amount + "&orderId=" +
                    orderid + "&orderInfo=" +
                    orderInfo + "&returnUrl=" +
                    returnUrl + "&notifyUrl=" +
                    notifyurl + "&extraData=" +
                    extraData;

                log.Debug("rawHash = " + rawHash);

                MoMoSecurity crypto = new MoMoSecurity();
                //sign signature SHA256
                string signature = crypto.signSHA256(rawHash, serectkey);
                log.Debug("Signature = " + signature);

                //build body json request
                JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };
                log.Debug("Json request to MoMo: " + message.ToString());
                string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
                JObject jmessage = JObject.Parse(responseFromMomo);

                saveOrder(order, "Cổng thanh toán MOMO", 2, orderid);
                return Redirect(jmessage.GetValue("payUrl").ToString());
            }
            //Neu Thanh toan Ngan Luong
            else if (payment_method.Equals("NL"))
            {
                string str_bankcode = Request["bankcode"];
                RequestInfo info = new RequestInfo();
                info.Merchant_id = nganluongInfo.Merchant_id;
                info.Merchant_password = nganluongInfo.Merchant_password;
                info.Receiver_email = nganluongInfo.Receiver_email;
                info.cur_code = "vnd";
                info.bank_code = str_bankcode;
                info.Order_code = orderCode;
                info.Total_amount = sumOrder;
                info.fee_shipping = "0";
                info.Discount_amount = "0";
                info.order_description = "Thanh toán ngân lượng cho đơn hàng";
                info.return_url = nganluongInfo.return_url;
                info.cancel_url = nganluongInfo.cancel_url;
                info.Buyer_fullname = order.DeliveryName;
                info.Buyer_email = order.DeliveryEmail;
                info.Buyer_mobile = order.DeliveryPhone;
                APICheckoutV3 objNLChecout = new APICheckoutV3();
                ResponseInfo result = objNLChecout.GetUrlCheckout(info, payment_method);
                // neu khong gap loi gi
                if (result.Error_code == "00")
                {
                    saveOrder(order, "Cổng thanh toán Ngân Lượng", 2, orderCode);
                    // chuyen sang trang ngan luong
                    return Redirect(result.Checkout_url);
                }
                else
                {
                    ViewBag.errorPaymentOnline = result.Description;
                    return View("payment");
                }

            }
            //Neu Thanh Toán ATM online
            else if (payment_method.Equals("ATM_ONLINE"))
            {
                string str_bankcode = Request["bankcode"];
                RequestInfo info = new RequestInfo();
                info.Merchant_id = nganluongInfo.Merchant_id;
                info.Merchant_password = nganluongInfo.Merchant_password;
                info.Receiver_email = nganluongInfo.Receiver_email;
                info.cur_code = "vnd";
                info.bank_code = str_bankcode;
                info.Order_code = orderCode;
                info.Total_amount = sumOrder;
                info.fee_shipping = "0";
                info.Discount_amount = "0";
                info.order_description = "Thanh toán ngân lượng cho đơn hàng";
                info.return_url = nganluongInfo.return_url;
                info.cancel_url = nganluongInfo.cancel_url;
                info.Buyer_fullname = order.DeliveryName;
                info.Buyer_email = order.DeliveryEmail;
                info.Buyer_mobile = order.DeliveryPhone;
                APICheckoutV3 objNLChecout = new APICheckoutV3();
                ResponseInfo result = objNLChecout.GetUrlCheckout(info, payment_method);
                // neu khong gap loi gi
                if (result.Error_code == "00")
                {
                    saveOrder(order, "ATM Online qua ngân lượng", 2, orderCode);
                    return Redirect(result.Checkout_url);
                }
                else
                {
                    ViewBag.errorPaymentOnline = result.Description;
                    return View("payment");
                }
            }
            return View("payment");
        }
        //Khi huy thanh toán Ngan Luong
        public ActionResult cancel_order()
        {
            return View();
        }
        //Khi thanh toán Ngan Luong XOng
        public ActionResult confirm_orderPaymentOnline()
        {

            String Token = Request["token"];
            RequestCheckOrder info = new RequestCheckOrder();
            info.Merchant_id = nganluongInfo.Merchant_id;
            info.Merchant_password = nganluongInfo.Merchant_password;
            info.Token = Token;
            APICheckoutV3 objNLChecout = new APICheckoutV3();
            ResponseCheckOrder result = objNLChecout.GetTransactionDetail(info);
            if (result.errorCode == "00")
            {
                String codeOrder = Session["OrderId"].ToString();
                var OrderInfo = db.Orders.OrderByDescending(m => m.Code == codeOrder).FirstOrDefault();
                OrderInfo.StatusPayment = 1;
                db.Entry(OrderInfo).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.paymentStatus = OrderInfo.StatusPayment;
                ViewBag.Methodpayment = OrderInfo.DeliveryPaymentMethod;
                return View("oderComplete", OrderInfo);
            }
            else
            {
                ViewBag.status = false;
            }

            return View("confirm_orderPaymentOnline");
        }

        //Khi huy thanh toán MOMO
        public ActionResult cancel_order_momo()
        {
            return View();
        }
        //Khi Thanh toám momo xong
        public ActionResult confirm_orderPaymentOnline_momo()
        {

            String errorCode = Request["errorCode"];
            String orderId = Request["orderId"];
            if (errorCode == "0")
            {
                Session["SessionCart"] = null;
                var OrderInfo = db.Orders.Where(m => m.Code == orderId).FirstOrDefault();
                OrderInfo.StatusPayment = 1;
                db.Entry(OrderInfo).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.paymentStatus = OrderInfo.StatusPayment;
                ViewBag.Methodpayment = OrderInfo.DeliveryPaymentMethod;
                return View("oderComplete", OrderInfo);
            }
            else
            {
                ViewBag.status = false;
            }

            return View("confirm_orderPaymentOnline");
        }
        //function ssave order when order success!
        public void saveOrder(modelOrder order, string paymentMethod, int StatusPayment, string ordercode)
        {
            var cart = Session[SessionCart];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }

            if (ModelState.IsValid)
            {
                order.Code = ordercode;
                order.CustemerId = 1;
                order.DeliveryPaymentMethod = paymentMethod;
                order.StatusPayment = StatusPayment;
                order.CreateDate = DateTime.Now;
                order.updated_by = 1;
                order.updated_at = DateTime.Now;
                order.updated_by = 1;
                order.Status = 2;
                order.ExportDate = DateTime.Now;
                db.Orders.Add(order);
                db.SaveChanges();
                Session["OrderId"] = ordercode;
                ViewBag.name = order.DeliveryName;
                ViewBag.email = order.DeliveryEmail;
                ViewBag.address = order.DeliveryAddress;
                ViewBag.code = order.Code;
                ViewBag.phone = order.DeliveryPhone;
                modelOrderdetail orderdetail = new modelOrderdetail();

                foreach (var item in list)
                {
                    float price = 0;
                    int sale = 0;
                    if (sale > 0)
                    {
                        price = (float)item.product.Price_sale * item.quantity;
                    }
                    else
                    {
                        price = (float)item.product.Price * (int)item.quantity;
                    }
                    orderdetail.OrderId = order.Id;
                    orderdetail.ProductId = item.product.Id;
                    orderdetail.priceSale = (int)item.product.Price_sale;
                    orderdetail.Price = item.product.Price;
                    orderdetail.Quantity = item.quantity;
                    orderdetail.Amount = price;

                    db.Orderdetails.Add(orderdetail);
                    db.SaveChanges();
                    var updatedProduct = db.Products.Find(item.product.Id);
                    db.Products.Attach(updatedProduct);
                    db.Entry(updatedProduct).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
        }
        //

        public ActionResult productDetailCheckOut(int orderId)
        {
            var list = db.Orderdetails.Where(m => m.OrderId == orderId).ToList();
            return View("_DetailProduct", list);

        }
        public ActionResult orderDetail(int orderId)
        {
            var single = db.Orders.Find(orderId);
            return View("oderComplete", single);
        }

        public ActionResult subnameProduct(int id)
        {
            var list = db.Products.Find(id);
            return View("_SubNameProduct", list);

        }
        public ActionResult formCheckOut()
        {
            //Muser user = (Muser)Session[Common.CommonConstants.CUSTOMER_SESSION];
            return View("_formCheckout"/*, user*/);

        }



    }
}