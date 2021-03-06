﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace WcfService
{
    public partial class Products : System.Web.UI.Page
    {
        Database database = new Database();
        string str;
        string strcon = "Server=localhost;Database=robocar;Uid=root;Pwd=;SslMode=none;port=3306";
        MySqlConnection con;
        MySqlCommand com;
        MySqlDataReader reader;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["username"] == null) Response.Redirect("LoginForm.aspx");
            con = new MySqlConnection(strcon);
            con.Open();

            /*
            str = "Select Position from prodotti";
            com = new MySqlCommand(str, con);

            

            reader = com.ExecuteReader();
            while (reader.Read()) {
                DropDownList1.Items.Add(new ListItem(reader[0].ToString()));
            }
            reader.Close();*/
            int dim = database.getStorageDim();
            DropDownList1.Items.Clear();
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    DropDownList1.Items.Add(new ListItem((char)(((int)'A') + i) + j.ToString(), (char)(((int)'A') + i) + j.ToString()));
                }
            }
        }
        protected void Home_Click(object Sender, EventArgs e)
        {
            Response.Redirect("WebForm1.aspx");
        }
        protected void Add_Click(object Sender, EventArgs e)
        {
            //str = "UPDATE prodotti SET PRODUCT=@Product WHERE POSITION=@Position;";
            str = "INSERT INTO prodotti (position, product) VALUES(@Position, @Product) ON DUPLICATE KEY UPDATE POSITION=@Position , PRODUCT = @Product";
            com = new MySqlCommand(str, con);

            com.Parameters.AddWithValue("@Position", DropDownList1.Text);

            com.Parameters.AddWithValue("@Product", TextBox2.Text);
            try
            {   
                reader = com.ExecuteReader();

                Response.Redirect("WebForm1.aspx");
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
                reader.Close();
            }
            reader.Close();
        }
        protected void Logout_Click(object Sender, EventArgs e)
        {
            if (Session["username"] == null) Response.Redirect("LoginForm.aspx");
            else
            {
                Session["username"] = null;
                Response.Redirect("LoginForm.aspx");
            }

       }

    }
}