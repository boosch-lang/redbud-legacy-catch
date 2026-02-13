using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using System;
using System.Linq;

namespace Maddux.Catch.customer
{
    public partial class contactdetail : System.Web.UI.Page
    {
        private int ContactID
        {
            get
            {
                if (ViewState["ContactID"] == null)
                {
                    ViewState["ContactID"] = Request.QueryString["ContactID"] == null || Request.QueryString["ContactID"] == "" ? 0 : (object)Request.QueryString["ContactID"];
                }
                return Convert.ToInt32(ViewState["ContactID"].ToString());
            }

            set
            {
                ViewState["ContactID"] = value;
            }
        }

        private int CustomerID
        {
            get
            {
                if (ViewState["CustomerId"] == null)
                {
                    ViewState["CustomerId"] = Request.QueryString["CustomerId"] == null || Request.QueryString["CustomerId"] == "" ? 0 : (object)Request.QueryString["CustomerId"];
                }
                return Convert.ToInt32(ViewState["CustomerId"].ToString());
            }

            set
            {
                ViewState["CustomerId"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var contact = new Contact();
                using (var db = new MadduxEntities())
                {
                    if (ContactID == 0)
                    {
                        contact.CustomerID = CustomerID;
                        delete.Visible = false;
                    }
                    else
                    {
                        contact = db.Contacts.FirstOrDefault(r => r.ContactID == ContactID);

                        txtContactFirstName.Text = contact.FirstName;
                        txtContactLastName.Text = contact.LastName;
                        txtEmail.Text = contact.EMail;
                        txtExtension.Text = contact.Extension;
                        txtFax.Text = contact.Fax;
                        txtMobile.Text = contact.CellPhone;
                        txtNotes.Text = contact.Notes;
                        txtPager.Text = contact.Pager;
                        txtPhone.Text = contact.Phone;
                        txtPosition.Text = contact.Position;
                        delete.Visible = true;
                    }
                }
            }
        }

        protected void saveAndClose_Click(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var contact = new Contact();
                if (ContactID == 0)
                {
                    db.Contacts.Add(contact);
                    contact.CustomerID = CustomerID;

                }
                else
                {
                    contact = db.Contacts.FirstOrDefault(r => r.ContactID == ContactID);
                }

                contact.FirstName = txtContactFirstName.Text;
                contact.LastName = txtContactLastName.Text;
                contact.Position = txtPosition.Text;
                contact.Phone = txtPhone.Text;
                contact.Extension = txtExtension.Text;
                contact.CellPhone = txtMobile.Text;
                contact.Fax = txtFax.Text;
                contact.Pager = txtPager.Text;
                contact.EMail = txtEmail.Text;
                contact.Notes = txtNotes.Text;

                db.SaveChanges();
                ContactID = contact.ContactID;
                CloseWindow(true);

            }
        }

        private void CloseWindow(Boolean RefreshParent)
        {
            Utils util = new Utils();

            if (RefreshParent)
            {
                util.RegisterStartupScriptBlock("CloseWindow", "window.parent.location.reload();", Page);
            }
            else
            {
                util.RegisterStartupScriptBlock("CloseWindow", "Close();", Page);
            }
        }

        protected void delete_Click(object sender, EventArgs e)
        {
            using (var db = new MadduxEntities())
            {
                var contact = db.Contacts.FirstOrDefault(r => r.ContactID == ContactID);
                db.Contacts.Remove(contact);
                db.SaveChanges();
                CloseWindow(true);
            }
        }
    }
}