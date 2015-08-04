using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Data.Entity.SqlServer;
using System.ServiceModel;
using System.Text;
using System.Data.Entity;

namespace FinallProject_
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        public Service1()
        {
            db.Configuration.ProxyCreationEnabled = false;
        }
        FinallProject_Entities db = new FinallProject_Entities();
        //method who getting a user by his id
        public User GetUser(int id)
        {

            return db.User.Include("Images").Include("Order").Include("Product").FirstOrDefault(u => u.Id == id);
        }
        //method who gets  Category by  id
        public Category GetCategory(int id)
        {
            //return DatabaseContext.Applications
            //.Include(a => a.Children.Select(c => c.ChildRelationshipType));

            Category t = db.Category.Include("SubCategoryCategory").Include("Product").Include("ParentCategory").FirstOrDefault(c => c.Parent == null && c.Id == id);
            return t;

        }
        //method who gets  SubCategory by  id
        public Category SubCategory(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.Category.Include("ParentCategory").Include("Product").FirstOrDefault(sc => sc.Parent != null && sc.Id == id);
        }
        //method who gets product by  id
        public Product GetProduct(int id)
        {
            return db.Product.Include(p => p.User.Images).Include(p => p.Category.ParentCategory).Include(o => o.Images).FirstOrDefault(p => p.Id == id);
        }
        public void AddProduct(Product product)
        {
            try
            {
                db.Product.Add(product);
            }
            catch (Exception ex)
            {
                Console.Write(ex.InnerException);
                Console.Write(ex.Message);
            }
        }

        //method who gets orderProduct by id
        public OrderProduct GetOrderProduct(int id)
        {

            return db.OrderProduct.Include(op => op.Order.User).Include(op => op.Product.User).FirstOrDefault(op => op.Id == id);
        }
        //method who gets  order by  id
        public Order GetOrder(int id)
        {

            return db.Order.Include("User").Include("Shipping_Company").FirstOrDefault(o => o.Id == id);
        }
        //method who gets  image by  id
        public Images GetImage(int id)
        {


            Images t = db.Images.Include("Product").Include("User").FirstOrDefault(d => d.Id == id);
            return t;
        }
        //method who gets  image by  id
        public Shipping_Company GetShipping_Company(int id)
        {

            return db.Shipping_Company.Include("Order").FirstOrDefault(s => s.Id == id);
        }


        public List<Category> GetCategories()
        {

            return db.Category.Include("SubCategoryCategory").Include("ParentCategory").Include("Product").Where(c => c.Parent == null).ToList();
        }

        public List<Category> SubCategories()
        {
            return db.Category.Include("SubCategoryCategory").Include("ParentCategory").Include("Product").Where(c => c.Parent != null).ToList();
        }

        public List<Product> GetProducts()
        {

            return db.Product.Include(p => p.Category.ParentCategory.ParentCategory).Include("User").Include("Images").ToList();
        }

        public List<OrderProduct> GetOrderProducts()
        {
            var aa = db.OrderProduct.Include(op => op.Order.User).Include(op => op.Product.User).Include(op => op.Product.Images).ToList();
            return aa;
        }

        public List<Order> GetOrders()
        {
            return db.Order.Include("User").ToList();
        }

        public List<Images> GetImages()
        {
            return db.Images.Include("Product").Include("User").ToList();
        }

        public List<Shipping_Company> GetShipping_Companys()
        {
            return db.Shipping_Company.Include("Order").ToList();
        }

        public void Delete_User(int id)
        {
            User user = db.User.FirstOrDefault(u => u.Id == id);
            db.User.Remove(user);
            db.SaveChanges();

        }

        public void Edit_User(int id, User editUser)
        {
            //User user = db.User.FirstOrDefault(u => u.Id == id);
            // user = editUser;
            //db.SaveChanges();
            db.Entry(editUser).State = EntityState.Modified;
            db.SaveChanges();

        }

        public void Delete_Category(int id)
        {
            Category cat = db.Category.FirstOrDefault(i => i.Parent == null && i.Id == id);
            db.Category.Remove(cat);
            db.SaveChanges();
        }

        public void Edit_Category(int id, string name)
        {
            db.Category.FirstOrDefault(i => i.Parent == null && i.Id == id).Name = name;

            db.SaveChanges();

        }



        public void Delete_SubCategory(int id)
        {
            Category subCat = db.Category.FirstOrDefault(s => s.Parent != null && s.Id == id);
            db.Category.Remove(subCat);
            db.SaveChanges();
        }

        public void Edit_SubCategory(int id, string name)
        {
            db.Category.FirstOrDefault(s => s.Parent != null && s.Id == id).Name = name;
            db.SaveChanges();

        }

        public void Delete_Product(int id)
        {
            db.Product.Remove(db.Product.FirstOrDefault(p => p.Id == id));
            db.SaveChanges();
        }

        public void Edit_Product(int id, Product editProduct)
        {
            Product product = db.Product.FirstOrDefault(s => s.Id == id);
            product = editProduct;
            db.SaveChanges();
        }

        public void Delete_OrderProduct(int id)
        {
            db.OrderProduct.Remove(db.OrderProduct.FirstOrDefault(p => p.Id == id));
            db.SaveChanges();
        }

        public void Edit_OrderProduct(int id, OrderProduct editOd)
        {
            OrderProduct od = db.OrderProduct.FirstOrDefault(p => p.Id == id);
            od = editOd;
            db.SaveChanges();
        }

        public void Delete_Order(int id)
        {
            db.Order.Remove(db.Order.FirstOrDefault(p => p.Id == id));
            db.SaveChanges();
        }

        public void Edit_Order(int id, Order editO)
        {
            Order o = db.Order.FirstOrDefault(p => p.Id == id);
            o.DateOfPayment = editO.DateOfPayment.Value;
            o.Single = editO.Single;
            db.Entry(o).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete_Image(int id)
        {
            db.Images.Remove(db.Images.FirstOrDefault(p => p.Id == id));
            db.SaveChanges();
        }

        public void Edit_Image(int id, Images image)
        {
            Images i = db.Images.FirstOrDefault(p => p.Id == id);
            i = image;
            db.SaveChanges();
        }

        public void Delete_Shipping_Company(int id)
        {
            db.Shipping_Company.Remove(db.Shipping_Company.FirstOrDefault(p => p.Id == id));
            db.SaveChanges();
        }

        public void Edit_Shipping_Company(int id, Shipping_Company editShipp)
        {
            Shipping_Company ship = db.Shipping_Company.FirstOrDefault(p => p.Id == id);
            ship = editShipp;
            db.SaveChanges();
        }

        public List<User> Users()
        {
            return db.User.Include(u => u.Images).Include(u => u.Order).Include(u => u.Product).ToList();
        }
        public User LoginUser(User user)
        {
            return db.User.Include(u => u.Images).Include(u => u.Order).Include(u => u.Product).Where(a => a.UserName.Equals(user.UserName) && a.Password.Equals(user.Password)).FirstOrDefault();
        }
        public void dbDispose()
        {
            db.Dispose();
        }

        public void AddUser(User user)
        {
            db.User.Add(user);
            db.SaveChanges();

        }

        public void AddImage(Images ima)
        {
            db.Images.Add(ima);
            db.SaveChanges();


        }
        public void AddOrderProduct(OrderProduct orderProduct)
        {
            OrderProduct op = new OrderProduct();
            op.OrderId = orderProduct.Order.Id;
            op.ProductId = orderProduct.Product.Id;
            op.Quantity = orderProduct.Quantity;
            op.PriceAtPayment = orderProduct.PriceAtPayment;
            try
            {
                db.OrderProduct.Add(op);
                db.SaveChanges();

            }
            catch (Exception ex)
            {

                Console.Write(ex.InnerException);
                Console.Write(ex.Message);
            }
        }
        public void AddOrder(Order order)
        {
            Order d = new Order();
            d.shippingId = order.Shipping_Company.Id;
            d.DateOfPayment = order.DateOfPayment;
            d.BuyerId = order.User.Id;
            d.Single = order.Single;
            try
            {
                db.Order.Add(d);
                db.SaveChanges();
            }
            catch { }

        }
        public User GetFacebookUser(string authId)
        {
            return db.User.FirstOrDefault(u => u.AuthID == authId);
        }
        public void AddMessage(Inbox inbox)
        {

            db.Inbox.Add(inbox);
            db.SaveChanges();
        }

        public List<Inbox> GetInbox(User u)
        {
            return db.Inbox.Where(i => i.Sender == u.Id).ToList();
        }
        public void AddInbox(Inbox inbox)
        {
           db.Inbox.Add(inbox);
        }
        public List<Inbox> GetOutbox(User u)
        {
            return db.Inbox.Where(i => i.Receiver == u.Id).ToList();
        }
    }
}
