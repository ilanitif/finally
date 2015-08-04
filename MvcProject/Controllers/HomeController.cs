using MvcProject.BuyNet;
using MvcProject.Infrastructure.FaceBook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;
using System.Net;

namespace MvcProject.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public static Service1Client db = new Service1Client();
        public static Service1Client client = new Service1Client();

        public ActionResult make_an_offer(int price)
        {

            return View("Create");
        }


        //userLogin
        public ActionResult Login()
        {
             return View();
        }

        //Login post
        [HttpPost]
        public ActionResult Login(string email, string password, string rember)
        {
            if (email == string.Empty || password == string.Empty)
            {
                ViewBag.message = "u must enter all field";
                return View();
            }
            BuyNet.User emailU = null;
            try
            {
                emailU = client.Users().FirstOrDefault(u => u.Email == email);
            }
            catch { }

            if (emailU == null || emailU.Password != password)
            {
                ViewBag.message = "Password or Email not valid. Try Again";
                return View();
            }

            Session["User"] = emailU;
           
            string url = (string)Session["url"];
            if(url== "YourDeatails")
            {
             int i=  (int) Session["id"];
             
                return RedirectToAction("YourDeatails", new { productId = i });
            }


            return RedirectToAction(url);
        }




        [HttpGet]
        public ActionResult Facebook(string code, string error)
        {

            // Local Test Yizhak 
            OAuth fb_oauth = new OAuth("1626739400941856", "0876eb435d94500377ea20d28ce8c5e4");


            if (string.IsNullOrEmpty(error))
            {

                if (string.IsNullOrEmpty(code))
                {
                    Response.Redirect(fb_oauth.UrlRedirect(AbsolutePathForFB("~/Home/Facebook")));
                }
                else
                {
                    var user = fb_oauth.UserData(code, AbsolutePathForFB("~/Home/Facebook"));
                    if (user != null)
                    {
                        // todo: enter to DB
                        var FBUser = client.GetFacebookUser(user.id);
                        if (FBUser != null)
                        {
                            //fb_oauth.GetUserFrinds();

                            Session["User"] = FBUser;
                           HttpCookie Cookief = new HttpCookie("NameOfUser", FBUser.UserName);
                            Response.Cookies.Add(Cookief);
                        }
                        else
                        {
                            try
                            {
                                HttpCookie Cookie = new HttpCookie("NameOfUser", user.name);
                                Response.Cookies.Add(Cookie);
                                string two = "2";

                              
                                var fullName = user.name.Split('|');

        
                                var e = new BuyNet.User()
                                {
                                    //לא לשכוח להפריד את השם משפחה ואת השם משתמש עם פונקצית split 
                                    Password = "1",
                                    AuthID = user.id,
                                    AuthType = two,
                                    Email = user.name + "FaceBook.com",
                                    UserName = user.name,
                                    First_Name = fullName[0],
                                    Last_Name = fullName[1],
                                    ProfilePic = user.picture,
                                    Notes = user.link
                                };
                                client.AddUser(e);

                                Session["User"] = e;
                                return RedirectToAction("Index", "Home");
                            }
                            catch (Exception)
                            {

                                throw;
                            }

                        }

                       string url=(string)Session["url"];
                        return RedirectToAction(url);
                    }
                }
            }

            return View();
        }
        public string AbsolutePathForFB(string path)
        {

            Uri uri = new Uri(path, UriKind.RelativeOrAbsolute);
            if (!uri.IsAbsoluteUri)
            {
                Uri requestUrl = Url.RequestContext.HttpContext.Request.Url;
                UriBuilder builder = new UriBuilder(requestUrl.Scheme, requestUrl.Host, requestUrl.Port);
                builder.Path = VirtualPathUtility.ToAbsolute(path);
                uri = builder.Uri;

            }
            return uri.ToString();
        }

        private void CreateAuthCookieFor(string AuthId, bool RememberMe)
        {
            var authTicket = new FormsAuthenticationTicket(
                        1,                             // version
                        AuthId,                        // user name
                        DateTime.Now,                  // created
                        DateTime.Now.AddMinutes(60),   // expires
                        RememberMe,        // persistent?
                         "Facebook"   // can be used to store roles
                        );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            //HttpContext.Current.Response.Cookies.Add(authCookie);
            Response.Cookies.Add(authCookie);
        }


        //user LogOff
        public ActionResult Logoff()
        {
            Session["User"] = null;
            return RedirectToAction("Index");
        }

        //serach bar action for ajax send 
        public ActionResult SearchActionMethod(string word)
        {
            Dictionary<string[], int> words = new Dictionary<string[], int>();
            foreach (var item in client.GetProducts())
            {
                if (item.Name.ToUpper().Contains(word.ToUpper()) && word != String.Empty)
                {
                    words.Add(new string[] { "pro", item.Name }, item.Id);

                }
            }
            foreach (var item in client.GetShipping_Companys())
            {
                if (item.Company_Name.ToUpper().Contains(word.ToUpper()) && word != String.Empty)
                {
                    words.Add(new string[] { "com", item.Company_Name }, item.Id);
                }
            }
            foreach (var item in client.SubCategories())
            {
                if (item.Name.ToUpper().Contains(word.ToUpper()) && word != String.Empty)
                {
                    words.Add(new string[] { "sub", item.Name }, item.Id);
                }
            }
            //foreach (var item in client.GetProducts())
            //{
            //    if (item.Name.ToUpper().Contains(word.ToUpper()) && word != String.Empty)
            //    {
            //        words.Add("",item.Name);
            //    }
            //}
            foreach (var item in client.GetCategories())
            {
                if (item.Name.ToUpper().Contains(word.ToUpper()) && word != String.Empty)
                {
                    words.Add(new string[] { "cat", item.Name }, item.Id);
                }

            }
            //client.GetCategories();

            return Json(words.Take(7), JsonRequestBehavior.AllowGet);
        }


        //partialview of top3producs of each 
        public ActionResult Top3products()

        {
            Dictionary<Product, Category> all = new Dictionary<Product, Category>();
            foreach (var item in client.GetCategories())
            {
                List<Product> pro = new List<Product>();
                foreach (var p in client.GetProducts())
                {
                    if (p.Category == null)
                    {
                        pro.Add(p);
                    }

                    else if (p.Category.Parent == null && p.Category.Id == item.Id)
                    {
                        pro.Add(p);
                    }
                    else if (p.Category.ParentCategory.Id == item.Id)
                    {
                        pro.Add(p);
                    }
                    else if (p.Category.ParentCategory.ParentCategory != null && p.Category.ParentCategory.ParentCategory.Id == item.Id)
                    {
                        pro.Add(p);
                    }

                }
                var e = pro.OrderByDescending(pr => pr.Rating).Take(3);
                foreach (var i in e)
                {
                    all.Add(i, item);
                }

            }


            return PartialView(all);
        }


        //partial view of navigation bar
        public ActionResult NavigationBar()
        {


            return PartialView();
        }


        //home page
        public ActionResult Index()
        {
            return View();
        }


        //producs from the same product view
        public ActionResult Category(string category)
        {
            if (category == string.Empty)
            {
                RedirectToAction("index");
            }
            List<Category> categories = client.GetCategories().ToList();
            Category ca = categories.Where(c => c.Name.ToLower().Trim() == category.ToLower().Trim()).FirstOrDefault();

            return PartialView(ca);
        }


        //partial view of product
        public ActionResult ProductView(Product p)
        {
            return PartialView(p);
        }


        //subCategory from the same product view
        [HttpPost]
        public ActionResult SubCategory(string subcategory)
        {
            if (subcategory == string.Empty)
            {
                RedirectToAction("index");
            }
            List<Category> subcat = client.SubCategories().ToList();
            Category sub = subcat.FirstOrDefault(c => c.Name.Trim().ToLower() == subcategory.Trim().ToLower());
            Dictionary<Product, Category> prosub = client.GetProducts().Where(p => p.CategoryId == sub.Id).ToDictionary(p => p, p => p.Category);
            return PartialView(prosub);
        }


        //user private zone view

        public ActionResult UserP()
        {
            Session["url"] = "UserP";
            BuyNet.User user = (BuyNet.User)Session["User"];
            if (user == null)
            {
                return RedirectToAction("login");
            }

            List<Images> im = user.Images.ToList();
            return View(user);
        }


        //upload of userpice opload of his private area
        [HttpPost]
        public ActionResult UserPicUpload(HttpPostedFileBase imge)
        {

            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {

                    var fileName = Path.GetFileName(file.FileName);
                    Guid g = Guid.NewGuid();
                    var local_path = "/pics/" + g.ToString() + fileName;
                    var root_path = Path.Combine(Server.MapPath("~/pics/"), g.ToString() + fileName);
                    // 
                    // string directory = @"~/pics/" + g;
                    BuyNet.User user = (BuyNet.User)Session["User"];
                    Images ima = new Images()
                    {
                        img = local_path,
                        UserId = user.Id
                    };
                    client.AddImage(ima);
                    file.SaveAs(root_path);

                }



            }
            return RedirectToAction("UserP");
        }


        //product
        public ActionResult Product_Page(int id)
        {

            Product pr = client.GetProducts().SingleOrDefault(p => p.Id == id);

            ViewBag.Product = pr;
            return View();
        }



        //after user click buy now view of user deatails
        public ActionResult YourDeatails(int productId)
        {
            Session["url"] = "YourDeatails";
            Session["id"] = productId;
            BuyNet.User u = (BuyNet.User)Session["User"];
            if (u == null)
            {
              return  RedirectToAction("login");
            }
            Shipping_Company shipCompany = client.GetShipping_Company(2);


            Product product = client.GetProduct(productId);
            Session["product"] = product;
            ViewBag.ship = shipCompany;


            return View();
        }

        public ActionResult CheckOut()
        {
            BuyNet.User u =(BuyNet.User)Session["User"];
            List < Order >oU= client.GetOrders().Where(o => o.User.Id == u.Id).ToList();
            foreach (var item in oU)
            {
                if(item.DateOfPayment==null)
                {
                    item.DateOfPayment = DateTime.Now;
                    client.Edit_Order(item.Id, item);
                    Inbox inbox = new Inbox();
                    inbox.Sender = 11;
                    inbox.Receiver = u.Id;
                    inbox.Date = DateTime.Now;
                    inbox.Topic = "Your Order have been approve";
                    inbox.Content = u.UserName + ", your order details have been  succesfuly approve got your order details/n" +
                     "Order number: " + item.Id + "  " + item.DateOfPayment + " /n ";
                    int sum = 0;
                     foreach (var orp in item.OrderProduct)
                    {
                    inbox.Content+= orp.Product + " " + orp.Product.Price + " " + orp.Quantity +" /n";
                        sum += int.Parse(orp.Product.Price.ToString()) * orp.Quantity;
                    }
                    inbox.Content += "/n Total :" +sum +"/n card number 345 have been aprove./n Thank u for buying in BuyNet";
                    client.AddInbox(inbox);
                    break;

                }
            }

            return RedirectToAction("UserP");
        }
        public ActionResult Inbox()
        {
            Session["url"] = "Inbox";
            BuyNet.User u =(BuyNet.User) Session["User"];
            if (u==null)
            {
                return RedirectToAction("login");
            }
           ViewBag.Inbox=   client.GetInbox(u);
            ViewBag.OutBox = client.GetOutbox(u);
           return View();

        }




        //post method of user deatiles form
        [HttpPost]
        public ActionResult YourDeatails(string notes, int quantity, string deliveryAdress, string country, string city, string card, string street, string streetNumber, int productId, int shippingId)
        {
            Session["url"] = "YourDeatails";
            BuyNet.User u = (BuyNet.User)Session["User"];
            if(u==null)
            {
                return RedirectToAction("Login");
            }
            if (country == string.Empty || country == null) { country = u.Country; }
            if (city == string.Empty || city == null) { city = u.City; }
            if (streetNumber == string.Empty || streetNumber == null) { streetNumber = u.Street_number; }
            Shipping_Company ship = client.GetShipping_Company(shippingId);
            ship.city = city;
            ship.country = country;
            ship.streetNumber = street+" "+streetNumber;
            Product product = client.GetProduct(productId);
            try
            {
                Order or = new Order();
                or.Shipping_Company = ship;
                or.User = u;
                or.DateOfPayment = null;
                or.Single = true;
                client.AddOrder(or);

                foreach (var item in client.GetOrders())
                {
                    if (item.BuyerId == or.User.Id && item.DateOfPayment == null && item.Single==true)
                    {
                        OrderProduct op = new OrderProduct()
                        {
                            Quantity = quantity,
                            Product = product,
                            Order = item,
                            OrderId = item.Id,
                            PriceAtPayment = int.Parse(product.Price.ToString()) * int.Parse(quantity.ToString())
                        };
                        op.OrderId = item.Id;
                        item.DateOfPayment = DateTime.Now;
                        client.Edit_Order(item.Id, item);
                        client.AddOrderProduct(op);
                        break;
                    }

                }

               
            }
            catch (Exception ex)
            {

                throw ex;
            }



            ViewBag.user = User;

            return RedirectToAction("uSERP");
        }



        //seller add new product method
        public ActionResult SellNewProduct()
        {
            Session["url"] = "SellNewProduct";
            BuyNet.User user = (BuyNet.User)Session["User"];
            if (user == null)
            {
                return RedirectToAction("login");
            }
            return View();
        }

        //seller add new product method post
        [HttpPost]
        public ActionResult SellNewProduct(string productName, int categoryId, int? subCategoryId, bool? itemCondiation, decimal price,
            string productDescription, HttpPostedFileBase[] files)
        {

            Product p = new Product()
            {
                CategoryId = subCategoryId,
                Condition = itemCondiation ?? true,
                Price = price,
                Description = productDescription,
                Category = client.SubCategory(subCategoryId ?? 1)
            };
            client.AddProduct(p);
            if (Request.Files.Count > 0)
            {

                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];

                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        Guid g = Guid.NewGuid();
                        var path = Path.Combine(Server.MapPath("~/pics/"), g.ToString() + fileName);

                        string directory = @"/pics/" + g.ToString() + fileName;

                        Images ima = new Images()
                        {
                            img = directory,
                            ProductId = client.GetProducts().FirstOrDefault(pr => pr == p).Id
                        };
                        client.AddImage(ima);
                        file.SaveAs(path);
                    }
                }

            }


            foreach (var photovar in Request.Files)
            {
                if (photovar is HttpPostedFileBase)
                {
                    HttpPostedFileBase photo = (HttpPostedFileBase)photovar;

                    if (photo != null && photo.ContentLength > 0)
                    {
                        Guid g = Guid.NewGuid();
                        string directory = @"~/pics/" + g;

                        Images ima = new Images()
                        {
                            img = directory,
                            ProductId = client.GetProducts().FirstOrDefault(pr => pr == p).Id
                        };


                        if (photo.ContentLength > 10240)
                        {
                            ModelState.AddModelError("photo", "The size of the file should not exceed 10 KB");
                            return View();
                        }

                        var supportedTypes = new[] { "jpg", "jpeg", "png" };

                        var fileExt = System.IO.Path.GetExtension(photo.FileName).Substring(1);

                        if (!supportedTypes.Contains(fileExt))
                        {
                            ModelState.AddModelError("photo", "Invalid type. Only the following types (jpg, jpeg, png) are supported.");
                            return View();
                        }

                        var fileName = Path.GetFileName(photo.FileName);
                        photo.SaveAs(Path.Combine(directory, fileName));
                    }

                }
            }

            return View();
        }

        public ActionResult AddToCart(int product)
        {/*, int qantity*/

            Session["url"] = "userP";
            BuyNet.User u = (BuyNet.User)Session["User"];
            if (u == null)
            {
                return RedirectToAction("login");
            }
            List<Order> userOrders = client.GetOrders().Where(o => o.User.UserName.Trim().ToLower() == u.UserName.Trim().ToLower()).ToList();
            Order order = userOrders.Where(o => o.DateOfPayment == null).FirstOrDefault();
            if (order == null)
            {
                order = new Order()
                {
                    User = u,
                    Shipping_Company = client.GetShipping_Company(2),
                    DateOfPayment = null,
                };
                client.AddOrder(order);
            }

            OrderProduct op = new OrderProduct();
            op.Order = order;
            op.Product = client.GetProduct(product);
            op.Quantity = 1;
            op.PriceAtPayment = int.Parse(op.Product.Price.ToString()) * int.Parse(op.Quantity.ToString());
            client.AddOrderProduct(op);


            return RedirectToAction("Userp");//or return view 
        }
        public ActionResult RemoveOneProduct(int orderProductId)
        {
            ushort RemoveOneProduct = 1;
            OrderProduct orderProduct = client.GetOrderProduct(orderProductId);
            if (orderProduct.Quantity > 0)
            {
                orderProduct.Quantity = (ushort)(orderProduct.Quantity - RemoveOneProduct);
            }


            return RedirectToAction("UserP");
        }

        public ActionResult RemoveOrderProduct(int orderProductId)
        {
            client.Delete_OrderProduct(orderProductId);

            return RedirectToAction("UserP");
        }
        public ActionResult Add(int orderProductId)
        {
            ushort AddOneProduct = 1;
            OrderProduct orderProduct = client.GetOrderProduct(orderProductId);

            orderProduct.Quantity = (ushort)(orderProduct.Quantity + AddOneProduct);



            return RedirectToAction("UserP");
        }




        //view of remove product-        to move to admincontroller
        public ActionResult Remove(int id)
        {
            Service1Client client = new Service1Client();
            Product pro = client.GetProduct(id);
            return View(pro);
        }
        //post action resulat of remove product-  to move to admincontroller
        [HttpPost]
        public ActionResult RemoveProduct(int id)
        {

            Service1Client client = new Service1Client();
            client.Delete_Product(id);
            return View("index", client.GetProducts());
        }

        //auctions
        public ActionResult FirstPumbicSale(string word)
        {
            Product product = client.GetProducts().FirstOrDefault();
            string[] details = new string[3];
            details[0] = product.Name;
            details[1] = product.Price.ToString();
            return Json(details, JsonRequestBehavior.AllowGet);
        }


        //contact admins page view
        public ActionResult ContactUs()
        {

            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(string email, string subject,string body)
        {
            MailMessage message = new MailMessage(email,"ashgar691@gmail.com");
            message.Subject = subject;
            message.Body = body;
            SmtpClient smt = new SmtpClient("smtp.gmail.com",587);

            smt.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "dmnzomzom@gmail.com",
                Password = "dmnzomzom12"
            };
         
            smt.EnableSsl = true;

          try
            {
                smt.Send(message);
            }
            catch
            {
                ViewBag.Message = "Message have been failed";
                ViewBag.Warning = "true";
            }
          

            ViewBag.Message = "Your Message have been sent";
            ViewBag.Warning = "false";

            return View();
        }

        //page about the user that made this site, Project X with our images
        [HttpPost]
        public ActionResult Aboutas()
        {
            //byte[] img

            ViewBag.Message = "Your application description page.";

            return View(client.GetProducts());
        }


        //need to delate add new product first virsion
        public ActionResult Add()
        {


            return View();
        }
        //need to delate add new product first virsion
        [HttpPost]
        public ActionResult AddProduct(bool Condition, string Description, string Name, decimal Price,
            string SubCategories_Categories_Name, string SubCategories, string image)
        {

            Service1Client client = new Service1Client();
            Guid g = Guid.NewGuid();
            Images ima = new Images()
            {
                User = (BuyNet.User)Session["User"],
                img = "~/pics/" + g,
                UserId = int.Parse((string)Session["LogUserId"])

            };
            client.AddImage(ima);
            Product product = new Product()
            {
                Condition = Condition,
                Description = Description,
                Name = Name,
                Price = Price,
            };
            client.AddProduct(product);

            return View("index", client.GetCategories());
        }

        //homecontroller about-to delete
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        //homecontroller Contact-to delete
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}