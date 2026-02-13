using Redbud.BL.DL;
using System;
using System.Linq;
using System.Web;

namespace Maddux.Catch.LocalClasses
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
            CurrentUser = new User();

            LastSearchType = "";
            LastSearchString = "";
            LastAssociationRegionFilter = "";
            LastAssociationStarRatingFilter = "";
        }

        public static AppSession Current
        {
            get
            {
                AppSession session = (AppSession)HttpContext.Current.Session["__Maddux.CatchSession__"];
                if (session == null)
                {
                    session = new AppSession();
                    HttpContext.Current.Session["__Maddux.CatchSession__"] = session;
                }
                return session;

            }
        }

        public void Clear()
        {
            InitializeSession();
        }
        private User _CurrentUser;
        public User CurrentUser
        {
            get
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    User account = null;

                    System.Web.HttpContext context = System.Web.HttpContext.Current;

                    if (context != null && context.User.Identity.IsAuthenticated)
                    {
                        string email = System.Web.HttpContext.Current.User.Identity.Name.TrimEnd();
                        account = db.Users.FirstOrDefault(x => string.Equals(email, x.EmailAddress.TrimEnd()));
                    }

                    return account;
                }
            }
            set
            {
                _CurrentUser = value;
            }
        }

        public string LastSearchType { get; set; }
        public string LastSearchString { get; set; }

        public string LastAssociationRegionFilter { get; set; }

        public string LastAssociationStarRatingFilter { get; set; }
    }
}