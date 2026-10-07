using System;
using System.Web;

namespace AcademicLeaveManagement
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Reads 7-day Remember Me cookie
                if (Request.Cookies["AcadixRememberMe"] != null)
                {
                    txtUserId.Text = Request.Cookies["AcadixRememberMe"].Value;
                    chkRememberMe.Checked = true;
                }

                // Reads Theme preference cookie
                if (Request.Cookies["AcadixUserPrefs"] != null)
                {
                    HttpCookie prefs = Request.Cookies["AcadixUserPrefs"];
                    if (ddlTheme.Items.FindByValue(prefs["Theme"]) != null)
                        ddlTheme.SelectedValue = prefs["Theme"];
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            // 1. Session Setup: Quotas and Role
            Session["UserID"] = txtUserId.Text.Trim();
            Session["UserName"] = txtName.Text.Trim();
            Session["Role"] = ddlRole.SelectedValue;
            Session["CL_Balance"] = 8;
            Session["ML_Balance"] = 10;
            Session["OD_Balance"] = 5;

            // 2. Cookie: Remember Me (7-day persistence)
            if (chkRememberMe.Checked)
            {
                HttpCookie rememberCookie = new HttpCookie("AcadixRememberMe", txtUserId.Text.Trim())
                {
                    Expires = DateTime.Now.AddDays(7)
                };
                Response.Cookies.Add(rememberCookie);
            }
            else
            {
                // Expire cookie if unchecked
                HttpCookie expireCookie = new HttpCookie("AcadixRememberMe") { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(expireCookie);
            }

            // 3. Cookie: User Theme and Last Access
            HttpCookie userPrefs = new HttpCookie("AcadixUserPrefs");
            userPrefs["Theme"] = ddlTheme.SelectedValue;
            userPrefs["LastLoginTime"] = DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt");
            userPrefs.Expires = DateTime.Now.AddDays(30);
            Response.Cookies.Add(userPrefs);

            Response.Redirect("LeaveDashboard.aspx");
        }
    }
}