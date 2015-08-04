using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace FinallProject_
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        //method who gets user by  id
        [OperationContract]
        User GetUser(int id);
        [OperationContract]
        List<User> Users();
        //method who delete user
        [OperationContract]
        void Delete_User(int id);
        //method who edit user
        [OperationContract]
        void Edit_User(int id, User user);
        //method who gets  Category by  
        [OperationContract]
        Category GetCategory(int id);
        //method who gets all Category by  
        [OperationContract]
        List<Category> GetCategories();
        //method who deleteCategory
        [OperationContract]
        void Delete_Category(int id);
        //method who edit Category
        [OperationContract]
        void Edit_Category(int id, string name);

        //method who gets  SubCategory by  id
        [OperationContract]
        Category SubCategory(int id);
        //method who gets all  SubCategory by  id
        [OperationContract]
        List<Category> SubCategories();
        //method who delete SubCategory
        [OperationContract]
        void Delete_SubCategory(int id);
        //method who edit SubCategory
        [OperationContract]
        void Edit_SubCategory(int id, string name);

        //method who gets product by  id
        [OperationContract]
        Product GetProduct(int id);
        //method who gets all product by  id
        [OperationContract]
        List<Product> GetProducts();
        //method who delete Product
        [OperationContract]
        void Delete_Product(int id);
        //method who edit Product
        [OperationContract]
        void Edit_Product(int id, Product pr);

        //method who gets  orderProduct by  id
        [OperationContract]
        OrderProduct GetOrderProduct(int id);
        //method who gets all orderProduct by  id
        [OperationContract]
        List<OrderProduct> GetOrderProducts();
        //method who delete OrderProduct
        [OperationContract]
        void Delete_OrderProduct(int id);
        //method who edit OrderProduct
        [OperationContract]
        void Edit_OrderProduct(int id, OrderProduct oP);

        //method who gets  order by  id
        [OperationContract]
        Order GetOrder(int id);
        //method who gets ALL order by  id
        [OperationContract]
        List<Order> GetOrders();
        //method who delete Order
        [OperationContract]
        void Delete_Order(int id);
        //method who edit Order
        [OperationContract]
        void Edit_Order(int id, Order o);
        //method who gets  image by  id
        [OperationContract]
        Images GetImage(int id);
        //method who gets all  image by  id
        [OperationContract]
        List<Images> GetImages();
        //method who delete Image
        [OperationContract]
        void Delete_Image(int id);
        //method who edit Image 
        [OperationContract]
        void Edit_Image(int id, Images img);


        //method who gets  GetShipping_Company by  id
        [OperationContract]
        Shipping_Company GetShipping_Company(int id);
        //method who gets all GetShipping_Company by  id
        [OperationContract]
        List<Shipping_Company> GetShipping_Companys();
        //method who delete Shipping_Company
        [OperationContract]
        void Delete_Shipping_Company(int id);
        //method who edit Shipping_Company
        [OperationContract]
        void Edit_Shipping_Company(int id, Shipping_Company ship);
        [OperationContract]
        void AddProduct(Product product);
        [OperationContract]
        User LoginUser(User user);
        [OperationContract]
        void dbDispose();
        [OperationContract]
        void AddUser(User user);
        [OperationContract]
        void AddImage(Images ima);
        [OperationContract]
        void AddOrderProduct(OrderProduct orderProduct);
        [OperationContract]
        void AddOrder(Order order);
        [OperationContract]
        User GetFacebookUser(string authId);
        [OperationContract]
        List<Inbox> GetInbox(User u);
        [OperationContract]
        List<Inbox> GetOutbox(User u);  
        [OperationContract]
        void AddMessage(Inbox inbox);
        [OperationContract]
        void AddInbox(Inbox inbox);
    }
    



}
