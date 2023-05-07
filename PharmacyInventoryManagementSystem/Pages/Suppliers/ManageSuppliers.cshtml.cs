using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace PharmacyInventoryManagementSystem.Pages.Suppliers
{
	public class ManageSuppliersModel : PageModel
    {
        public MedicinesSuppliers supplier = new MedicinesSuppliers();
        public List<MedicinesSuppliers> listOfSuppliers = new List<MedicinesSuppliers>();

        string connstring = @"server=localhost;user=root;password=mugishathi;database=pharmacyInventoryMgtSystem_db";

        public string errors = "";
        public string success = "";

        public void OnPost()
        {
            supplier.supplier_name = Request.Form["supplier_name"];
            supplier.description = Request.Form["description"];

            using (MySqlConnection conn = new MySqlConnection(connstring))
            {
                // Insert query
                String query = "INSERT INTO tbl_supplier (supplier_name, description, created_by) VALUES(@supplier_name, @description, @created_by)";

                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@supplier_name", supplier.supplier_name);
                    cmd.Parameters.AddWithValue("@description", supplier.description);
                    cmd.Parameters.AddWithValue("@created_by", 3);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        Response.Redirect("/Suppliers/ManageSuppliers/");
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
                String qry = "SELECT supplier_id, supplier_name, description, tbl_supplier.created_date, full_name FROM tbl_supplier JOIN tbl_user ON tbl_user.user_id = tbl_supplier.created_by";
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MedicinesSuppliers categoryToSee = new MedicinesSuppliers();

                            categoryToSee.supplier_id = "" + reader.GetString(0);
                            categoryToSee.supplier_name = reader.GetString(1);
                            categoryToSee.description = reader.GetString(2);
                            categoryToSee.created_date = reader.GetString(3);
                            categoryToSee.creator = reader.GetString(4);

                            listOfSuppliers.Add(categoryToSee);
                        }
                    }
                }
            }
        }

    }

    public class MedicinesSuppliers
    {
        public string supplier_id;
        public string supplier_name;
        public string description;
        public string created_date;
        public string creator;
    }
}
