using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.customer
{
    public partial class new_association : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var user = AppSession.Current.CurrentUser;
            if (!user.CanDeleteCustomers || !user.CanDeleteOrders)
            {
                Response.Redirect("/customer/associations.aspx", true);
            }
            //association.CustomerAsscs
            Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
            litPageHeader.Text = $@"New Association";
        }



        protected void Save_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                try
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        string classText = ddlClass.SelectedValue;
                        int calculated = int.Parse(ddlCalculated.SelectedValue);
                        Association association = db.Associations.Create();

                        association.Active = true;
                        association.Calculated = Convert.ToBoolean(calculated);
                        association.Class = classText;
                        association.AsscDesc = $"{classText}-{txtDesc.Text}";
                        association.AsscShort = $"{classText}-{txtDesc.Text}";
                        association.BannerMessage = BannerMessage.Text;
                        association.TagLine = TagLine.Text;

                        db.Associations.Add(association);

                        int changes = db.SaveChanges();
                        if (changes > 0)
                        {
                            Response.Redirect($"/customer/associationdetail.aspx?aID={association.AssociationID}", true);
                        }
                    }

                }
                catch
                {
                    litMessage.Text = StringTools.GenerateError("Error while creating new association!");
                }
            }
        }
    }
}