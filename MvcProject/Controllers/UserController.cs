using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcProject.BuyNet;
using System.Net;
using System.Web.Security;
using System.Net.Mail;

namespace MvcProject.Controllers
{
    public class UserController : Controller
    {
        //private UserContext db = new UserContext();
        public static Service1Client db = new Service1Client();

        // GET: /User/
        public ActionResult Index()
        {
            return View(db.Users());
        }

        // GET: /User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

             User user = db.GetUser((int)id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                return View (user);
            
           
            
        }

        // GET: /User/Login
      
        // GET: /User/Create
        public ActionResult Create()
        {
            return View();
        }
      
        //
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            ViewData["Logoff"] = true;

            return PartialView("Index");
        }
        //
      


        public ActionResult AfterLogin()
        {
            if (Session["LogUserId"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string first_name, string last_name, string user_name, string email, string password, string Country, string City, string Street_Number, string Zip_Code)
        {
            //var v = db.GetUser(user.Id);
            if (email == string.Empty || password == string.Empty || user_name == string.Empty || first_name == string.Empty || last_name == string.Empty || Country == string.Empty || string.IsNullOrEmpty(Street_Number) || string.IsNullOrEmpty(Zip_Code))
            {

                ViewBag.message = "YOU Must Enter All fields!!!";
                return View();
            }
            if (!isEmailValid(email))
            {
                ViewBag.message = "Email: " + email + " not valid !!";
                return View();
            }
            if (password.Length < 6)
            {
                ViewBag.message = "YOU Must Enter 6 laters";

            }
            var users = db.Users();
            var isExist = users.Any(x => x.Email == email);

            if (isExist)
            {
                ViewBag.message = "User Alredy Exist";
                return View();
            }

            User user1 = new User();
            user1.Email = email;
            user1.Password = password;
            user1.UserName = user_name;
            user1.First_Name = first_name;
            user1.Last_Name = last_name;
            user1.Country = Country;
            user1.City = City;

            user1.Street_number = Street_Number;
            user1.Zip_Code = Zip_Code;
            db.AddUser(user1);

            Session["User"] = user1;

            return RedirectToAction("Index", "Home");
        }

        private bool isEmailValid(string email)
        {
            try
            {
                MailAddress mail = new MailAddress(email);
                return true;
            }
            catch (FormatException e)
            {
                return false;
            }
        }



        // GET: /User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyNet.User user = db.GetUser((int)id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,City,Country,Email,First_Name,Last_Name,Notes,Password,Street_number,UserName,Zip_Code")] BuyNet.User user)
        {
            if (ModelState.IsValid)
            {
                db.Edit_User(user.Id,user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: /User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuyNet.User user = db.GetUser((int)id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Delete_User(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.dbDispose();
            }
            base.Dispose(disposing);
        }
    }
}
    
