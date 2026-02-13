using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.journal
{
    public partial class journaldetail : Page
    {
        private int JournalID
        {
            get
            {
                if (ViewState["JournalId"] == null)
                {
                    ViewState["JournalId"] = Request.QueryString["id"] == null || Request.QueryString["id"] == "" ? 0 : (object)Request.QueryString["id"];
                }

                return Convert.ToInt32(ViewState["JournalId"].ToString());
            }

            set { ViewState["JournalId"] = value; }
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

            set { ViewState["CustomerId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string user;
            Redbud.BL.DL.Journal journal = new Redbud.BL.DL.Journal();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            user = HttpContext.Current.User.Identity.Name.Trim();
            hdnUserId.Value = user;
            if (!IsPostBack)
            {
                try
                {
                    string contactName;
                    LoadCampaigns();
                    LoadSales(user);
                    using (MadduxEntities db = new MadduxEntities())
                    {
                        if (JournalID == 0)
                        {
                            delete.Visible = false;
                            journal.CustomerID = CustomerID;
                            journal.CreateDate = DateTime.Now;
                            NextFollowupDiv.Visible = true;
                            RepeatUntilDiv.Visible = false;
                            rdblstRepeat.SelectedValue = "none";
                            txtRepeatFromDate.Text = DateTime.Now.ToString("MMMM dd, yyyy");
                        }
                        else
                        {
                            journal = db.Journals.FirstOrDefault(r => r.JournalID == JournalID);
                            CustomerID = journal.CustomerID;
                            delete.Visible = true;
                            RepeatEveryDiv.Visible = false;
                            RepeatUntilDiv.Visible = false;
                        }

                        Customer customer = db.Customers.FirstOrDefault(r => r.CustomerId == CustomerID);
                        contactName = customer.FirstName + " " + customer.LastName;
                        hlCompany.Text = customer.Company;
                        hlCompany.NavigateUrl = "javascript:OpenCustomer(" + customer.CustomerId + ")";
                        litStarRating.Text = customer.StarRatingGraphic;
                        if (string.IsNullOrEmpty(customer.Email))
                        {
                            hlContact.Visible = false;
                            lblContact.Text = contactName + "(no email)";
                            lblContact.Visible = true;
                        }
                        else
                        {
                            lblContact.Visible = false;
                            hlContact.Text = contactName.Trim();
                            hlContact.NavigateUrl = "mailto:" + customer.Email;
                            hlContact.Visible = true;
                        }

                        lblCustomerPhone.Text = customer.Phone;
                        lblJournalDate.Text = journal.CreateDate.Value.ToLongDateString();

                        txtFollowup.Text = journal.FollowUpDate != null ? journal.FollowUpDate.Value.ToString("MMMM dd, yyyy") : null;

                        chkResolved.Checked = journal.IsResolved;
                        txtNotes.Text = HttpUtility.HtmlDecode(journal.Notes);

                        if (JournalID != 0)
                        {
                            ddSalesPerson.SelectedValue = journal.AssignedToId.ToString();
                        }

                        if (journal.CampaignID.HasValue)
                        {
                            ddlCampaign.SelectedValue = journal.CampaignID.ToString();
                        }

                        foreach (Redbud.BL.DL.Campaign journalCampaign in journal.Campaigns)
                        {
                            ddlCampaign.Items.FindByValue(journalCampaign.CampaignID.ToString()).Selected = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    litMessage.Text = StringTools.GenerateError(ex.Message);
                }
            }
        }

        private void LoadCampaigns()
        {
            Lookup.LoadCampaignDropDown(ref ddlCampaign);
        }

        private void LoadSales(string defaultUser)
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                List<ListItem> users = db.Users.Where(r => r.Active || r.UserID == 0)
                                         .OrderBy(r => r.SortPosition)
                                         .Select(
                                              r => new ListItem
                                              {
                                                  Value = r.UserID.ToString(),
                                                  Text = r.FirstName + " " + r.LastName,
                                                  Selected = r.EmailAddress == defaultUser
                                              })
                                         .ToList();
                foreach (ListItem user in users)
                {
                    ddSalesPerson.Items.Add(user);
                }
            }
        }
        /// ========================================================================
        /// <summary>
        ///     Saves new journal
        /// </summary>
        /// <returns>bool</returns>
        /// ========================================================================
        private bool CreateFollowUp()
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                List<Redbud.BL.DL.Journal> journals = new List<Redbud.BL.DL.Journal>();
                Redbud.BL.DL.Journal journal = new Redbud.BL.DL.Journal
                {
                    CustomerID = CustomerID,
                    DateStamp = DateTime.Today,
                    Author = HttpContext.Current.User.Identity.Name.Trim(),
                    CreateDate = DateTime.Today,
                    WhoInserted = HttpContext.Current.User.Identity.Name.Trim(),
                    WhoUpdated = HttpContext.Current.User.Identity.Name.Trim(),
                    Notes = HttpUtility.HtmlEncode(txtNotes.Text.Trim()),
                    IsResolved = chkResolved.Checked,
                    AssignedToId = int.Parse(ddSalesPerson.SelectedValue)
                };

                if (journal.IsResolved == false && chkResolved.Checked)
                {
                    // Resolve this journal.
                    journal.ResolvedDate = DateTime.Now;
                }

                //Assign selected campaigns to journal
                foreach (ListItem listItem in ddlCampaign.Items)
                {
                    if (listItem.Selected)
                    {
                        int campaignID = int.Parse(listItem.Value);
                        Redbud.BL.DL.Campaign campaign = db.Campaigns.Find(campaignID);
                        if (campaign != null)
                        {
                            journal.Campaigns.Add(campaign);
                        }
                    }
                }
                var followUpDates = GetFollowUpDates();
                if (followUpDates.Count > 0)
                {
                    foreach (var date in followUpDates)
                    {
                        Redbud.BL.DL.Journal nextJuornal = journal;
                        db.Entry(nextJuornal).State = System.Data.Entity.EntityState.Added;
                        nextJuornal.FollowUpDate = date;
                        db.Journals.Add(nextJuornal);
                        db.SaveChanges();
                    }
                }
                else
                {
                    if (DateTime.TryParse(txtFollowup.Text, out DateTime followUp))
                    {
                        journal.FollowUpDate = followUp;
                    }
                    db.Journals.Add(journal);
                }

                db.SaveChanges();
                return true;
            }
        }
        public List<DateTime> GetFollowUpDates()
        {
            List<DateTime> repeatDates = new List<DateTime>();

            DateTime.TryParse(txtRepeatUntilDate.Text, out DateTime repeatUntil);
            DateTime.TryParse(txtRepeatFromDate.Text, out DateTime nextDate);
            int days = 0, months = 0;
            //int
            if (rdblstRepeat.SelectedItem.Value == "none")
                return repeatDates;
            else if (rdblstRepeat.SelectedItem.Value == "month")
                months = 1;
            else if (rdblstRepeat.SelectedItem.Value == "week")
                days = 7;

            bool repeats = true;

            while (repeats)
            {
                nextDate = nextDate.AddDays(days).AddMonths(months);
                if (nextDate < repeatUntil)
                {
                    repeatDates.Add(nextDate);
                }
                else
                {
                    repeats = false;
                }
            }
            return repeatDates;
        }
        /// ========================================================================
        /// <summary>
        ///     Saves edits to a journal
        /// </summary>
        /// <returns>bool</returns>
        /// ========================================================================
        private bool EditJournal()
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                try
                {
                    Redbud.BL.DL.Journal journal = db.Journals.Find(JournalID);
                    journal.WhoUpdated = HttpContext.Current.User.Identity.Name.Trim();
                    journal.DateUpdated = DateTime.Today;

                    if (DateTime.TryParse(txtFollowup.Text, out DateTime followUp))
                    {
                        journal.FollowUpDate = followUp;
                    }

                    journal.Notes = HttpUtility.HtmlEncode(txtNotes.Text.Trim());
                    journal.IsResolved = chkResolved.Checked;
                    journal.AssignedToId = int.Parse(ddSalesPerson.SelectedValue);

                    if (journal.IsResolved == false && chkResolved.Checked)
                    {
                        // Resolve this journal.
                        journal.ResolvedDate = DateTime.Now;
                    }

                    //Assign selected campaigns to journal
                    foreach (ListItem listItem in ddlCampaign.Items)
                    {
                        if (listItem.Selected)
                        {
                            int campaignID = int.Parse(listItem.Value);
                            Redbud.BL.DL.Campaign campaign = db.Campaigns.Find(campaignID);
                            if (campaign != null)
                            {
                                journal.Campaigns.Add(campaign);
                            }
                        }
                    }

                    bool results = db.SaveChanges() == 1;
                    JournalID = journal.JournalID;
                    return true;

                }
                catch
                {
                    return false;
                }
            }
        }
        /// ========================================================================
        /// <summary>
        ///     Saves new journal and edits to a journal
        /// </summary>
        /// <returns>bool</returns>
        /// ========================================================================
        private bool Save()
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                try
                {
                    bool saved = JournalID == 0 ? CreateFollowUp() : EditJournal();
                    return saved;
                }
                catch (Exception ex)
                {
                    litMessage.Text = StringTools.GenerateError(ex.Message);
                    return false;
                }
            }
        }

        protected void SaveAndClose_Click(object sender, EventArgs e)
        {
            if (rdblstRepeat.SelectedItem != null)
            {
                if ((rdblstRepeat.SelectedItem.Value == "month" || rdblstRepeat.SelectedItem.Value == "week") && string.IsNullOrEmpty(txtRepeatFromDate.Text))
                {
                    RepeatFromRequired.Attributes.Add("style", "");
                    return;
                }
                if ((rdblstRepeat.SelectedItem.Value == "month" || rdblstRepeat.SelectedItem.Value == "week") && string.IsNullOrEmpty(txtRepeatUntilDate.Text))
                {
                    RepeatUntilRequired.Attributes.Add("style", "");
                    return;
                }
            }
            Save();
            CloseWindow(true);
        }

        private void CloseWindow(bool refreshParent)
        {
            Utils util = new Utils();

            util.RegisterStartupScriptBlock(
                "CloseWindow",
                refreshParent ? "window.parent.location.reload();" : "Close();",
                Page);
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            using (MadduxEntities db = new MadduxEntities())
            {
                Redbud.BL.DL.Journal followup = db.Journals.Find(JournalID);

                if (followup != null)
                {
                    db.Journals.Remove(followup);
                    db.SaveChanges();
                }
            }

            CloseWindow(true);
        }
        protected void rdblstRepeat_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (rdblstRepeat.SelectedItem.Value)
            {
                case "week":
                case "month":
                    NextFollowupDiv.Visible = false;
                    RepeatUntilDiv.Visible = true;
                    break;
                case "none":
                    NextFollowupDiv.Visible = true;
                    RepeatUntilDiv.Visible = false;
                    break;
            }

        }
    }
}
