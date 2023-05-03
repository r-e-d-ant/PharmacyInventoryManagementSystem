using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace PharmacyInventoryManagementSystem.Pages.Medecines
{
    
    public class ManageMedecinesModel : PageModel
    {
        public List<MedicinesData> listMedicines = new List<MedicinesData>();
        string connstring = @"server=localhost;user=root;password=mugishathi;database=pharmacyInventoryMgtSystem_db";

        public void OnGet()
        {
            MySqlConnection conn = null;

            using (conn = new MySqlConnection(connstring))
            {
                conn.Open();
                String qry = "SELECT medicine_id, batch_no, name, specification, measurement, price, retail_price, expiry_date FROM tbl_medicine";
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MedicinesData medicine = new MedicinesData();

                            medicine.medicineId = ""+ reader.GetString(0);
;                           medicine.batchNo = reader.GetString(1);
                            medicine.name = reader.GetString(2);
                            medicine.specification = reader.GetString(3);
                            medicine.measurement = ""+ reader.GetString(4);
                            medicine.price = "" + reader.GetString(5);
                            medicine.retailPrice = "" + reader.GetString(6);
                            medicine.expiryDate = reader.GetString(7);

                            listMedicines.Add(medicine);
                        }
                    }
                }
            }
        }
    }

    public class MedicinesData
    {
        public string medicineId;
        public string batchNo;
        public string name;
        public string specification;
        public string measurement;
        public string price;
        public string retailPrice;
        public string expiryDate;
    }
}
