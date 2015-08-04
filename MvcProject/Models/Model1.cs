using MvcProject.BuyNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProject.Models
{
    public class Model1
    { public static Service1Client client = new Service1Client();

        internal static void PayOrde(string card, string cardNumber, string cvc, string firstName, string lastName, string Id, string street, string city, string post_code, OrderProduct or)
        {
            if (card == string.Empty || firstName == string.Empty || lastName == string.Empty || street == string.Empty || city == string.Empty)
            {
                throw new ApplicationException("One or more of the deatails is missing please try again");
            }
            if (cardNumber.Length < 9) throw new ApplicationException("Card number not valid please try again");
            if (cvc.Length != 3) throw new ApplicationException("Card securty code (cvc) not valid please try again");
            if (Id.Length < 9) throw new ApplicationException("Id number not valid please try again");
            if (post_code.Length < 4) throw new ApplicationException("Post code not valid please try again");

            Order ordery = client.GetOrder(or.Order.Id); 
            //closing order by date time feled
            ordery.DateOfPayment = DateTime.Now;

        }
    }
}