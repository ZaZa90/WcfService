using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

namespace WcfService
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        Database database = new Database();
        String str;
        string strcon = "Server=localhost;Database=robocar;Uid=root;Pwd=;SslMode=none;port=3306";
        MySqlConnection con;
        MySqlCommand com;
        MySqlDataReader reader;

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void ReloadData()
        {

            TextBox1.Text = "Not Connected";
            TextBox2.Text = "0.0.0.0";
            if (database.GetIp() != "0.0.0.0")
            {
                TextBox1.Text = "Connected";
                TextBox2.Text = database.GetIp();
            }

            if (database.GetPicture() != null)
            {
                Label4.Text = database.GetPicture().getUrl();
                Label3.Text = database.GetPicture().getQRCode();
                Image1.ImageUrl = database.GetPicture().getUrl();
            }

            TextBox4.Text = printOperation();
            TextBox3.Text = database.getCurrentOperation();
            StatusBox.Text = database.getStatus();

            if (database.needsInit())
            {
                int dim = database.getStorageDim();
                ddlDim.Items.Clear();
                chkList.Items.Clear();
                for (int i = 0; i < dim; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        if (database.getProducts().ContainsKey((char)(((int)'A') + i) + j.ToString()))
                        {
                            ddlDim.Items.Add(new ListItem((char)(((int)'A') + i) + j.ToString(), (char)(((int)'A') + i) + j.ToString()));

                            chkList.Items.Add(new ListItem()
                            {
                                Text = database.getProducts()[(char)(((int)'A') + i) + j.ToString()],
                                Value = (char)(((int)'A') + i) + j.ToString()
                            });
                        }
                    }

                }
                
                database.setInit(false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"]==null) Response.Redirect("LoginForm.aspx");
            Timer1.Tick += Timer1_Tick;
            con = new MySqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
                con.Open();


            str = "Select Position, product from prodotti";
            com = new MySqlCommand(str, con);
            reader = com.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    database.getProducts().Add(reader[0].ToString(), reader[1].ToString());
                }

            }
            catch (Exception ex)
            {
                reader.Close();
                con.Close();
                Console.WriteLine(ex.Message);
            }
            reader.Close();
            con.Close();
            ReloadData();

        }

        protected void Reload_Click(object sender, EventArgs e)
        {
            //Reload
            ReloadData();
        }

        protected void Reset_Click(object sender, EventArgs e)
        {
            //Reset
            database.removeAllOperation();
        }

        private string printOperation()
        {
            string result = "";
            foreach( String op in database.getOperations()){
                if (result.Equals("")) result += op;
                else result += ", " + op;
            }
            return result;
        }

        protected void Set_Target(object sender, EventArgs e)
        {
            if (!database.getLock())
            {
                database.setLock(true);
                List<string> selectedValues = chkList.Items.Cast<ListItem>()
                    .Where(li => li.Selected)
                    .Select(li => li.Value)
                    .ToList();
                database.SetTargetAndChecks(ddlDim.SelectedValue, selectedValues);
            }

        }
    }
}