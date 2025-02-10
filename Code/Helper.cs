using CMS.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for Helper
/// </summary>
public static class Helper
{
    public static void SendEmail(string from, string to, string subject, string body)
    {

        try
        {
            MailMessage mailMessage = new MailMessage(from, to, subject, string.Format(Resources.Strings.EmailTemplate, body, subject));
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;
            // Create the SMTP client using settings from web.config
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = false; // Make sure to set this to 'false'
            smtpClient.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["FromEmail"], ConfigurationManager.AppSettings["pass"]);

            // Send the email
            smtpClient.Send(mailMessage);
            Console.WriteLine("Email sent successfully!");
        }
        catch (Exception ex)
        {

        }


    }
    public static void SaveImage(string[] filenames, string IgnoreNumbers, string pFileName, string pImagesServerPath, string pPostName, int pWidth, int pHeight, CMS.Helpers.ImageHelper.Anchor pAnchor, CMS.Helpers.ImageHelper.PicResizeMode pResizeMode, ImageFormat pImageFormat, string pBaseImage, int pPadding)
    {
        int i = 1;
        foreach (string filename in filenames)
        {
            if (string.IsNullOrEmpty(filename)) //to ignore the empty first one
                continue;
            if (!string.IsNullOrEmpty(IgnoreNumbers))
                while (IgnoreNumbers.Contains(i.ToString()))
                    i++;
            ImageHelper.SaveImage(pImagesServerPath + pFileName + i + "_" + pPostName, HttpContext.Current.Server.MapPath("~/tempfiles/" + HttpContext.Current.Session["tempfolder"] + "/" + filename), "", pWidth, pHeight, pAnchor, pResizeMode, pImageFormat, pBaseImage, pPadding);
            i++;
        }
    }

}