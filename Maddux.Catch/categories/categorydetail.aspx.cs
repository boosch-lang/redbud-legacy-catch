using Redbud.BL.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Maddux.Catch.categories
{
    public partial class categorydetail : Page
    {

        private int CategoryID
        {
            get
            {
                if (ViewState["CategoryID"] == null)
                {
                    ViewState["CategoryID"] = Request.QueryString["CategoryID"] == null || Request.QueryString["CategoryID"] == "" ? 0 : (object)Request.QueryString["CategoryID"];
                }
                return Convert.ToInt32(ViewState["CategoryID"].ToString());
            }

            set
            {
                ViewState["CategoryID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Title = "Maddux.Catch | Category Details";
                Literal litPageHeader = (Literal)Master.FindControl("litPageHeader");
                txtActiveTab.Text = "tab-item-details";
                successAlert.Visible = false;
                errorAlert.Visible = false;
                LoadSupProductCategories();

                if (CategoryID != 0)
                {
                    try
                    {
                        LoadProducts();
                        using (MadduxEntities db = new MadduxEntities())
                        {
                            supProductSubCategory category = db.supProductSubCategories.First(x => x.SubCategoryID == CategoryID);
                            txtCategoryDescription.Text = category.SubCategoryDesc;
                            ddlSupProductCategory.SelectedValue = category.ProductCategoryID.ToString();

                            litPageHeader.Text = "Category: " + category.SubCategoryDesc.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    litPageHeader.Text = "New Category";
                    tabAssociations.Visible = false;
                    btnDelete.Visible = false;
                }
            }
        }
        /// <summary>
        /// Populates categories dropdown
        /// </summary>
        private void LoadSupProductCategories()
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    List<supProductCategory> suProductCategories = db.supProductCategories
                        .OrderBy(x => x.CategoryDesc)
                        .ToList();

                    foreach (supProductCategory suProductCategory in suProductCategories)
                    {
                        ListItem li = new ListItem(suProductCategory.CategoryDesc.ToString(),
                            suProductCategory.CategoryID.ToString());
                        ddlSupProductCategory.Items.Add(li);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Populates table with products assigned to the category selected
        /// </summary>
        private void LoadProducts()
        {
            try
            {
                using (MadduxEntities db = new MadduxEntities())
                {
                    gridProducts.DataSource = db.Products.Where(x => x.supProductSubCategory.SubCategoryID == CategoryID)
                        .OrderBy(x => x.ProductName)
                        .ToList();
                    gridProducts.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void gridProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView gv = (GridView)sender;
            // Change the row state
            gv.Rows[e.NewEditIndex].RowState = DataControlRowState.Edit;
        }

        protected void gridProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        /// <summary>
        /// Delete handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSuccess;

                using (MadduxEntities db = new MadduxEntities())
                {
                    supProductSubCategory category = db.supProductSubCategories.First(x => x.SubCategoryID == CategoryID);
                    db.supProductSubCategories.Remove(category);
                    isSuccess = db.SaveChanges() > 0;

                    if (isSuccess)
                    {
                        successAlert.Visible = true;
                        spSuccessMessage.InnerText = "Category deleted successfully.";
                        Response.AddHeader("REFRESH", "1;URL=/categories/categorylist.aspx");
                    }
                    else
                    {
                        errorAlert.Visible = true;
                        spErrorMessage.InnerText = "An error occurred deleting category. Please contact site administrator";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Cancel handler. Redirect to category list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/categories/categorylist.aspx");
        }
        /// <summary>
        /// Pagination handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SubmitProductsGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridProducts.PageIndex = e.NewPageIndex;
            LoadProducts();
            gridProducts.DataBind();
        }
        /// <summary>
        /// Saving/updating handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bool isSuccess;
                using (MadduxEntities db = new MadduxEntities())
                {
                    if (CategoryID == 0) //New category
                    {
                        supProductSubCategory category = new supProductSubCategory
                        {
                            SubCategoryDesc = txtCategoryDescription.Text.ToString(),
                            ProductCategoryID = Convert.ToInt32(ddlSupProductCategory.SelectedValue)
                        };
                        db.supProductSubCategories.Add(category);
                        isSuccess = db.SaveChanges() > 0;
                        if (isSuccess)
                        {
                            successAlert.Visible = true;
                            spSuccessMessage.InnerText = "Category added successfully.";
                        }
                        else
                        {
                            errorAlert.Visible = true;
                            spErrorMessage.InnerText = "An error occurred creating category. Please contact site administrator";
                        }
                    }
                    else //Existing - update
                    {
                        supProductSubCategory category = db.supProductSubCategories.First(x => x.SubCategoryID == CategoryID);
                        category.SubCategoryDesc = txtCategoryDescription.Text.ToString();
                        category.ProductCategoryID = Convert.ToInt32(ddlSupProductCategory.SelectedValue);
                        isSuccess = db.SaveChanges() > 0;
                        if (isSuccess)
                        {
                            successAlert.Visible = true;
                            spSuccessMessage.InnerText = "Category updated successfully.";
                        }
                        else
                        {
                            errorAlert.Visible = true;
                            spErrorMessage.InnerText = "An error occurred updating. Please contact site administrator";
                        }
                    }
                    txtActiveTab.Text = "tab-item-details";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}