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
    public class CreateMedicineModel : PageModel
    {
        public MedicinesDataToInsert medicine = new MedicinesDataToInsert();

        public List<retrievedRows> retRows = new List<retrievedRows>();

        public List<categoriesInfos> categoriesList = new List<categoriesInfos>();
        public List<typesInfos> typessList = new List<typesInfos>();
        public List<suppliersInfos> suppliersList = new List<suppliersInfos>();

        string connstring = @"server=localhost;user=root;password=mugishathi;database=pharmacyInventoryMgtSystem_db";

        public string errors = "";
        public string success = "";

        public List<List<String>> getResult(string table_name, string column1, string column2)
        {
            MySqlConnection conn = null;

            using (conn = new MySqlConnection(connstring))
            {
                List<List<String>> resultList = new List<List<String>>();

                conn.Open();
                String qry = "SELECT "+column1+", "+column2+" FROM " + table_name + "";

                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            retrievedRows retrRows = new retrievedRows();

                            retrRows.id = "" + reader.GetString(0);
                            retrRows.name = reader.GetString(1);

                            resultList.Add(retrRows);
                        }
                    }
                }
                return resultList;
            }
        }


        public void OnGet()
        {
            List <String> categoriesReader = getResult("tbl_medicine_category", "tbl_medicine_category.category_id", "tbl_medicine_category.category_name");

            categoriesInfos categories = new categoriesInfos();

            categories.category_id = categoriesReader[0];
            categories.category_name = categoriesReader[1];
            categoriesList.Add(categories);

            List<String> typesReader = getResult("tbl_medicine_type", "tbl_medicine_type.type_id", "tbl_medicine_type.type_name");

            typesInfos types = new typesInfos();

            types.type_id = typesReader[0];
            types.type_name = typesReader[1];
            typessList.Add(types);

            List<String> suppliersReader = getResult("tbl_supplier", "tbl_supplier.supplier_id", "tbl_supplier.supplier_name");

            suppliersInfos suppliers = new suppliersInfos();

            suppliers.supplier_id = suppliersReader[0];
            suppliers.supplier_name = suppliersReader[1];
            suppliersList.Add(suppliers);
        }


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
                        Response.Redirect("/Medicines/ManageMedicines/");
                    }
                }
            }
        }
    }

    public class retrievedRows
    {
        public string id;
        public string name;
    }

    public class categoriesInfos
    {
        public string category_id;
        public string category_name;
    }
    public class typesInfos
    {
        public string type_id;
        public string type_name;
    }
    public class suppliersInfos
    {
        public string supplier_id;
        public string supplier_name;
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
