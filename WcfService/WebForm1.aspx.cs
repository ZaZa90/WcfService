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
                // TODO
                //Curr_pos.SelectedValue = (Label3.Text != "Error") ? Label3.Text : database.getCurrPos();
                //Curr_dir.SelectedValue = database.getDir();
                Image1.ImageUrl = database.GetPicture().getUrl();
            }

            TextBox4.Text = printOperation();
            TextBox3.Text = database.getCurrentOperation();
            StatusBox.Text = database.getStatus();

            /*if (database.needsInit())
            {                
                database.setInit(false);
            }*/
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"]==null) Response.Redirect("LoginForm.aspx");
            string strcon = "Server=localhost;Database=robocar;Uid=root;Pwd=;SslMode=none;port=3306";

            con = new MySqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
                con.Open();


            str = "Select Position, product from prodotti";
            com = new MySqlCommand(str, con);
            reader = com.ExecuteReader();
            try
            {
                int dim = database.getStorageDim();
                if (!Page.IsPostBack)
                {
                    ddlDim.Items.Clear();
                    chkList.Items.Clear();
                    Curr_pos.Items.Clear();

                    for (int i = 0; i < dim; i++)
                    {
                        for (int j = 0; j < dim; j++)
                        {
                            Curr_pos.Items.Add(new ListItem((char)(((int)'A') + i) + j.ToString(), (char)(((int)'A') + i) + j.ToString()));
                            ddlDim.Items.Add(new ListItem((char)(((int)'A') + i) + j.ToString(), (char)(((int)'A') + i) + j.ToString()));

                        }

                    }

                    while (reader.Read())
                    {
                        if(!database.getProducts().ContainsKey(reader[0].ToString()))
                            database.getProducts().Add(reader[0].ToString(), reader[1].ToString());
                        chkList.Items.Add(new ListItem()
                        {
                            Text = reader[1].ToString(),
                            Value = reader[0].ToString()
                        });
                    }
                }

            }
            catch (System.ArgumentException ex)
            {
                Label5.Text = ex.StackTrace;
               
            }
            catch(NullReferenceException ex)
            {
                Label5.Text = ex.StackTrace;

                if (!reader.IsClosed)
                    reader.Close();
            }
            if(!reader.IsClosed)
                reader.Close();
            if (con.State == ConnectionState.Open)
                con.Close();
            Timer1.Tick += Timer1_Tick;
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
            database.setDir(Curr_dir.SelectedValue);
            database.setPosition(Curr_pos.SelectedValue);
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