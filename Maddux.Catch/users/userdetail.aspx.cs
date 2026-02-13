using Maddux.Catch.LocalClasses;
using Redbud.BL.DL;
using Redbud.BL.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Maddux.Catch.users
{
    public partial class userdetail : System.Web.UI.Page
    {
        private User currentUser;
        /// <summary>
        /// The user id of the user we are loading details
        /// </summary>
        private int UserID
        {
            get
            {
                if (ViewState["UserId"] == null)
                {
                    ViewState["UserId"] = Request.QueryString["UserId"] == null || Request.QueryString["UserId"] == "" ? -1 : (object)Request.QueryString["UserId"];
                }
                return Convert.ToInt32(ViewState["UserId"].ToString());
            }

            set
            {
                ViewState["UserId"] = value;
            }
        }
        /// <summary>
        /// Handles creating and updating user
        /// </summary>
        /// <returns></returns>
        private bool Save()
        {
            bool _bSaveSuccess, newUser;
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {

                    newUser = UserID == -1;

                    if (newUser)
                    {
                        User userAccount = new User
                        {
                            FirstName = txtFirstName.Text.Trim(),
                            LastName = txtLastName.Text.Trim(),
                            Gender = ddlGender.Text.Trim(),
                            EmailAddress = txtEmailAddress.Text.Trim(),
                            Password = Redbud.BL.Utils.FCSEncryption.Encrypt(txtUserPassword.Text),
                            HighlightBackgroundColour = ddlHighlightBackColour.SelectedValue,
                            HighlightForegroundColour = ddlHighlightForeColour.SelectedValue,
                            Active = chkActive.Checked,
                            DefaultToPersonalJournals = chkDefaultToPersonalJournals.Checked,
                            ShowOtherMyJournals = chkShowOtherMyJournals.Checked,
                            DefaultToPersonalOrders = chkDefaultToPersonalOrders.Checked,
                            ShowOtherMyOrders = chkShowOtherMyOrders.Checked,
                            DefaultToPersonalShipments = chkDefaultToPersonalShipments.Checked,
                            ShowOtherMyShipments = chkShowOtherMyShipments.Checked,

                            CanOnlyViewOwnCustomers = chkCanOnlyViewOwnCustomers.Checked,
                            CanOnlyViewAssignedAssociations = chkCanOnlyViewAssignedAssociations.Checked,
                            CanOnlyViewAssignedProvinces = chkCanOnlyViewAssignedProvinces.Checked,
                            CanOnlyViewAssignedCatalogs = chkCanOnlyViewAssignedCatalogs.Checked,

                            CanDeleteCustomers = chkCanDeleteCustomers.Checked,
                            CanMergeCustomers = chkCanMergeCustomers.Checked,
                            CanViewCustomerAssociations = chkCanViewCustomerAssociations.Checked,

                            CanEditOrders = chkCanEditOrders.Checked,
                            CanDeleteOrders = chkCanDeleteOrders.Checked,
                            CanEmailPrintExportOrders = chkCanEmailPrintExportOrders.Checked,
                            CanApproveOrders = chkCanApproveOrders.Checked,

                            CanEditShipments = chkCanEditShipments.Checked,
                            CanDeleteShipments = chkCanDeleteShipments.Checked,
                            CanEmailPrintExportShipments = chkCanEmailPrintExportShipments.Checked,

                            CanEditCreditMemos = chkCanEditCreditMemos.Checked,
                            CanDeleteCreditMemos = chkCanDeleteCreditMemos.Checked,
                            CanEmailPrintExportCreditMemos = chkCanEmailPrintExportCreditMemos.Checked,

                            ShowSettings = chkShowSettings.Checked,
                            CanCreateLabels = chkCanCreateLabels.Checked,
                            CanSendNewsletters = chkCanSendNewsletters.Checked,
                            CanViewMyAccountActivity = chkCanViewMyAccountActivity.Checked
                        };

                        db.Users.Add(userAccount);
                        bool saved = db.SaveChanges() > 0 ? true : false;

                        LoadAssociations();
                        LoadProvinces();
                        LoadCatalogs();
                        _bSaveSuccess = saved;
                        if (_bSaveSuccess)
                        {
                            UserID = userAccount.UserID;
                            Header.Title = txtFirstName.Text + " " + txtLastName.Text;
                            tabProvinces.Visible = true;
                            tabDetails.Visible = true;
                            tabCatalogues.Visible = true;
                            tabAssociations.Visible = true;
                        }
                        return _bSaveSuccess;

                    }
                    else
                    {
                        User userAccount = db.Users.FirstOrDefault(x => x.UserID == UserID);

                        userAccount.FirstName = txtFirstName.Text.Trim();
                        userAccount.LastName = txtLastName.Text.Trim();
                        userAccount.EmailAddress = txtEmailAddress.Text.Trim();
                        userAccount.HighlightBackgroundColour = ddlHighlightBackColour.SelectedValue;
                        userAccount.HighlightForegroundColour = ddlHighlightForeColour.SelectedValue;
                        userAccount.Active = chkActive.Checked;

                        userAccount.DefaultToPersonalJournals = chkDefaultToPersonalJournals.Checked;
                        userAccount.ShowOtherMyJournals = chkShowOtherMyJournals.Checked;
                        userAccount.DefaultToPersonalOrders = chkDefaultToPersonalOrders.Checked;
                        userAccount.ShowOtherMyOrders = chkShowOtherMyOrders.Checked;
                        userAccount.DefaultToPersonalShipments = chkDefaultToPersonalShipments.Checked;
                        userAccount.ShowOtherMyShipments = chkShowOtherMyShipments.Checked;

                        userAccount.CanOnlyViewOwnCustomers = chkCanOnlyViewOwnCustomers.Checked;
                        userAccount.CanOnlyViewAssignedAssociations = chkCanOnlyViewAssignedAssociations.Checked;
                        userAccount.CanOnlyViewAssignedProvinces = chkCanOnlyViewAssignedProvinces.Checked;
                        userAccount.CanOnlyViewAssignedCatalogs = chkCanOnlyViewAssignedCatalogs.Checked;

                        userAccount.CanDeleteCustomers = chkCanDeleteCustomers.Checked;
                        userAccount.CanMergeCustomers = chkCanMergeCustomers.Checked;
                        userAccount.CanViewCustomerAssociations = chkCanViewCustomerAssociations.Checked;

                        userAccount.CanEditOrders = chkCanEditOrders.Checked;
                        userAccount.CanDeleteOrders = chkCanDeleteOrders.Checked;
                        userAccount.CanEmailPrintExportOrders = chkCanEmailPrintExportOrders.Checked;
                        userAccount.CanApproveOrders = chkCanApproveOrders.Checked;

                        userAccount.CanEditShipments = chkCanEditShipments.Checked;
                        userAccount.CanDeleteShipments = chkCanDeleteShipments.Checked;
                        userAccount.CanEmailPrintExportShipments = chkCanEmailPrintExportShipments.Checked;

                        userAccount.CanEditCreditMemos = chkCanEditCreditMemos.Checked;
                        userAccount.CanDeleteCreditMemos = chkCanDeleteCreditMemos.Checked;
                        userAccount.CanEmailPrintExportCreditMemos = chkCanEmailPrintExportCreditMemos.Checked;

                        userAccount.ShowSettings = chkShowSettings.Checked;
                        userAccount.CanCreateLabels = chkCanCreateLabels.Checked;
                        userAccount.CanSendNewsletters = chkCanSendNewsletters.Checked;
                        userAccount.CanViewMyAccountActivity = chkCanViewMyAccountActivity.Checked;

                        _bSaveSuccess = db.SaveChanges() > 0 ? true : false;
                        if (_bSaveSuccess)
                        {
                            UserID = userAccount.UserID;
                            Header.Title = txtFirstName.Text + " " + txtLastName.Text;
                            tabProvinces.Visible = true;
                            tabDetails.Visible = true;
                            tabCatalogues.Visible = true;
                            tabAssociations.Visible = true;
                        }
                        return _bSaveSuccess;
                    }
                }
            }
            catch (DbEntityValidationException)
            {
                throw;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = AppSession.Current.CurrentUser;
            successAlert.Visible = false;         //To diplay success messages  
            passwordField.Visible = false;       //The Password field is visible when adding a new user   
            genderField.Visible = false;        //Gender is required on db so if we have a new user we must get the gender
            txtActiveTab.Text = "tab-item-details";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (!IsPostBack)
            {
                Title = "Maddux.Catch | User Details";
                try
                {
                    LoadColourCombo(ref ddlHighlightBackColour, "#ffffff");
                    LoadColourCombo(ref ddlHighlightForeColour, "#000000");
                    LoadUserData();
                }
                catch (Exception ex)
                {
                    litMessage.Text = StringTools.GenerateError(ex.Message);
                }
            }
        }
        /// <summary>
        /// Generates colours
        /// </summary>
        /// <param name="dropdown"> Dropdown ID</param>
        /// <param name="defaultColour">The colour</param>
        public static void LoadColourCombo(ref DropDownList dropdown, string defaultColour)
        {
            try
            {
                ArrayList ColorList = new ArrayList();
                Type colorType = typeof(System.Drawing.Color);
                System.Reflection.PropertyInfo[] propInfoList = colorType.GetProperties(System.Reflection.BindingFlags.Static |
                                              System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.Public);
                foreach (System.Reflection.PropertyInfo c in propInfoList)
                {
                    string colorName = c.Name;
                    string colorHTML = "";

                    try
                    {
                        System.Drawing.Color tmpColour = System.Drawing.Color.FromName(colorName);
                        colorHTML = "#" + tmpColour.R.ToString("X2") + tmpColour.G.ToString("X2") + tmpColour.B.ToString("X2");
                    }
                    catch
                    {
                        colorHTML = defaultColour;
                    }

                    ListItem li = new ListItem(colorName, colorHTML);
                    dropdown.Items.Add(li);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Users data
        /// </summary>
        private void LoadUserData()
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");

                    if (UserID == -1)
                    {
                        //This is a new user so this fields and buttons are removed
                        tabProvinces.Visible = false;
                        tabDetails.Visible = false;
                        tabCatalogues.Visible = false;
                        tabAssociations.Visible = false;
                        btnDeleteUser.Visible = false;
                        //New user so we need password and gender
                        ListItem male = new ListItem("Male", "M");
                        ddlGender.Items.Add(male);
                        ListItem female = new ListItem("Female", "F");
                        ddlGender.Items.Add(female);
                        passwordField.Visible = true;
                        genderField.Visible = true;

                    }
                    else
                    {
                        LoadAssociations();
                        LoadProvinces();
                        LoadCatalogs();
                        btnDeleteUser.Visible = true;
                    }

                    litPageHeader.Text = "User details";

                    if (UserID != -1)
                    {
                        User userAccount = db.Users.FirstOrDefault(x => x.UserID == UserID);
                        litPageHeader.Text = userAccount.FirstName + " " + userAccount.LastName;
                        txtFirstName.Text = userAccount.FirstName;
                        txtLastName.Text = userAccount.LastName;

                        txtEmailAddress.Text = userAccount.EmailAddress;

                        chkActive.Checked = userAccount.Active;
                        ddlHighlightBackColour.SelectedValue = userAccount.HighlightBackgroundColour;
                        ddlHighlightForeColour.SelectedValue = userAccount.HighlightForegroundColour;

                        chkDefaultToPersonalJournals.Checked = userAccount.DefaultToPersonalJournals;
                        chkShowOtherMyJournals.Checked = userAccount.ShowOtherMyJournals;
                        chkDefaultToPersonalOrders.Checked = userAccount.DefaultToPersonalOrders;
                        chkShowOtherMyOrders.Checked = userAccount.ShowOtherMyOrders;
                        chkDefaultToPersonalShipments.Checked = userAccount.DefaultToPersonalShipments;
                        chkShowOtherMyShipments.Checked = userAccount.ShowOtherMyShipments;

                        chkCanOnlyViewOwnCustomers.Checked = userAccount.CanOnlyViewOwnCustomers;
                        chkCanOnlyViewAssignedAssociations.Checked = userAccount.CanOnlyViewAssignedAssociations;
                        chkCanOnlyViewAssignedProvinces.Checked = userAccount.CanOnlyViewAssignedProvinces;
                        chkCanOnlyViewAssignedCatalogs.Checked = userAccount.CanOnlyViewAssignedCatalogs;

                        chkCanDeleteCustomers.Checked = userAccount.CanDeleteCustomers;
                        chkCanMergeCustomers.Checked = userAccount.CanMergeCustomers;
                        chkCanViewCustomerAssociations.Checked = userAccount.CanViewCustomerAssociations;

                        chkCanEditOrders.Checked = userAccount.CanEditOrders;
                        chkCanDeleteOrders.Checked = userAccount.CanDeleteOrders;
                        chkCanEmailPrintExportOrders.Checked = userAccount.CanEmailPrintExportOrders;
                        chkCanApproveOrders.Checked = userAccount.CanApproveOrders;

                        chkCanEditShipments.Checked = userAccount.CanEditShipments;
                        chkCanDeleteShipments.Checked = userAccount.CanDeleteShipments;
                        chkCanEmailPrintExportShipments.Checked = userAccount.CanEmailPrintExportShipments;

                        chkCanEditCreditMemos.Checked = userAccount.CanEditCreditMemos;
                        chkCanDeleteCreditMemos.Checked = userAccount.CanDeleteCreditMemos;
                        chkCanEmailPrintExportCreditMemos.Checked = userAccount.CanEmailPrintExportCreditMemos;

                        chkShowSettings.Checked = userAccount.ShowSettings;
                        chkCanCreateLabels.Checked = userAccount.CanCreateLabels;
                        chkCanSendNewsletters.Checked = userAccount.CanSendNewsletters;
                        chkCanViewMyAccountActivity.Checked = userAccount.CanViewMyAccountActivity;
                    }
                    else
                    {
                        litPageHeader.Text = "New User";
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }

        /// <summary>
        /// Handle filtering and loading the associations
        /// </summary>
        private void LoadAssociations()
        {
            ListItem _li;
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {

                    User userAccount = db.Users.FirstOrDefault(x => x.UserID == UserID);
                    //Get the associations IDs for the user
                    List<int> associationIDs = userAccount.UserAsscs.Select(x => x.AssociationID).ToList();

                    //Load up the list associated with the user
                    foreach (int associationID in associationIDs)
                    {
                        Association userAssociation = db.Associations.FirstOrDefault(x => x.AssociationID == associationID);
                        _li = new ListItem(userAssociation.AsscDesc.ToString().Trim(), userAssociation.AssociationID.ToString());
                        lbEmpAssc.Items.Add(_li);
                    }



                    //Loads up the associations not already associated with the user
                    foreach (Association availableAssociation in db.Associations.Where(x => !associationIDs.Contains(x.AssociationID)))
                    {
                        _li = new ListItem(availableAssociation.AsscDesc.ToString().Trim(), availableAssociation.AssociationID.ToString());
                        lbAssociations.Items.Add(_li);
                    }
                };
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }


        /// <summary>
        /// Handles filtering and loading provinces/states
        /// </summary>
        private void LoadProvinces()
        {
            ListItem _li;

            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    User userAccount = db.Users.FirstOrDefault(x => x.UserID == UserID);
                    //The StateID is varchar(2) oh yeah
                    List<string> statesIDs = userAccount.UserStates.Select(x => x.StateID).ToList();

                    //Load up assigned provinces
                    foreach (string stateID in statesIDs)
                    {
                        State state = db.States.FirstOrDefault(x => x.StateID == stateID && string.Equals(x.Country, "Canada"));
                        _li = new ListItem(state.StateName.ToString().Trim(), state.StateID.ToString());
                        lbEmpProvinces.Items.Add(_li);
                    }



                    //Load up states not already associated with the user
                    foreach (State availableState in db.States.Where(x => !statesIDs.Contains(x.StateID) && string.Equals(x.Country, "Canada")))
                    {
                        _li = new ListItem(availableState.StateName.ToString().Trim(), availableState.StateID.ToString());
                        lbProvinces.Items.Add(_li);
                    }
                };
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Handles filtering and loading product catalog
        /// </summary>
        private void LoadCatalogs()
        {
            ListItem _li;

            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    User userAccount = db.Users.FirstOrDefault(x => x.UserID == UserID);

                    //Product catalogs already linked to the user
                    List<int> productCatalogIDs = userAccount.UserCatalogs.Select(x => x.CatalogID).ToList();
                    //Load up assigned catalog
                    foreach (int catalogID in productCatalogIDs)
                    {
                        ProductCatalog productCatalog = db.ProductCatalogs.FirstOrDefault(x => x.CatalogId == catalogID);
                        _li = new ListItem(productCatalog.CatalogName.ToString().Trim(), productCatalog.CatalogId.ToString());
                        lbEmpCatalogs.Items.Add(_li);
                    }
                    //Load up unassigned catalog
                    IQueryable<ProductCatalog> productCatalogs = db.ProductCatalogs.Where(x => !productCatalogIDs.Contains(x.CatalogId))
                        .OrderByDescending(x => x.CatalogYear);
                    foreach (ProductCatalog productCatalog in productCatalogs)
                    {
                        _li = new ListItem(productCatalog.CatalogName.ToString().Trim(), productCatalog.CatalogId.ToString());
                        lbCatalogs.Items.Add(_li);
                    }
                }
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Add association
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAddAssc_Click(object sender, EventArgs e)
        {
            foreach (ListItem _liItem in lbAssociations.Items)
            {
                if (_liItem.Selected == true)
                {
                    if (lbEmpAssc.Items.Count > 0)
                    {
                        int _iIndex = 0;
                        bool _bInserted = false;
                        foreach (ListItem _liExistingItem in lbEmpAssc.Items)
                        {
                            if (string.Compare(_liExistingItem.Text.ToString(), _liItem.Text.ToString()) >= 0)
                            {
                                lbEmpAssc.Items.Insert(_iIndex, _liItem);
                                _bInserted = true;
                                break;
                            }
                            _iIndex++;
                        }

                        if (_bInserted == false)
                        {
                            lbEmpAssc.Items.Add(_liItem);
                        }
                    }
                    else
                    {
                        lbEmpAssc.Items.Add(_liItem);
                    }
                    _liItem.Selected = false;
                    lbAssociations.Items.Remove(_liItem);
                    break;
                }
            }

            txtActiveTab.Text = "tab-item-associations";
        }
        /// <summary>
        /// Remove association
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdRemAssc_Click(object sender, EventArgs e)
        {
            foreach (ListItem _liItem in lbEmpAssc.Items)
            {
                if (_liItem.Selected == true)
                {
                    if (lbAssociations.Items.Count > 0)
                    {
                        int _iIndex = 0;
                        bool _bInserted = false;
                        foreach (ListItem _liExistingItem in lbAssociations.Items)
                        {
                            if (String.Compare(_liExistingItem.Text.ToString(), _liItem.Text.ToString()) >= 0)
                            {
                                lbAssociations.Items.Insert(_iIndex, _liItem);
                                _bInserted = true;
                                break;
                            }
                            _iIndex++;
                        }

                        if (_bInserted == false)
                        {
                            lbAssociations.Items.Add(_liItem);
                        }
                    }
                    else
                    {
                        lbAssociations.Items.Add(_liItem);
                    }
                    _liItem.Selected = false;
                    lbEmpAssc.Items.Remove(_liItem);
                    break;
                }
            }

            txtActiveTab.Text = "tab-item-associations";
        }
        /// <summary>
        /// Save associations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSaveAssc_Click(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    User userAccount = db.Users.FirstOrDefault(x => x.UserID == UserID);



                    //Remove the associations
                    foreach (UserAssc userAssoc in db.UserAsscs.Where(x => x.UserID == UserID))
                    {
                        db.UserAsscs.Remove(userAssoc);
                    }
                    db.SaveChanges();

                    //Add newly selected associations
                    foreach (ListItem _liItem in lbEmpAssc.Items)
                    {
                        UserAssc newAssociation = new UserAssc
                        {
                            UserID = UserID,
                            AssociationID = Convert.ToInt32(_liItem.Value)
                        };
                        db.UserAsscs.Add(newAssociation);
                    }
                    db.SaveChanges();
                }

                txtActiveTab.Text = "tab-item-associations";

                successAlert.Visible = true;
                spSuccessMessage.InnerText = "Association(s)  saved/removed successfully.";
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Add Province
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAddProv_Click(object sender, EventArgs e)
        {
            foreach (ListItem _liItem in lbProvinces.Items)
            {
                if (_liItem.Selected == true)
                {
                    if (lbEmpProvinces.Items.Count > 0)
                    {
                        int _iIndex = 0;
                        bool _bInserted = false;
                        foreach (ListItem _liExistingItem in lbEmpProvinces.Items)
                        {
                            if (String.Compare(_liExistingItem.Text.ToString(), _liItem.Text.ToString()) >= 0)
                            {
                                lbEmpProvinces.Items.Insert(_iIndex, _liItem);
                                _bInserted = true;
                                break;
                            }
                            _iIndex++;
                        }

                        if (_bInserted == false)
                        {
                            lbEmpProvinces.Items.Add(_liItem);
                        }
                    }
                    else
                    {
                        lbEmpProvinces.Items.Add(_liItem);
                    }
                    _liItem.Selected = false;
                    lbProvinces.Items.Remove(_liItem);
                    break;
                }
            }

            txtActiveTab.Text = "tab-item-provinces";
        }
        /// <summary>
        /// Remove province
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdRemProv_Click(object sender, EventArgs e)
        {
            foreach (ListItem _liItem in lbEmpProvinces.Items)
            {
                if (_liItem.Selected == true)
                {
                    if (lbProvinces.Items.Count > 0)
                    {
                        int _iIndex = 0;
                        bool _bInserted = false;
                        foreach (ListItem _liExistingItem in lbProvinces.Items)
                        {
                            if (string.Compare(_liExistingItem.Text.ToString(), _liItem.Text.ToString()) >= 0)
                            {
                                lbProvinces.Items.Insert(_iIndex, _liItem);
                                _bInserted = true;
                                break;
                            }
                            _iIndex++;
                        }

                        if (_bInserted == false)
                        {
                            lbProvinces.Items.Add(_liItem);
                        }
                    }
                    else
                    {
                        lbProvinces.Items.Add(_liItem);
                    }
                    _liItem.Selected = false;
                    lbEmpProvinces.Items.Remove(_liItem);
                    break;
                }
            }

            txtActiveTab.Text = "tab-item-provinces";
        }
        /// <summary>
        /// Save province/state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSaveProv_Click(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    User userAccount = db.Users.FirstOrDefault(x => x.UserID == UserID);



                    //Remove provice
                    foreach (UserState userProvince in db.UserStates.Where(x => x.UserID == UserID))
                    {
                        db.UserStates.Remove(userProvince);
                    }
                    db.SaveChanges();

                    //Add province
                    foreach (ListItem _liItem in lbEmpProvinces.Items)
                    {
                        UserState newProvince = new UserState
                        {
                            UserID = UserID,
                            StateID = Convert.ToString(_liItem.Value)
                        };
                        db.UserStates.Add(newProvince);
                    }
                    db.SaveChanges();
                }
                txtActiveTab.Text = "tab-item-provinces";

                successAlert.Visible = true;
                spSuccessMessage.InnerText = "Province(s)  saved/removed successfully.";
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Add Catalog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdAddCat_Click(object sender, EventArgs e)
        {
            foreach (ListItem _liItem in lbCatalogs.Items)
            {
                if (_liItem.Selected == true)
                {
                    if (lbEmpCatalogs.Items.Count > 0)
                    {
                        int _iIndex = 0;
                        bool _bInserted = false;
                        foreach (ListItem _liExistingItem in lbEmpCatalogs.Items)
                        {
                            if (string.Compare(_liExistingItem.Text.ToString(), _liItem.Text.ToString()) >= 0)
                            {
                                lbEmpCatalogs.Items.Insert(_iIndex, _liItem);
                                _bInserted = true;
                                break;
                            }
                            _iIndex++;
                        }

                        if (_bInserted == false)
                        {
                            lbEmpCatalogs.Items.Add(_liItem);
                        }
                    }
                    else
                    {
                        lbEmpCatalogs.Items.Add(_liItem);
                    }
                    _liItem.Selected = false;
                    lbCatalogs.Items.Remove(_liItem);
                    break;
                }
            }
            txtActiveTab.Text = "tab-item-catalogues";
        }
        /// <summary>
        /// Remove catalog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdRemCat_Click(object sender, EventArgs e)
        {
            foreach (ListItem _liItem in lbEmpCatalogs.Items)
            {
                if (_liItem.Selected == true)
                {
                    if (lbCatalogs.Items.Count > 0)
                    {
                        int _iIndex = 0;
                        bool _bInserted = false;
                        foreach (ListItem _liExistingItem in lbCatalogs.Items)
                        {
                            if (String.Compare(_liExistingItem.Text.ToString(), _liItem.Text.ToString()) >= 0)
                            {
                                lbCatalogs.Items.Insert(_iIndex, _liItem);
                                _bInserted = true;
                                break;
                            }
                            _iIndex++;
                        }

                        if (_bInserted == false)
                        {
                            lbCatalogs.Items.Add(_liItem);
                        }
                    }
                    else
                    {
                        lbCatalogs.Items.Add(_liItem);
                    }
                    _liItem.Selected = false;
                    lbEmpCatalogs.Items.Remove(_liItem);
                    break;
                }
            }

            txtActiveTab.Text = "tab-item-catalogues";
        }
        /// <summary>
        /// Save catalog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmdSaveCat_Click(object sender, EventArgs e)
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    User userAccount = db.Users.FirstOrDefault(x => x.UserID == UserID);



                    //Remove catalog
                    foreach (UserCatalog userCatalog in db.UserCatalogs.Where(x => x.UserID == UserID))
                    {
                        db.UserCatalogs.Remove(userCatalog);
                    }
                    db.SaveChanges();

                    //Add catalog
                    foreach (ListItem _liItem in lbEmpCatalogs.Items)
                    {
                        UserCatalog newCatalog = new UserCatalog
                        {
                            UserID = UserID,
                            CatalogID = Convert.ToInt32(_liItem.Value)
                        };
                        db.UserCatalogs.Add(newCatalog);
                    }
                    db.SaveChanges();
                }

                successAlert.Visible = true;
                spSuccessMessage.InnerText = "Catalog(s)  saved/removed successfully.";

                txtActiveTab.Text = "tab-item-catalogues";
            }
            catch (Exception ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }
        /// <summary>
        /// Save user details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isSaved = Save();
            if (isSaved)
            {
                successAlert.Visible = true;
                spSuccessMessage.InnerText = "User Added/updated successfully.";
                Response.AddHeader("REFRESH", "1;URL=/users/userlist.aspx");
            }
        }
        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new MadduxEntities())
                {
                    User userAccount = db.Users.FirstOrDefault(x => x.UserID == UserID);

                    List<Order> userOrders = db.Orders.Where(x => x.SalesPersonID == UserID).ToList();

                    //Id the user has order we dont want to remove them so set the SalesPersonID (UserID) in
                    //user table = 0. UserID 0 is a unassigned account in the db, yeah yeah
                    if (userOrders.Count() > 0)
                    {
                        userOrders.ForEach(a => a.SalesPersonID = 0);
                        db.SaveChanges();
                    }
                    userAccount.Active = false;
                    //db.Users.Remove(userAccount);
                    db.SaveChanges();
                    successAlert.Visible = true;
                    spSuccessMessage.InnerText = "User deleted successfully.";
                    Response.AddHeader("REFRESH", "1;URL=/users/userlist.aspx");
                }
            }
            catch (DbEntityValidationException ex)
            {
                litMessage.Text = StringTools.GenerateError(ex.Message);
            }
        }/// <summary>
         /// Cancel changes
         /// </summary>
         /// <param name="sender"></param>
         /// <param name="e"></param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/users/userlist.aspx");
        }
    }
}
