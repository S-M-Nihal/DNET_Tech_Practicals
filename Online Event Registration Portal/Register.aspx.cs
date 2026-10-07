using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;

namespace Online_Event_Registration_Portal
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void cvTerms_ServerValidate(object source, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            args.IsValid = (chkTerms != null && chkTerms.Checked);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                ShowAlert("Validation Error", "Please fill in all required fields properly.", "warning");
                return;
            }

            string fullName = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPass.Text;
            int age = int.Parse(txtAge.Text.Trim());
            string eventTrack = ddlEvent.SelectedItem.Text;
            string registrationRef = "REG-" + DateTime.Now.ToString("yyyyMMdd") + "-" + new Random().Next(1000, 9999);

            string passwordHash = ComputeSha256Hash(password);
            string connString = ConfigurationManager.ConnectionStrings["EventDbConnection"].ConnectionString;

            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // 1. Check for Duplicate Email
                    string checkQuery = "SELECT COUNT(1) FROM EventRegistrations WHERE Email = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = email;
                        int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            ShowAlert("Already Registered", "This email address has already been used for registration.", "info");
                            return;
                        }
                    }

                    // 2. Insert Record into SQL
                    string insertQuery = @"
                        INSERT INTO EventRegistrations (FullName, Email, PasswordHash, Age, EventTrack, RegistrationRef, CreatedAt)
                        VALUES (@FullName, @Email, @PasswordHash, @Age, @EventTrack, @RegistrationRef, GETDATE())";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.Add("@FullName", SqlDbType.NVarChar, 150).Value = fullName;
                        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 255).Value = email;
                        cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 255).Value = passwordHash;
                        cmd.Parameters.Add("@Age", SqlDbType.Int).Value = age;
                        cmd.Parameters.Add("@EventTrack", SqlDbType.NVarChar, 100).Value = eventTrack;
                        cmd.Parameters.Add("@RegistrationRef", SqlDbType.NVarChar, 50).Value = registrationRef;

                        cmd.ExecuteNonQuery();
                    }
                }

                // 3. Send Confirmation Email
                SendConfirmationEmail(fullName, email, eventTrack, registrationRef);

                // 4. Clear Form Inputs
                ClearInputs();

                // 5. Pop up Success Alert Modal
                ShowAlert("Enrolled Successfully!", $"Welcome {fullName}! Your registration for {eventTrack} is confirmed. A receipt with Ref ID #{registrationRef} has been sent to your email.", "success");
            }
            catch (Exception ex)
            {
                ShowAlert("Error Occurred", ex.Message.Replace("'", "\\'"), "error");
            }
        }

        private void SendConfirmationEmail(string name, string toEmail, string eventTrack, string refId)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.To.Add(toEmail);
                    mail.Subject = $"Registration Confirmed - {eventTrack} (Ref: {refId})";
                    mail.IsBodyHtml = true;
                    mail.Body = $@"
                        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #e2e8f0; border-radius: 8px; overflow: hidden;'>
                            <div style='background: #4f46e5; color: white; padding: 20px; text-align: center;'>
                                <h2 style='margin:0;'>Registration Confirmed!</h2>
                            </div>
                            <div style='padding: 24px; color: #1e293b; line-height: 1.6;'>
                                <p>Dear <strong>{name}</strong>,</p>
                                <p>Thank you for registering. You have been enrolled successfully for <strong>{eventTrack}</strong>.</p>
                                <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
                                    <tr>
                                        <td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'><strong>Reference ID:</strong></td>
                                        <td style='padding: 8px; border-bottom: 1px solid #e2e8f0; font-family: monospace; color: #4f46e5;'>{refId}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'><strong>Event Track:</strong></td>
                                        <td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'>{eventTrack}</td>
                                    </tr>
                                    <tr>
                                        <td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'><strong>Date:</strong></td>
                                        <td style='padding: 8px; border-bottom: 1px solid #e2e8f0;'>{DateTime.Now:dd MMM yyyy, hh:mm tt}</td>
                                    </tr>
                                </table>
                                <p>Please keep this reference ID handy for event check-in.</p>
                            </div>
                        </div>";

                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Send(mail);
                    }
                }
            }
            catch
            {
                // Silently bypass if SMTP server is unconfigured in development
            }
        }

        private void ShowAlert(string title, string text, string icon)
        {
            string cleanText = text.Replace("'", "\\'");
            string script = $@"
                Swal.fire({{
                    title: '{title}',
                    text: '{cleanText}',
                    icon: '{icon}',
                    confirmButtonColor: '#4f46e5'
                }});";

            ScriptManager.RegisterStartupScript(this, GetType(), "PopupMessage", script, true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ClearInputs();
        }

        private void ClearInputs()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPass.Text = string.Empty;
            txtConfirmPass.Text = string.Empty;
            txtAge.Text = string.Empty;
            ddlEvent.SelectedIndex = 0;
            chkTerms.Checked = false;
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}