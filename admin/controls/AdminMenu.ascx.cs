using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Admin_controls_AdminMenu : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        EnsureStyleSheetRegistration();
    }

    private void EnsureStyleSheetRegistration()
    {
        // We need <head runat="server"> for this code to work
        if (this.Page.Header == null)
            throw new NotSupportedException("No <head runat=\"server\"> control found in page.");

        // Get the stylesheet resource URL
        var styleSheetUrl = "controls/style/special.css";

        // Check if this stylesheer is already registered
        var alreadyRegistered = this.Page.Header.Controls.OfType<HtmlLink>().Any(x => x.Href.Equals(styleSheetUrl));
        if (alreadyRegistered) return; // no work here

        // If not, register it
        var link = new HtmlLink();
        link.Attributes["rel"] = "stylesheet";
        link.Attributes["type"] = "text/css";
        link.Attributes["href"] = styleSheetUrl;
        this.Page.Header.Controls.Add(link);

        // Get the stylesheet resource URL
        var styleSheetUrl2 = "controls/style/animations.css";

        // Check if this stylesheer is already registered
        var alreadyRegistered2 = this.Page.Header.Controls.OfType<HtmlLink>().Any(x => x.Href.Equals(styleSheetUrl2));
        if (alreadyRegistered2) return; // no work here

        // If not, register it
        var link2 = new HtmlLink();
        link2.Attributes["rel"] = "stylesheet";
        link2.Attributes["type"] = "text/css";
        link2.Attributes["href"] = styleSheetUrl2;
        this.Page.Header.Controls.Add(link2);

        // Get the stylesheet resource URL
        var styleSheetUrl3 = "controls/font-awesome/css/font-awesome.css";

        // Check if this stylesheer is already registered
        var alreadyRegistered3 = this.Page.Header.Controls.OfType<HtmlLink>().Any(x => x.Href.Equals(styleSheetUrl3));
        if (alreadyRegistered3) return; // no work here

        // If not, register it
        var link3 = new HtmlLink();
        link3.Attributes["rel"] = "stylesheet";
        link3.Attributes["type"] = "text/css";
        link3.Attributes["href"] = styleSheetUrl3;
        this.Page.Header.Controls.Add(link3);

        // Get the stylesheet resource URL
        var styleSheetUrl4 = "http://fonts.googleapis.com/css?family=Lato";

        // Check if this stylesheer is already registered
        var alreadyRegistered4 = this.Page.Header.Controls.OfType<HtmlLink>().Any(x => x.Href.Equals(styleSheetUrl4));
        if (alreadyRegistered4) return; // no work here

        // If not, register it
        var link4 = new HtmlLink();
        link4.Attributes["rel"] = "stylesheet";
        link4.Attributes["type"] = "text/css";
        link4.Attributes["href"] = styleSheetUrl4;
        this.Page.Header.Controls.Add(link4);
    }
}