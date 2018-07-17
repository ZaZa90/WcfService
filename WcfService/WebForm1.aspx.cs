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
            TextBox3.Text = database.getCurrentOperation().ToString();
            StatusBox.Text = database.getStatus();
        }

        protected void Set_Grid_Dim(object sender, EventArgs e)
        {
            ddlDim.Items.Clear();
            chkList.Items.Clear();
            int dim = 0;
            if (!Int32.TryParse(TextBox5.Text, out dim))
            {
                //TODO return error in console
            }
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    ddlDim.Items.Add(new ListItem((char)(((int)'A') + i) + j.ToString(), (char)(((int)'A') + i) + j.ToString()));

                    chkList.Items.Add(new ListItem() {
                        Text = (char)(((int)'A') + i) + j.ToString(),
                        Value = (char)(((int)'A') + i) + j.ToString()
                    });
                }
            }

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
                ReloadData();
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["username"]==null) Response.Redirect("LoginForm.aspx");
            ReloadData();

        }

        protected void Reload_Click(object sender, EventArgs e)
        {
            //Reload
            ReloadData();
        }

        private string printOperation()
        {
            string result = "";
            foreach( Operation op in database.getOperations()){
                result += op + ",";
            }
            return result;
        }



        protected void Reset_Click(object sender, EventArgs e)
        {
            //Reset
            database.removeAllOperation();
            ReloadData();
        }

        protected void Picture_Click(object sender, EventArgs e)
        {
            //Take Picture
            if (!database.getLock())
                database.addOperation(Operation.PICTURE);
            ReloadData();
        }


        protected void Left_Click(object sender, EventArgs e)
        {
            //Manual Left
            if (!database.getLock())
                database.addOperation(Operation.LEFT2);
            ReloadData();
        }

        protected void Forward_Click(object sender, EventArgs e)
        {
            //Manual Forward
            if (!database.getLock())
                database.addOperation(Operation.FORWARD2);
            ReloadData();
        }

        protected void Right_Click(object sender, EventArgs e)
        {
            //Manual Right
            if (!database.getLock())
                database.addOperation(Operation.RIGHT2);
            ReloadData();
        }

        protected void Backward_Click(object sender, EventArgs e)
        {
            //Manual Backward
            if (!database.getLock())
                database.addOperation(Operation.BACKWARD2);
            ReloadData();
        }

        protected void Stop_Click(object sender, EventArgs e)
        {
            //Manual Stop
            if (!database.getLock())
                database.addOperation(Operation.STOP);
            ReloadData();
        }




    }
}