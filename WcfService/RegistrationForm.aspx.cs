using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace WcfService
{
    public partial class RegistrationForm : System.Web.UI.Page
    {
        string str;
        string strcon = "Server=localhost;Database=Robocar;Uid=root;Pwd=;SslMode=none;port=3306";
        protected void Button1_Click(object Sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(strcon);
            MySqlCommand com;
            MySqlDataReader reader;
            str = "INSERT INTO login VALUES(NULL,@Name,@Pass)";
            com = new MySqlCommand(str, con);
            com.Parameters.AddWithValue("Name", TextBox1.Text);
            com.Parameters.AddWithValue("Pass", TextBox2.Text);
            try
            {
                con.Open();

                reader = com.ExecuteReader();

                Response.Redirect("LoginForm.aspx");
            }
            catch (Exception ex)
            {
                lb1.Text = ex.Message;
            }
        }
    }
}