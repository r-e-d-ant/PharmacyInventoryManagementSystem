using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace PharmacyInventoryManagementSystem.Pages.Medicines
{
	public class ManageMedicineTypeModel : PageModel
    {
        public MedicinesListTypes type = new MedicinesListTypes();
        public List<MedicinesListTypes> listTypes = new List<MedicinesListTypes>();

        string connstring = @"server=localhost;user=root;password=mugishathi;database=pharmacyInventoryMgtSystem_db";

        public string errors = "";
        public string success = "";

        public void OnPost()
        {
            type.type_name = Request.Form["type_name"];

            using (MySqlConnection conn = new MySqlConnection(connstring))
            {
                // Insert query
                String query = "INSERT INTO tbl_medicine_type (type_name, created_by) VALUES(@type_name, @creator_id)";

                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@type_name", type.type_name);
                    cmd.Parameters.AddWithValue("@creator_id", 2);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        Response.Redirect("/Medicines/ManageMedicineType");
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
                String qry = "SELECT type_id, tbl_medicine_type.type_name, tbl_medicine_type.created_date, tbl_user.full_name FROM tbl_medicine_type JOIN tbl_user ON tbl_user.user_id = tbl_medicine_type.created_by";
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MedicinesListTypes typeToSee = new MedicinesListTypes();

                            typeToSee.type_id = "" + reader.GetString(0);
                            typeToSee.type_name = reader.GetString(1);
                            typeToSee.created_date = reader.GetString(2);
                            typeToSee.creator = reader.GetString(3);

                            listTypes.Add(typeToSee);
                        }
                    }
                }
            }
        }

    }

    public class MedicinesListTypes
    {
        public string type_id;
        public string type_name;
        public string created_date;
        public string creator;
    }
}
