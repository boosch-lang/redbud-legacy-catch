using Redbud.BL.DL;
using System;
using System.Linq;

namespace Password_Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new MadduxEntities())
            {
                var _customers = db.Customers.Where(x => x.WebPassword_Hash != null).ToList();
                Console.WriteLine(_customers.Count());
                //foreach (var customer in _customers)
                //{
                //    string password = customer.WebPassword;
                //    customer.WebPassword_Hash = FCSEncryption.Encrypt(password);
                //    Console.WriteLine(customer.WebPassword_Hash);
                //}
                //db.SaveChanges();
            }
        }
    }
}
