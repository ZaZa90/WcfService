using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Data;

namespace WcfService
{

    public partial class Config : System.Web.UI.Page
    {
        Database database = new Database();
        string str;
        string strcon = "Server=localhost;Database=robocar;Uid=root;Pwd=;SslMode=none;port=3306";
        MySqlConnection con;
        MySqlCommand com;
        Dictionary<String, String> products = new Dictionary<string, string>();
        MySqlDataReader reader;

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            ReloadData();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["username"] == null) Response.Redirect("LoginForm.aspx");
            Timer1.Tick += Timer1_Tick;
            ReloadData();
        
            int dim = database.getStorageDim();
            
            for (int i = 0; i < dim; i++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                Table1.Rows.Add(tRow);
                for (int j = 0; j < dim; j++)
                {
                    // Create a new cell and add it to the row.
                    TableCell tCell = new TableCell();
                    TextBox tb = new TextBox();
                    tb.ID = (char)(((int)'A') + i) + j.ToString();
                    if (database.getProducts().ContainsKey((char)(((int)'A') + i) + j.ToString()))
                        tb.Text = database.getProducts()[(char)(((int)'A') + i) + j.ToString()];
                    else
                        tb.Text = "";
                    tCell.Controls.Add(tb);
                    tCell.BorderWidth = 1;
                    tRow.Cells.Add(tCell);
                }

            }
          
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
            else if (dim > 0)
            {
                database.setStorageDim(dim);
            }
            else ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Value not permitted')", true);
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

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Products.aspx");
        }
        protected void Home_Click(object Sender, EventArgs e)
        {
            Response.Redirect("WebForm1.aspx");
        }
        protected void Update_Click(object Sender, EventArgs e)
        {
            //str = "UPDATE prodotti SET PRODUCT=@Product WHERE POSITION=@Position;";
            int dim = database.getStorageDim();
            con = new MySqlConnection(strcon);
            if (con.State == ConnectionState.Closed)
                con.Open();

            for (int i = 0; i < dim; i++)
            {
                // Create new row and add it to the table.
                TableRow tRow = new TableRow();
                Table1.Rows.Add(tRow);

                for (int j = 0; j < dim; j++)
                {
                    TextBox item = (TextBox)Table1.Rows[i].Cells[j].Controls[0];
                    if (com != null)
                        com.Parameters.Clear();
                    if (item.Text.Equals(""))
                    {
                        str = "DELETE FROM prodotti WHERE Position = @Position";
                        database.getProducts().Remove((char)(((int)'A') + i) + j.ToString());
                        com = new MySqlCommand(str, con);

                    }
                    else
                    {

                        str = "INSERT INTO prodotti (position, product) VALUES(@Position, @Product) ON DUPLICATE KEY UPDATE POSITION=@Position , PRODUCT = @Product";
                        if (database.getProducts().ContainsKey((char)(((int)'A') + i) + j.ToString()))
                            database.getProducts()[(char)(((int)'A') + i) + j.ToString()] = item.Text;
                        else
                            database.getProducts().Add((char)(((int)'A') + i) + j.ToString(), item.Text);
                        com = new MySqlCommand(str, con);
                        com.Parameters.AddWithValue("@Product", item.Text);
                    }
                    com.Parameters.AddWithValue("@Position", (char)(((int)'A') + i) + j.ToString());

                    try
                    {
                        reader = com.ExecuteReader();
                    }
                    catch (Exception ex)
                    {
                        Label1.Text = ex.Message;
                        reader.Close();
                    }
                    if(!reader.IsClosed)
                        reader.Close();
                }

            }
        }
    }
}
