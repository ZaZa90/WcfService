using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WcfService
{
    public partial class Config : System.Web.UI.Page
    {
        Database database = new Database();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null) Response.Redirect("LoginForm.aspx");
            ReloadData();
        }

        protected void ReloadData()
        {
            database.setInit(true);

            Label1.Text = database.getConf(0).ToString("F2", CultureInfo.InvariantCulture);
            Label2.Text = database.getConf(1).ToString("F2", CultureInfo.InvariantCulture);
            Label3.Text = database.getConf(2).ToString("F2", CultureInfo.InvariantCulture);
            Label4.Text = database.getConf(3).ToString("F2", CultureInfo.InvariantCulture);
            Label5.Text = database.getStorageDim().ToString();
        }

        protected void UpdateConfig(object sender, EventArgs e)
        {
            float slow = (float.IsNaN(floatBoxConversion(TextBox1.Text))) ? float.Parse(Label1.Text, CultureInfo.InvariantCulture) : float.Parse(TextBox1.Text, CultureInfo.InvariantCulture);
            float high = (float.IsNaN(floatBoxConversion(TextBox2.Text))) ? float.Parse(Label2.Text, CultureInfo.InvariantCulture) : float.Parse(TextBox2.Text, CultureInfo.InvariantCulture);
            float lines = (float.IsNaN(floatBoxConversion(TextBox3.Text))) ? float.Parse(Label3.Text, CultureInfo.InvariantCulture) : float.Parse(TextBox3.Text, CultureInfo.InvariantCulture);
            float turn = (float.IsNaN(floatBoxConversion(TextBox4.Text))) ? float.Parse(Label4.Text, CultureInfo.InvariantCulture) : float.Parse(TextBox4.Text, CultureInfo.InvariantCulture);
            database.setConf(slow, high, lines, turn);
            database.addOperation("C" + slow.ToString("F2", CultureInfo.InvariantCulture) + '/' + high.ToString("F2", CultureInfo.InvariantCulture) + '/' + lines.ToString("F2", CultureInfo.InvariantCulture) + '/' + turn.ToString("F2", CultureInfo.InvariantCulture));
            ReloadData();
        }

        protected float floatBoxConversion(string value)
        {
            float number;

            bool result = float.TryParse(value, out number);
            if (result) return number;
            else return float.NaN;
        }

        protected void Set_Grid_Dim(object sender, EventArgs e)
        {
            int dim = 0;
            if (!Int32.TryParse(TextBox5.Text, out dim))
            {
                //TODO return error in console
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('ERROR PARSING')", true);

            }
            database.setStorageDim(dim);
            ReloadData();
        }

        protected void Picture_Click(object sender, EventArgs e)
        {
            //Take Picture
            if (!database.getLock())
                database.addOperation("PICT");
        }


        protected void Left_Click(object sender, EventArgs e)
        {
            //Manual Left
            if (!database.getLock())
                database.addOperation("L000");
        }

        protected void Forward_Click(object sender, EventArgs e)
        {
            //Manual Forward
            if (!database.getLock())
                database.addOperation("F000");
        }

        protected void Right_Click(object sender, EventArgs e)
        {
            //Manual Right
            if (!database.getLock())
                database.addOperation("R000");
        }

        protected void Backward_Click(object sender, EventArgs e)
        {
            //Manual Backward
            if (!database.getLock())
                database.addOperation("B000");
        }

        protected void Stop_Click(object sender, EventArgs e)
        {
            //Manual Stop
            if (!database.getLock())
                database.addOperation("STOP");
        }
    }


}