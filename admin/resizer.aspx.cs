using CMS;
using CMS.Config;
using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class resizer : KensoftPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnresize_Click(object sender, EventArgs e)
    {
        string ImagesPath = @"~\..\content\";
        string imagesServerPath = this.Page.Server.MapPath(ImagesPath);
        string fileName;
        string filePath = null;

        foreach (string filename in Directory.EnumerateFiles(imagesServerPath, "*.jpg",SearchOption.AllDirectories))
        {
            if (filename.Contains("_"))
                continue;
            fileName = filename.Substring(filename.LastIndexOf("\\")+1,filename.LastIndexOf(".")- filename.LastIndexOf("\\")-1);
            string fileFolder = string.Empty;
            if (filename.Contains("\\"))
            {
                fileFolder = filename.Substring(0, filename.LastIndexOf("\\")+1);
            }
            Response.Write(fileName);

            CMSConfigurations config = CMSConfigurations.GetSection();
            foreach (CMSImageInfo imginfo in config.Images)
            {
                ImageHelper.SaveImage(fileFolder + fileName + "_" + (string.IsNullOrEmpty(imginfo.OverrideName) ? imginfo.Name : imginfo.OverrideName), filename, "", imginfo.Width, imginfo.Height, imginfo.Anchor, imginfo.ResizeMode, (imginfo.ImageFormat == "png" ? System.Drawing.Imaging.ImageFormat.Png : System.Drawing.Imaging.ImageFormat.Jpeg),imginfo.BaseImage, imginfo.Padding);
            }
        }
    }
}