using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WcfService
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        Database database = new Database();

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
                        ddlDim.Items.Add(new ListItem((char)(((int)'A') + i) + j.ToString(), (char)(((int)'A') + i) + j.ToString()));

                        chkList.Items.Add(new ListItem()
                        {
                            Text = (char)(((int)'A') + i) + j.ToString(),
                            Value = (char)(((int)'A') + i) + j.ToString()
                        });
                    }
                }
                database.setInit(false);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"]==null) Response.Redirect("LoginForm.aspx");
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Redirect("Products.aspx");
        }
    }
}