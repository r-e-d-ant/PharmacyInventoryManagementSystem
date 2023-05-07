using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace PharmacyInventoryManagementSystem.Pages.Users
{
	public class ManageUsersModel : PageModel
    {
        public systemUsers user = new systemUsers();
        public List<systemUsers> listOfUsers = new List<systemUsers>();

        string connstring = @"server=localhost;user=root;password=mugishathi;database=pharmacyInventoryMgtSystem_db";

        public string errors = "";
        public string success = "";

        public void OnPost()
        {
            user.full_name = Request.Form["full_name"];
            user.password = Request.Form["password"];
            user.user_type = Request.Form["user_type"];
            user.account_status = Request.Form["account_status"];

            using (MySqlConnection conn = new MySqlConnection(connstring))
            {
                // Insert query
                String query = "INSERT INTO tbl_user (full_name, password, user_type, account_status) VALUES(@full_name, @password, @user_type, @account_status)";

                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@full_name", user.full_name);
                    cmd.Parameters.AddWithValue("@password", user.password);
                    cmd.Parameters.AddWithValue("@user_type", user.user_type);
                    cmd.Parameters.AddWithValue("@account_status", user.account_status);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        Response.Redirect("/Users/ManageUsers/");
                    }
                }
            }
        }
        public void OnGet()
        {
            MySqlConnection conn = null;

            using (conn = new MySqlConnection(connstring))
            {
                conn.Open();
                String qry = "SELECT user_id, full_name, password, user_type, account_status, created_date FROM tbl_user";
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            systemUsers user = new systemUsers();

                            user.user_id = "" + reader.GetString(0);
                            user.full_name = reader.GetString(1);
                            user.password = reader.GetString(2);
                            user.user_type = reader.GetString(3);
                            user.account_status = reader.GetString(4);
                            user.created_date = reader.GetString(5);

                            listOfUsers.Add(user);
                        }
                    }
                }
            }
        }

    }

    public class systemUsers
    {
        public string user_id;
        public string full_name;
        public string password;
        public string user_type;
        public string account_status;
        public string created_date;
    }
}
