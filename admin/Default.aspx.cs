using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CMS;

public partial class Admin_Default : KensoftPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnBack_Click(object sender, ImageClickEventArgs e)
    {

        Response.Redirect(string.Format("Default.aspx?mod={0}", Request["mod"].Replace("-p","-g")));
    }

    protected void page_Init(object sender, EventArgs e)
    {
        CMS.Config.CMSConfigurations config = CMS.Config.CMSConfigurations.GetSection();
        switch (Request["mod"])
        {
            case "art-g":
                CMS.Admin.ArticleGrid artGrid = new CMS.Admin.ArticleGrid();
                phControl.Controls.Add(artGrid);
                ltlTitle.Text = Resources.CMS.Articles;
                btnBack.Visible = false;
                break;
            case "art-p":
                CMS.Admin.ArticlePanel artPanel = new CMS.Admin.ArticlePanel();
   
                phControl.Controls.Add(artPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.ArticleAdd : Resources.CMS.ArticleEdit);
                btnBack.Visible = true;
                break;
            case "artcat-g":
                CMS.Admin.ArticleTypeGrid artcatGrid = new CMS.Admin.ArticleTypeGrid();
                phControl.Controls.Add(artcatGrid);
                ltlTitle.Text = Resources.CMS.ArticleCategories;
                btnBack.Visible = false;
                break;
            case "artcat-p":
                CMS.Admin.ArticleTypePanel artcatPanel = new CMS.Admin.ArticleTypePanel();
                phControl.Controls.Add(artcatPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.ArticleCategoryAdd : Resources.CMS.ArticleCategoryEdit);
                btnBack.Visible = true;
                break;
            case "evt-g":
                CMS.Admin.EventGrid evtGrid = new CMS.Admin.EventGrid();
                phControl.Controls.Add(evtGrid);
                ltlTitle.Text = Resources.CMS.Events;
                btnBack.Visible = false;
                break;
            case "evt-p":
                CMS.Admin.EventPanel evtPanel = new CMS.Admin.EventPanel();
                phControl.Controls.Add(evtPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.EventAdd : Resources.CMS.EventEdit);
                btnBack.Visible = true;
                break;
            case "evtcat-g":
                CMS.Admin.EventTypeGrid evtcatGrid = new CMS.Admin.EventTypeGrid();
                phControl.Controls.Add(evtcatGrid);
                ltlTitle.Text = Resources.CMS.EventCategories;
                btnBack.Visible = false;
                break;
            case "evtcat-p":
                CMS.Admin.EventTypePanel evtcatPanel = new CMS.Admin.EventTypePanel();
                phControl.Controls.Add(evtcatPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.EventCategoryAdd : Resources.CMS.EventCategoryEdit);
                btnBack.Visible = true;
                break;
            case "pag-g":
                CMS.Admin.StaticPageGrid pagGrid = new CMS.Admin.StaticPageGrid();
                phControl.Controls.Add(pagGrid);
                ltlTitle.Text = Resources.CMS.Pages;
                btnBack.Visible = false;
                break;
            case "pag-p":
                CMS.Admin.StaticPagePanel pagPanel = new CMS.Admin.StaticPagePanel();
                phControl.Controls.Add(pagPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.PageAdd : Resources.CMS.PageEdit);
                btnBack.Visible = true;
                break;
            case "pro-g":
                CMS.Admin.ProductGrid proGrid = new CMS.Admin.ProductGrid();
                phControl.Controls.Add(proGrid);
                ltlTitle.Text = Resources.CMS.Products;
                btnBack.Visible = false;
                break;
            case "pro-p":
                CMS.Admin.ProductPanel proPanel = new CMS.Admin.ProductPanel();
                proPanel.OldPostNumbering = false;
                proPanel.CommerceEnabled = true;
                proPanel.ShippingEnabled = true;
                proPanel.OwnerEnabled = true;
                proPanel.DateEnabled = true;
                proPanel.ParentEnabled = true;
                phControl.Controls.Add(proPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.ProductAdd : Resources.CMS.ProductEdit);
                btnBack.Visible = true;
                break;
            case "procat-g":
                CMS.Admin.CategoryGrid procatGrid = new CMS.Admin.CategoryGrid();
                phControl.Controls.Add(procatGrid);
                ltlTitle.Text = Resources.CMS.Categories;
                btnBack.Visible = false;
                break;
            case "procat-p":
                CMS.Admin.CategoryPanel procatPanel = new CMS.Admin.CategoryPanel();
                phControl.Controls.Add(procatPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.CategoryAdd : Resources.CMS.CategoryEdit);
                btnBack.Visible = true;
                break;
            case "probra-g":
                CMS.Admin.BrandGrid probraGrid = new CMS.Admin.BrandGrid();
                phControl.Controls.Add(probraGrid);
                ltlTitle.Text = Resources.CMS.Brands;
                btnBack.Visible = false;
                break;
            case "probra-p":
                CMS.Admin.BrandPanel probraPanel = new CMS.Admin.BrandPanel();
                phControl.Controls.Add(probraPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.BrandAdd : Resources.CMS.BrandEdit);
                btnBack.Visible = true;
                break;
            case "procatspec-g":
                CMS.Admin.SpecGrid proSpecGrid = new CMS.Admin.SpecGrid();
                proSpecGrid.CatID = Request["catid"];
                phControl.Controls.Add(proSpecGrid);
                ltlTitle.Text = Resources.CMS.CategorySpecs;
                btnBack.Visible = false;
                break;
            case "procatspec-p":
                CMS.Admin.SpecPanel proSpecPanel = new CMS.Admin.SpecPanel();
                proSpecPanel.CatID = Request["catid"];
                phControl.Controls.Add(proSpecPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.SpecAdd : Resources.CMS.SpecEdit);
                btnBack.Visible = true;
                break;
            case "lok-g":
                CMS.Admin.LookupItemGrid lokGrid = new CMS.Admin.LookupItemGrid();
                phControl.Controls.Add(lokGrid);
                ltlTitle.Text = Resources.CMS.Lookups;
                btnBack.Visible = false;
                break;
            case "lok-p":
                CMS.Admin.LookupItemPanel lokPanel = new CMS.Admin.LookupItemPanel();
                phControl.Controls.Add(lokPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.LookupAdd : Resources.CMS.LookupEdit);
                btnBack.Visible = true;
                break;
            case "lokcat-g":
                CMS.Admin.LookupCategoryGrid lokcatGrid = new CMS.Admin.LookupCategoryGrid();
                phControl.Controls.Add(lokcatGrid);
                ltlTitle.Text = Resources.CMS.LookupCategories;
                btnBack.Visible = false;
                break;
            case "lokcat-p":
                CMS.Admin.LookupCategoryPanel lokcatPanel = new CMS.Admin.LookupCategoryPanel();
                phControl.Controls.Add(lokcatPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.LookupCategoryAdd : Resources.CMS.LookupCategoryEdit);
                btnBack.Visible = true;
                break;
            case "gal-g":
                CMS.Admin.PhotoGrid galGrid = new CMS.Admin.PhotoGrid();
                phControl.Controls.Add(galGrid);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["pub"])?Resources.CMS.Photos:Resources.CMS.PhotosUnpublished);
                btnBack.Visible = false;
                break;
            case "gal-p":
                CMS.Admin.PhotoPanel galPanel = new CMS.Admin.PhotoPanel();
                phControl.Controls.Add(galPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.PhotoAdd : Resources.CMS.PhotoEdit);
                btnBack.Visible = true;
                break;
            case "galcat-g":
                CMS.Admin.AlbumGrid galcatGrid = new CMS.Admin.AlbumGrid();
                phControl.Controls.Add(galcatGrid);
                ltlTitle.Text = Resources.CMS.Albums;
                btnBack.Visible = false;
                break;
            case "galcat-p":
                CMS.Admin.AlbumPanel galcatPanel = new CMS.Admin.AlbumPanel();
                phControl.Controls.Add(galcatPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.AlbumAdd : Resources.CMS.AlbumEdit);
                btnBack.Visible = true;
                break;
            case "usr-g":
                CMS.Admin.UserGrid usrGrid = new CMS.Admin.UserGrid();
                phControl.Controls.Add(usrGrid);
                ltlTitle.Text = Resources.CMS.Users;
                btnBack.Visible = false;
                break;
            case "usr-p":
                CMS.Admin.UserPanel usrPanel = new CMS.Admin.UserPanel();
                phControl.Controls.Add(usrPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.UserAdd : Resources.CMS.UserEdit);
                btnBack.Visible = true;
                break;
            case "men-g":
                CMS.Admin.MenuGrid menGrid = new CMS.Admin.MenuGrid();
                phControl.Controls.Add(menGrid);
                ltlTitle.Text = Resources.CMS.Menu;
                btnBack.Visible = false;
                break;
            case "men-p":
                CMS.Admin.MenuPanel menPanel = new CMS.Admin.MenuPanel();
                phControl.Controls.Add(menPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.MenuAdd : Resources.CMS.MenuEdit);
                btnBack.Visible = true;
                break;
            case "cnt-g":
                CMS.Admin.ContentFileGrid cntGrid = new CMS.Admin.ContentFileGrid();
                phControl.Controls.Add(cntGrid);
                ltlTitle.Text = Resources.CMS.ContentFiles;
                btnBack.Visible = false;
                break;
            case "cnt-p":
                CMS.Admin.ContentFilePanel cntPanel = new CMS.Admin.ContentFilePanel();
                phControl.Controls.Add(cntPanel);
                ltlTitle.Text = Resources.CMS.ContentAdd;
                btnBack.Visible = true;
                break;
            case "lnk-g":
                CMS.Admin.LinkGrid lnkGrid = new CMS.Admin.LinkGrid();
                phControl.Controls.Add(lnkGrid);
                ltlTitle.Text = Resources.CMS.Links;
                btnBack.Visible = false;
                break;
            case "lnk-p":
                CMS.Admin.LinkPanel lnkPanel = new CMS.Admin.LinkPanel();
                phControl.Controls.Add(lnkPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.LinkAdd : Resources.CMS.LinkEdit);
                btnBack.Visible = true;
                break;
            case "lnkcat-g":
                CMS.Admin.LinkCategoryGrid lnkcatGrid = new CMS.Admin.LinkCategoryGrid();
                phControl.Controls.Add(lnkcatGrid);
                ltlTitle.Text = Resources.CMS.LinkCategories;
                btnBack.Visible = false;
                break;
            case "lnkcat-p":
                CMS.Admin.LinkCategoryPanel lnkcatPanel = new CMS.Admin.LinkCategoryPanel();
                phControl.Controls.Add(lnkcatPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.LinkCategoryAdd : Resources.CMS.LinkCategoryEdit);
                btnBack.Visible = true;
                break;
            case "job-g":
                CMS.Admin.JobGrid jobGrid = new CMS.Admin.JobGrid();
                phControl.Controls.Add(jobGrid);
                ltlTitle.Text = Resources.CMS.Jobs;
                btnBack.Visible = false;
                break;
            case "job-p":
                CMS.Admin.JobPanel jobPanel = new CMS.Admin.JobPanel();
                jobPanel.CompanyID = new Guid("f1b41216-ecdc-4a90-8bfa-f85b8eb5d0b3");
                phControl.Controls.Add(jobPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.JobAdd : Resources.CMS.JobEdit);
                btnBack.Visible = true;
                break;
            case "cmp-g":
                CMS.Admin.CompanyGrid cmpGrid = new CMS.Admin.CompanyGrid();
                phControl.Controls.Add(cmpGrid);
                ltlTitle.Text = Resources.CMS.Companies;
                btnBack.Visible = false;
                break;
            case "cmp-p":
                CMS.Admin.CompanyPanel cmpPanel = new CMS.Admin.CompanyPanel();
                phControl.Controls.Add(cmpPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.CompanyAdd : Resources.CMS.CompanyEdit);
                btnBack.Visible = true;
                break;
            case "cmpcat-g":
                CMS.Admin.CompanyFieldGrid cmpcatGrid = new CMS.Admin.CompanyFieldGrid();
                phControl.Controls.Add(cmpcatGrid);
                ltlTitle.Text = Resources.CMS.CompanyCategories;
                btnBack.Visible = false;
                break;
            case "cmpcat-p":
                CMS.Admin.CompanyFieldPanel cmpcatPanel = new CMS.Admin.CompanyFieldPanel();
                phControl.Controls.Add(cmpcatPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.CompanyCategoryAdd : Resources.CMS.CompanyCategoryEdit);
                btnBack.Visible = true;
                break;
            case "lib-g":
                CMS.Admin.LibraryItemGrid libGrid = new CMS.Admin.LibraryItemGrid();
                phControl.Controls.Add(libGrid);
                ltlTitle.Text = Resources.CMS.Files;
                btnBack.Visible = false;
                break;
            case "lib-p":
                CMS.Admin.LibraryItemPanel libPanel = new CMS.Admin.LibraryItemPanel();
                phControl.Controls.Add(libPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.FileAdd : Resources.CMS.FileEdit);
                btnBack.Visible = true;
                break;
            case "libcat-g":
                CMS.Admin.LibrarySectionGrid libcatGrid = new CMS.Admin.LibrarySectionGrid();
                phControl.Controls.Add(libcatGrid);
                ltlTitle.Text = Resources.CMS.LibraryCategories;
                btnBack.Visible = false;
                break;
            case "libcat-p":
                CMS.Admin.LibrarySectionPanel libcatPanel = new CMS.Admin.LibrarySectionPanel();
                phControl.Controls.Add(libcatPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.LibraryCategoryAdd : Resources.CMS.LibraryCategoryEdit);
                btnBack.Visible = true;
                break;
            case "que-g":
                CMS.Admin.QAGrid queGrid = new CMS.Admin.QAGrid();
                phControl.Controls.Add(queGrid);
                ltlTitle.Text = Resources.CMS.Questions;
                btnBack.Visible = false;
                break;
            case "que-p":
                CMS.Admin.QAPanel quePanel = new CMS.Admin.QAPanel();
                phControl.Controls.Add(quePanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.QuestionAdd : Resources.CMS.QuestionEdit);
                btnBack.Visible = true;
                break;
            case "quecat-g":
                CMS.Admin.QuestionTypeGrid quecatGrid = new CMS.Admin.QuestionTypeGrid();
                phControl.Controls.Add(quecatGrid);
                ltlTitle.Text = Resources.CMS.QuestionCategories;
                btnBack.Visible = false;
                break;
            case "quecat-p":
                CMS.Admin.QuestionTypePanel quecatPanel = new CMS.Admin.QuestionTypePanel();
                phControl.Controls.Add(quecatPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.QuestionCategoryAdd : Resources.CMS.QuestionCategoryEdit);
                btnBack.Visible = true;
                break;
            case "pol-g":
                CMS.Admin.PollGrid polGrid = new CMS.Admin.PollGrid();
                phControl.Controls.Add(polGrid);
                ltlTitle.Text = Resources.CMS.Polls;
                btnBack.Visible = false;
                break;
            case "pol-p":
                CMS.Admin.PollPanel polPanel = new CMS.Admin.PollPanel();
                phControl.Controls.Add(polPanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.PollAdd : Resources.CMS.PollEdit);
                btnBack.Visible = true;
                break;
            case "gue-g":
                CMS.Admin.GuestBookGrid gueGrid = new CMS.Admin.GuestBookGrid();
                phControl.Controls.Add(gueGrid);
                ltlTitle.Text = Resources.CMS.GuestbookEntries;
                btnBack.Visible = false;
                break;
            case "gue-p":
                CMS.Admin.GuestBookPanel guePanel = new CMS.Admin.GuestBookPanel();
                phControl.Controls.Add(guePanel);
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.GuestbookEntryAdd : Resources.CMS.GuestbookEntryEdit);
                btnBack.Visible = true;
                break;
            case "newsub-g":
                CMS.Admin.NewsletterSubscribersGrid newsubGrid = new CMS.Admin.NewsletterSubscribersGrid();
                phControl.Controls.Add(newsubGrid);
                ltlTitle.Text = Resources.CMS.NewsletterSubscribers;
                btnBack.Visible = false;
                break;
            case "face-p":
                CMS.Admin.FacebookLoginPanel facePanel = new CMS.Admin.FacebookLoginPanel();
                phControl.Controls.Add(facePanel);
                ltlTitle.Text = Resources.CMS.FacebookLogin;
                btnBack.Visible = false;
                break;
            case "dir-g":
                CMS.Admin.DirectoryOutletGrid outGrid = new CMS.Admin.DirectoryOutletGrid();
                phControl.Controls.Add(outGrid);
                ltlTitle.Text = Resources.CMS.Outlets;
                btnBack.Visible = false;
                break;
            case "dir-p":
                CMS.Admin.DirectoryOutletPanel outPanel = new CMS.Admin.DirectoryOutletPanel();
                phControl.Controls.Add(outPanel);
                outPanel.CityLookupID = 2;
                ltlTitle.Text = (string.IsNullOrEmpty(Request["id"]) ? Resources.CMS.OutletAdd : Resources.CMS.OutletEdit);
                btnBack.Visible = true;
                break;
            default:
                Literal ltlWelcome = new Literal();
                ltlWelcome.Text = string.Format("<h1>{0}</h1>{1}", Resources.CMS.CMSTitle, Resources.CMS.CMSWelcome);
                phControl.Controls.Add(ltlWelcome);
                btnBack.Visible = false;
                break;
        }
    }
}