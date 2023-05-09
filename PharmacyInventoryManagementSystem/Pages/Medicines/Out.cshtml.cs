using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using PharmacyInventoryManagementSystem.Pages.Medecines;

namespace PharmacyInventoryManagementSystem.Pages.Medicines
{
	public class OutModel : PageModel
    {
        public getId get_id = new getId();

        string connstring = @"server=localhost;user=root;password=mugishathi;database=pharmacyInventoryMgtSystem_db";

        public void OnGet()
        {
            String batch_no = Request.Query["id"];
            String supplier_id = Request.Query["supplier_id"];

            MySqlConnection conn = null;

            using (conn = new MySqlConnection(connstring))
            {
                conn.Open();
                String qry = "DELETE FROM tbl_medicine WHERE batch_no=@batch_no";

                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cmd.Parameters.AddWithValue("@batch_no", batch_no);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        
                    }
                }
            }
            using (conn = new MySqlConnection(connstring))
            {
                conn.Open();
                String qry = "INSERT INTO tbl_stock_out(batch_no, supplier_id) VALUES(@batch_no, @supplier_id)";

                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    //Response.Redirect("/Medicines/ManageMedicines/");
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        cmd.Parameters.AddWithValue("@batch_no", batch_no);
                        cmd.Parameters.AddWithValue("@supplier_id", supplier_id);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected >= 1)
                        {
                            Response.Redirect("/Medicines/ManageMedicines/");
                        }
                    }
                }
            }
        }
    }

    public class getId
    {
        public String medId;
    }
}
