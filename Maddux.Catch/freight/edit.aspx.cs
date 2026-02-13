using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Maddux.Catch.Helpers;

namespace Maddux.Catch.freight
{
    public partial class edit : System.Web.UI.Page
    {
        private string AreaID
        {
            get
            {
                if (ViewState["AreaID"] == null)
                {
                    ViewState["AreaID"] = Request.QueryString["aID"] == null || Request.QueryString["aID"] == "" ? string.Empty : Request.QueryString["aID"];
                }
                return ViewState["AreaID"].ToString();
            }

            set
            {
                ViewState["AreaID"] = value;
            }
        }
        private void LoadDropdown()
        {
            //Provinces
            using (MadduxEntities db = new MadduxEntities())
            {
                ddlProvince.DataSource = db.States.Where(x => x.Country.TrimEnd() == "Canada").Select(x => new ListItem
                {
                    Text = x.StateName,
                    Value = x.StateID
                }).ToList();
                ddlProvince.DataBind();
            }

            //Regions
            ddlRegion.DataSource = RegionHelper.GetRegionsForSelectList(false);
            ddlRegion.DataBind();
        }
        private void LoadPage(FreightCharge charge)
        {
            string _ch = charge.Charge.ToString();
            AreaIDText.Text = charge.AreaID;
            PlaceName.Text = charge.PlaceName;
            ddlCharges.SelectedValue = charge.Charge.ToString();
            ddlProvince.SelectedValue = charge.Province.TrimEnd();
            ddlRegion.SelectedValue = charge.Region?.TrimEnd() ?? "";
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                    LoadDropdown();
                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");

                using (MadduxEntities db = new MadduxEntities())
                {
                    FreightCharge charge = db.FreightCharges.FirstOrDefault(x => x.AreaID == AreaID);

                    if (charge == null)
                    {
                        litPageHeader.Text = $@"Add new Freight Charges";
                        charge = db.FreightCharges.Create();
                    }
                    else
                    {
                        litPageHeader.Text = $@"Freight Charges <small>({charge.AreaID})</small>";
                        if (!Page.IsPostBack)
                            LoadPage(charge);
                    }

                }
            }
            catch (Exception ex)
            {

                litMessage.Text = StringTools.GenerateError(ex.Message);
            }


        }

        protected void Save_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate();
                if (Page.IsValid)
                {
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        bool IsNew = false;
                        string Province = ddlProvince.SelectedValue;
                        string Region = ddlRegion.SelectedValue;
                        FreightCharge charge = db.FreightCharges.FirstOrDefault(x => x.AreaID == AreaID);
                        if (charge == null)
                        {
                            IsNew = true;
                            charge = db.FreightCharges.Create();
                        }
                        charge.AreaID = AreaIDText.Text;
                        charge.PlaceName = PlaceName.Text;
                        charge.Province = Province;
                        charge.Region = Region;
                        charge.Charge = Convert.ToDecimal(ddlCharges.SelectedValue);
                        if (IsNew)
                            db.FreightCharges.Add(charge);
                        db.SaveChanges();

                        Response.Redirect("/freight/default.aspx", true);
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    FreightCharge charge = db.FreightCharges.FirstOrDefault(x => x.AreaID == AreaID);
                    if (charge != null)
                    {
                        db.FreightCharges.Remove(charge);
                        db.SaveChanges();
                        Response.Redirect("/freight/default.aspx", true);
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
    }
}