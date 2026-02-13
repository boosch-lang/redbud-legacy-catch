using Redbud.BL.DL;
using System;
using System.Linq;
using System.Web;

namespace Maddux.Pitch.LocalClasses
{
    [Serializable]
    public partial class AppSession
    {
        private AppSession()
        {
            InitializeSession();
        }

        private void InitializeSession()
        {
            CurrentCustomer = new Customer();
        }

        public static AppSession Current
        {
            get
            {
                AppSession session = null;
                if (HttpContext.Current.Session != null)
                {
                    session = (AppSession)HttpContext.Current.Session["__Maddux.PitchSession__"];
                    if (session == null)
                    {
                        session = new AppSession();
                        HttpContext.Current.Session["__Maddux.PitchSession__"] = session;
                    }
                }

                return session;
            }
        }

        public void Clear()
        {
            InitializeSession();
        }
        private Customer _CurrentCustomer;
        public Customer CurrentCustomer
        {
            get
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    Customer account = null;

                    System.Web.HttpContext context = System.Web.HttpContext.Current;

                    if (context != null && context.User.Identity.IsAuthenticated)
                    {
                        string email = System.Web.HttpContext.Current.User.Identity.Name.TrimEnd();
                        account = db.Customers.FirstOrDefault(x => string.Equals(email, x.Email.TrimEnd()));
                    }

                    return account;
                }

            }
            set
            {
                _CurrentCustomer = value;
            }
        }
    }
}