using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace PharmacyInventoryManagementSystem.Pages.Medicines
{
    public class CreateMedicineModel : PageModel
    {
        public MedicinesDataToInsert medicine = new MedicinesDataToInsert();

        string connstring = @"server=localhost;user=root;password=mugishathi;database=pharmacyInventoryMgtSystem_db";

        public string errors = "";
        public string success = "";

        public void OnPost()
        {
            medicine.batchNo = Request.Form["batchNo"];
            medicine.name = Request.Form["medName"];
            medicine.specification = Request.Form["specification"];
            medicine.medCateg = Request.Form["medCateg"];
            medicine.medType = Request.Form["medType"];
            medicine.supplier = Request.Form["supplier"];
            medicine.measurement = Request.Form["measurement"];
            medicine.remarks = Request.Form["remarks"];
            medicine.price = Request.Form["price"];
            medicine.retailPrice = Request.Form["retailPrice"];
            medicine.qtyOnHand = Request.Form["qtyOnHand"];
            medicine.expiryDate = Request.Form["expiryDate"];

            using (MySqlConnection conn = new MySqlConnection(connstring))
            {
                // Insert query
                String query = "INSERT INTO tbl_medicine (batch_no, name, specification, category_id, type_id, supplier_id, measurement, remarks, price, retail_price, quantity_on_hand, expiry_date, created_by)" +
                    "VALUES(@batch_no, @name, @specification, @category_id, @type_id, @supplier_id, @measurement, @remarks, @price, @retail_price, @quantity_on_hand, @expiry_date, @created_by)";

                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@batch_no", medicine.batchNo);
                    cmd.Parameters.AddWithValue("@name", medicine.name);
                    cmd.Parameters.AddWithValue("@specification", medicine.specification);
                    cmd.Parameters.AddWithValue("@category_id", medicine.medCateg);
                    cmd.Parameters.AddWithValue("@type_id", medicine.medType);
                    cmd.Parameters.AddWithValue("@supplier_id", medicine.supplier);
                    cmd.Parameters.AddWithValue("@measurement", medicine.measurement);
                    cmd.Parameters.AddWithValue("@remarks", medicine.remarks);
                    cmd.Parameters.AddWithValue("@price", medicine.price);
                    cmd.Parameters.AddWithValue("@retail_price", medicine.retailPrice);
                    cmd.Parameters.AddWithValue("@quantity_on_hand", medicine.qtyOnHand);
                    cmd.Parameters.AddWithValue("@expiry_date", medicine.expiryDate);
                    cmd.Parameters.AddWithValue("@created_by", 2);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        Response.Redirect("/Medicines/ManageMedicines");
                    }
                }
            }
        }
    }

    public class MedicinesDataToInsert
    {
        public string batchNo;
        public string name;
        public string specification;
        public string medCateg;
        public string medType;
        public string supplier;
        public string measurement;
        public string remarks;
        public string price;
        public string retailPrice;
        public string qtyOnHand;
        public string expiryDate;
        public string createdBy;
    }
}
