using CMS;
using CMS.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;

/// <summary>
/// Summary description for Orders
/// </summary>
public partial class ReportInventory : KensoftPage
{
    public int PageNumber
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            int num = 1;
            if (!string.IsNullOrEmpty(base.Request["pnum"]))
            {
                num = Convert.ToInt32(base.Request["pnum"]);
            }
            this.PageNumber = num;
            int num2;
            DataSet sqlDS = new DataSet("resulttab");
            using (var conn = new SqlConnection((string)Application["constr"]))
            using (var command = new SqlCommand("[Catalog_ReportsProductsGetPaged]", conn)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                command.Parameters.Add("@pcatid", SqlDbType.BigInt).Value = Request["catID"];
                command.Parameters.Add("@pbrandid", SqlDbType.BigInt).Value = Request["bID"];
                command.Parameters.Add("@pLangID", SqlDbType.BigInt).Value = CurrentLanguage.ID;
                command.Parameters.Add("@pPageNum", SqlDbType.Int).Value = num;
                command.Parameters.Add("@pPageSize", SqlDbType.Int).Value = 50;
                command.Parameters.Add("@pMinStatus", SqlDbType.Int).Value = string.IsNullOrEmpty(Request["minStat"]) ? "2" : Request["minStat"];
                command.Parameters.Add("@pMaxStatus", SqlDbType.Int).Value = string.IsNullOrEmpty(Request["maxStat"]) ? "6" : Request["maxStat"];
                command.Parameters.Add("@pMinDate", SqlDbType.DateTime).Value = string.IsNullOrEmpty(Request["minDate"]) ? DateTime.Now.AddMonths(-1) : DateTime.ParseExact(Request["minDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                command.Parameters.Add("@pMaxDate", SqlDbType.DateTime).Value = string.IsNullOrEmpty(Request["maxDate"]) ? DateTime.Now : DateTime.ParseExact(Request["maxDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                SqlParameter countParam = new SqlParameter("@pcount", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(countParam);
                conn.Open();

                SqlDataAdapter sqlAdapt = new SqlDataAdapter(command);
                sqlAdapt.Fill(sqlDS);
                num2 = Convert.ToInt32(countParam.Value);
                num2 = (int)Math.Ceiling((double)num2 / (double)50);
                conn.Close();

                minDate.SelectedDate = string.IsNullOrEmpty(Request["minDate"]) ? DateTime.Now.AddMonths(-1) : DateTime.ParseExact(Request["minDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                maxDate.SelectedDate = string.IsNullOrEmpty(Request["maxDate"]) ? DateTime.Now : DateTime.ParseExact(Request["maxDate"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            int num3;
            this.repOrders.DataSource = sqlDS;

            this.repOrders.DataBind();
            this.liPrev.Visible = (this.PageNumber > 1);
            this.liNext.Visible = (this.PageNumber < num2);
            List<int> list = new List<int>();
            for (int i = 1; i <= num2; i++)
            {
                list.Add(i);
            }
            this.repPages.DataSource = list;
            this.repPages.DataBind();
        }
    }

    protected void BtnSearch_Click(object sender, EventArgs e)
    {
        Response.Redirect(string.Format("reportinventory?mindate={0}&maxdate={1}",minDate.SelectedDate.HasValue?minDate.SelectedDate.Value.ToString("dd/MM/yyyy"):"", maxDate.SelectedDate.HasValue ? maxDate.SelectedDate.Value.ToString("dd/MM/yyyy") : ""));
    }
}