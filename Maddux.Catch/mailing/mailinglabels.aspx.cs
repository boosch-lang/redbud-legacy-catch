using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.mailing
{
    public partial class mailinglabels : System.Web.UI.Page
    {
        private User currentUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                currentUser = AppSession.Current.CurrentUser;

                if (!Page.IsPostBack)
                {

                    Title = "Maddux.Catch | Mailing Labels";
                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                    litPageHeader.Text = "Mailing Assistant - CSV File for Mail Merge";

                    txtFollowupDate.Text = DateTime.Today.AddDays(14).ToString("yyyy-MM-dd");
                    txtJournalNotes.Text = string.Format("{0} - {1} - Sent mailer.", currentUser.FullName, DateTime.Now.ToString());

                    LoadAssociations();
                    LoadProvinces();

                }
            }
            catch
            {
            }
        }

        private void LoadAssociations()
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    lbAssociations.DataSource = db.Associations.Where(a => a.Active).OrderBy(a => a.AsscDesc).Select(a => new ListItem
                    {
                        Text = a.AsscDesc,
                        Value = a.AssociationID.ToString()
                    }).ToList();
                    lbAssociations.DataBind();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        private void LoadProvinces()
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    lbProvinces.DataSource = db.States.Where(p => string.Equals(p.Country, "Canada")).Select(s => new ListItem
                    {
                        Text = s.StateName,
                        Value = s.StateID
                    }).ToList();
                    lbProvinces.DataBind();
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }


        protected void btnSelectAllAsscs_Click(object sender, EventArgs e)
        {
            foreach (ListItem li in lbAssociations.Items)
            {
                li.Selected = true;
            }
        }

        protected void btnSelectNoAssc_Click(object sender, EventArgs e)
        {
            foreach (ListItem li in lbAssociations.Items)
            {
                li.Selected = false;
            }
        }

        protected void btnSelectAllProv_Click(object sender, EventArgs e)
        {
            foreach (ListItem li in lbProvinces.Items)
            {
                li.Selected = true;
            }
        }

        protected void btnSelectNoProv_Click(object sender, EventArgs e)
        {

            foreach (ListItem li in lbProvinces.Items)
            {
                li.Selected = false;
            }
        }

        protected void chkCreateJournals_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCreateJournals.Checked)
            {
                lblFollowupDate.Enabled = true;
                lblFollowupDetails.Enabled = true;
                txtFollowupDate.Enabled = true;
                txtJournalNotes.Enabled = true;
            }
            else
            {
                lblFollowupDate.Enabled = false;
                lblFollowupDetails.Enabled = false;
                txtFollowupDate.Enabled = false;
                txtJournalNotes.Enabled = false;
            }

        }

        protected void cmdCreateCSV_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> _sAssocIncludeIds = new List<int>();
                List<string> _sProvinces = new List<string>();

                foreach (ListItem _li in lbAssociations.Items)
                {
                    if (_li.Selected)
                    {
                        _sAssocIncludeIds.Add(int.Parse(_li.Value));
                    }
                }

                foreach (ListItem _liProv in lbProvinces.Items)
                {
                    if (_liProv.Selected)
                    {
                        _sProvinces.Add(_liProv.Value);
                    }
                }
                using (MadduxEntities db = new MadduxEntities())
                {
                    List<CustomerAssc> customerAsscs = db.CustomerAsscs.Include("Customer").Where(c => _sAssocIncludeIds.Contains(c.AssociationID) && c.Association.Active).ToList();
                    List<int> customerIDS = new List<int>();
                    List<Customer> customer = customerAsscs.Select(x => x.Customer).Distinct().ToList();

                    if (_sProvinces.Count > 0)
                    {
                        customer = customer.Where(c => _sProvinces.Contains(c.State)).ToList();
                    }

                    var customers = customer
                        .OrderBy(x => x.Company)
                        .ThenBy(x => x.Email)
                        .Where(c=>c.Active)
                        .Select(r => new
                        {
                            r.CustomerId,
                            r.Company,
                            r.Address,
                            r.City,
                            r.State,
                            r.Zip,
                            r.Country,
                            r.Phone,
                            r.Email,
                            r.AlternateEmail,
                            r.WebSite,
                            r.Fax
                        }).ToList();

                    StringBuilder sbOutput = new StringBuilder();
                    bool headerRow = true;
                    foreach (var cust in customers)
                    {
                        if (cust != null)
                        {
                            string _sSeparator = "";
                            if (headerRow)
                            {
                                foreach (var prop in cust.GetType().GetProperties())
                                {
                                    sbOutput.Append(_sSeparator).Append(prop.Name);
                                    _sSeparator = ",";
                                }
                                sbOutput.Append("\n");
                                headerRow = false;
                            }

                            _sSeparator = "";
                            var list = cust.GetType().GetProperties();
                            foreach (var prop in cust.GetType().GetProperties())
                            {
                                string val = string.Empty;
                                switch (prop.Name.ToLower())
                                {
                                    case "customerid":
                                        val = prop.GetValue(cust, null).ToString();
                                        sbOutput.Append(_sSeparator).Append(val.Replace(",", " ").Replace("\n", " ").Replace("\r", " "));
                                        break;
                                    case "company":
                                    case "address":
                                    case "city":
                                    case "state":
                                    case "zip":
                                    case "country":
                                    case "phone":
                                    case "email":
                                    case "alternateemail":
                                    case "website":
                                        if (prop.GetValue(cust, null) != null)
                                        {
                                            val = prop.GetValue(cust, null).ToString();
                                            sbOutput.Append(_sSeparator).Append(val.Replace(",", " ").Replace("\n", " ").Replace("\r", " "));
                                        }
                                        else
                                        {
                                            sbOutput.Append(_sSeparator);
                                        }
                                        break;
                                    case "fax":
                                        if ((string)prop.GetValue(cust, null) != null)
                                        {
                                            val = (string)prop.GetValue(cust, null);
                                            val = val.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
                                        }

                                        if (val.Length > 0)
                                        {
                                            if (val.Substring(0, 1) != "1")
                                            {
                                                val = "1" + val;
                                            }
                                        }
                                        sbOutput.Append(_sSeparator).Append(val.Replace(",", " ").Replace("\n", " ").Replace("\r", " "));
                                        break;
                                    default:
                                        break;
                                }

                                _sSeparator = ",";
                            }

                            sbOutput.Append("\n");
                            string _email = currentUser.EmailAddress.Trim();

                            if (chkCreateJournals.Checked)
                            {
                                Redbud.BL.DL.Journal journal = new Redbud.BL.DL.Journal
                                {
                                    AssignedToId = 0,
                                    IsResolved = false,
                                    CreateDate = DateTime.Today,
                                    WhoInserted = _email,
                                    DateUpdated = DateTime.Today,
                                    WhoUpdated = _email,
                                    CustomerID = cust.CustomerId,
                                    DateStamp = DateTime.Now,
                                    FollowUpDate = Convert.ToDateTime(txtFollowupDate.Text),
                                    Notes = txtJournalNotes.Text.Trim()
                                };
                                db.Journals.Add(journal);
                            }
                        }
                    }
                    db.SaveChanges();

                    byte[] bytes = Encoding.ASCII.GetBytes(sbOutput.ToString());

                    if (bytes != null)
                    {
                        Response.Clear();
                        Response.ContentType = "text/csv";
                        Response.AddHeader("Content-Length", bytes.Length.ToString());
                        Response.AddHeader("Content-disposition", "attachment; filename=RedbudCustomers.csv");
                        Response.BinaryWrite(bytes);
                        Response.Flush();
                        Response.OutputStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                var test = ex.InnerException;
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
    }
}