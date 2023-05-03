using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace PharmacyInventoryManagementSystem.Pages.Medicines
{
	public class ManageMedicineCategoryModel : PageModel
    {
        public MedicinesCategories category = new MedicinesCategories();
        public List<MedicinesCategories> listCategories = new List<MedicinesCategories>();

        string connstring = @"server=localhost;user=root;password=mugishathi;database=pharmacyInventoryMgtSystem_db";

        public string errors = "";
        public string success = "";

        public void OnPost()
        {
            category.category_name = Request.Form["category_name"];

            using (MySqlConnection conn = new MySqlConnection(connstring))
            {
                // Insert query
                String query = "INSERT INTO tbl_medicine_category (category_name, created_by) VALUES(@category_name, @creator_id)";

                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@category_name", category.category_name);
                    cmd.Parameters.AddWithValue("@creator_id", 2);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        Response.Redirect("/Medicines/ManageMedicineCategory/");
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
                String qry = "SELECT category_id, category_name, created_date, full_name FROM tbl_medicine_category JOIN tbl_user ON tbl_user.user_id = tbl_medicine_category.created_by";
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MedicinesCategories categoryToSee = new MedicinesCategories();

                            categoryToSee.category_id = "" + reader.GetString(0);
                            categoryToSee.category_name = reader.GetString(1);
                            categoryToSee.created_date = reader.GetString(2);
                            categoryToSee.creator = reader.GetString(3);

                            listCategories.Add(categoryToSee);
                        }
                    }
                }
            }
        }

    }

    public class MedicinesCategories
    {
        public string category_id;
        public string category_name;
        public string created_date;
        public string creator;
    }
}
