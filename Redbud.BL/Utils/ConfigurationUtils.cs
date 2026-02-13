using Redbud.BL.DL;
using System.Linq;

namespace Redbud.BL.Utils
{
    public static class ConfigurationUtils
    {
        public static Configuration GetConfiguration()
        {
            using (var db = new MadduxEntities())
            {

                //localhost config for tesing

                //_localConfig.NewsletterSMTPServer = "localhost";
                //_localConfig.NewsletterSMTPPort = 25;
                //_localConfig.NewsletterSMTPLogin = string.Empty;
                //_localConfig.NewsletterSMTPPwd = string.Empty;

                //_localConfig.EmailSMTPServer = "localhost";
                //_localConfig.EmailSMTPPort = 25;
                //_localConfig.EmailSMTPLogin = null;
                //_localConfig.EmailSMTPPwd = null;
                return db.Configurations.FirstOrDefault();
            }
        }
    }
}
