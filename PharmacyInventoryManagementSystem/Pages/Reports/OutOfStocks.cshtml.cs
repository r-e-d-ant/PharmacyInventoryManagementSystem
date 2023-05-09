using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using PharmacyInventoryManagementSystem.Pages.Medicines;

namespace PharmacyInventoryManagementSystem.Pages
{
	public class OutOfStocksModel : PageModel
    {

        public outMedicines outs = new outMedicines();
        public List<outMedicines> listOuts = new List<outMedicines>();

        public void OnGet()
        {
            //String id = Request.Query["id"];

            MySqlConnection conn = null;
            string connstring = @"server=localhost;user=root;password=mugishathi;database=pharmacyInventoryMgtSystem_db";

            using (conn = new MySqlConnection(connstring))
            {
                conn.Open();
                String qry = "SELECT stock_out_id, tbl_stock_out.batch_no, tbl_medicine.name, supplier_name FROM tbl_stock_out JOIN tbl_medicine ON tbl_medicine.batch_no = tbl_stock_out.batch_no JOIN tbl_supplier ON tbl_supplier.supplier_id = tbl_medicine.supplier_id WHERE tbl_medicine.quantity_on_hand = 0";
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            outMedicines outsToSee = new outMedicines();

                            outsToSee.stock_out_id = "" + reader.GetString(0);
                            outsToSee.batch_no = reader.GetString(1);
                            outsToSee.name = reader.GetString(2);
                            outsToSee.supplier_name = reader.GetString(3);

                            listOuts.Add(outsToSee);
                        }
                    }
                }
            }
        }

        public class outMedicines
        {
            public string stock_out_id;
            public string batch_no;
            public string name;
            public string supplier_name;
        }
    }
}

