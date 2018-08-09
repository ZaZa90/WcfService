using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace WcfService
{
    public partial class LoginForm : System.Web.UI.Page
    {
        Database database = new Database();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

            string strcon = "Server=localhost;Database=Robocar;Uid=root;Pwd=;SslMode=none;";

            string str;

            MySqlCommand com;

            MySqlDataReader reader;



            protected void btn_login_Click(object sender, EventArgs e)
            {

                MySqlConnection con = new MySqlConnection(strcon);

                str = "select * from login where Name=@UserName and Pass=@Password";

                com = new MySqlCommand(str, con);

                com.Parameters.AddWithValue("@UserName", TextBox_user_name.Text);

                com.Parameters.AddWithValue("@Password", TextBox_password.Text);


                try
                {
                    con.Open();


                    reader = com.ExecuteReader();

                    if (reader.HasRows)
                    { 
                    Session["username"] = TextBox_user_name.Text;
                        // pagina di controllo
                        Response.Redirect("WebForm1.aspx");

                    }

                    else
                    {

                        lb1.Text = "invalid user name and password";

                    }

                    con.Close();

                }
                catch (Exception ex)
                {
                    lb1.Text = ex.Message;
                }


        }
    }
}