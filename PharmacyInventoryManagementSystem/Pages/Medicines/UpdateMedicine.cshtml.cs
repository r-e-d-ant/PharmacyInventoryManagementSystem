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
	public class UpdateMedicineModel : PageModel
    {
        // Request.Query["id"]
        public MedicinesDataToInsertUp medicine = new MedicinesDataToInsertUp();
        public List<MedicinesDataToInsertUp> listMeds = new List<MedicinesDataToInsertUp>();

        public List<categoriesInfosUp> categoriesList = new List<categoriesInfosUp>();
        public List<typesInfosUp> typessList = new List<typesInfosUp>();
        public List<suppliersInfosUp> suppliersList = new List<suppliersInfosUp>();

        string connstring = @"server=localhost;user=root;password=mugishathi;database=pharmacyInventoryMgtSystem_db";

        public string errors = "";
        public string success = "";

        public void getTypesCategoryAndSuppliers()
        {
            String batch_no = Request.Query["id"];
            Console.WriteLine("Batch No: " + batch_no);
            MySqlConnection conn = null;

            using (conn = new MySqlConnection(connstring))
            {
                conn.Open();
                String categoryQuery = "SELECT category_id, category_name FROM tbl_medicine_category";
                String typesQuery = "SELECT type_id, type_name FROM tbl_medicine_type";
                String suppliersQuery = "SELECT supplier_id, supplier_name FROM tbl_supplier";

                // categories
                using (MySqlCommand cmd = new MySqlCommand(categoryQuery, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categoriesInfosUp categories = new categoriesInfosUp();

                            categories.category_id = "" + reader.GetString(0);
                            categories.category_name = reader.GetString(1);

                            categoriesList.Add(categories);
                        }
                    }
                }
                // types
                using (MySqlCommand cmd = new MySqlCommand(typesQuery, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            typesInfosUp types = new typesInfosUp();

                            types.type_id = "" + reader.GetString(0);
                            types.type_name = reader.GetString(1);

                            typessList.Add(types);
                        }
                    }
                }
                // suppliers
                using (MySqlCommand cmd = new MySqlCommand(suppliersQuery, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suppliersInfosUp suppliers = new suppliersInfosUp();

                            suppliers.supplier_id = "" + reader.GetString(0);
                            suppliers.supplier_name = reader.GetString(1);

                            suppliersList.Add(suppliers);
                        }
                    }
                }
            }
        }


        public void OnGet()
        {
            String batch_no = Request.Query["id"];
            Console.WriteLine("Batch No: " + batch_no);
            getTypesCategoryAndSuppliers();
            string connstring = @"server=localhost;user=root;password=mugishathi;database=pharmacyInventoryMgtSystem_db";

            MySqlConnection conn = null;

            using (conn = new MySqlConnection(connstring))
            {
                using (conn = new MySqlConnection(connstring))
                {
                    conn.Open();
                    String qry = "SELECT batch_no, name, specification, category_id, type_id, supplier_id, measurement, remarks, price, retail_price, quantity_on_hand, expiry_date, created_by FROM tbl_medicine WHERE batch_no=@batch_no";
                    using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                    {
                        cmd.Parameters.AddWithValue("@batch_no", batch_no);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                MedicinesDataToInsertUp medicine = new MedicinesDataToInsertUp();
                                ViewData["batchNo"] = reader.GetString(0);
                                medicine.batchNo = reader.GetString(0);
                                @ViewData["medName"] = reader.GetString(1);
                                medicine.name = reader.GetString(1);
                                @ViewData["specification"]= reader.GetString(2);
                                medicine.specification = reader.GetString(2);
                                @ViewData["medCateg"] = reader.GetString(3);
                                medicine.category_id = reader.GetString(3);
                                @ViewData["medType"]= reader.GetString(4);
                                medicine.type_id = reader.GetString(4);
                                @ViewData["supplier"]= reader.GetString(5);
                                medicine.supplier_id = reader.GetString(5);
                                @ViewData["measurement"]= "" + reader.GetString(6);
                                medicine.measurement = "" + reader.GetString(6);
                                @ViewData["remarks"]= "" + reader.GetString(7);
                                medicine.remarks = "" + reader.GetString(7);
                                @ViewData["price"]= "" + reader.GetString(8);
                                medicine.price = "" + reader.GetString(8);
                                @ViewData["retailPrice"]= "" + reader.GetString(9);
                                medicine.retailPrice = "" + reader.GetString(9);
                                @ViewData["qtyOnHand"]= "" + reader.GetString(10);
                                medicine.quantity_on_hand = "" + reader.GetString(10);
                                @ViewData["expiryDate"] = "" + reader.GetString(11);
                                medicine.expiry_date = "" + reader.GetString(11);
                                medicine.created_by = "" + reader.GetString(12);

                                listMeds.Add(medicine);
                            }
                        }
                    }
                }
            }    
        }


        public void OnPost()
        {
            medicine.batchNo = Request.Form["batchNo"];
            medicine.name = Request.Form["medName"];
            medicine.specification = Request.Form["specification"];
            medicine.category_id = Request.Form["medCateg"];
            medicine.type_id = Request.Form["medType"];
            medicine.supplier_id = Request.Form["supplier"];
            medicine.measurement = Request.Form["measurement"];
            medicine.remarks = Request.Form["remarks"];
            medicine.price = Request.Form["price"];
            medicine.retailPrice = Request.Form["retailPrice"];
            medicine.quantity_on_hand = Request.Form["qtyOnHand"];
            medicine.expiry_date = Request.Form["expiryDate"];
            medicine.created_by = "2";

            using (MySqlConnection conn = new MySqlConnection(connstring))
            {
                // Insert query
                String query = "UPDATE tbl_medicine SET name=@name, specification=@specification, category_id=@category_id, type_id=@type_id, supplier_id=@supplier_id, measurement=@measurement, remarks=@remarks, price=@price, retail_price=@retail_price, quantity_on_hand=@quantity_on_hand, expiry_date=@expiry_date, created_by=@created_by WHERE batch_no=@batch_no";
                    //"VALUES(@name, @specification, @category_id, @type_id, @supplier_id, @measurement, @remarks, @price, @retail_price, @quantity_on_hand, @expiry_date, @created_by)";

                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@batch_no", medicine.batchNo);
                    cmd.Parameters.AddWithValue("@name", medicine.name);
                    cmd.Parameters.AddWithValue("@specification", medicine.specification);
                    cmd.Parameters.AddWithValue("@category_id", medicine.category_id);
                    cmd.Parameters.AddWithValue("@type_id", medicine.type_id);
                    cmd.Parameters.AddWithValue("@supplier_id", medicine.supplier_id);
                    cmd.Parameters.AddWithValue("@measurement", medicine.measurement);
                    cmd.Parameters.AddWithValue("@remarks", medicine.remarks);
                    cmd.Parameters.AddWithValue("@price", medicine.price);
                    cmd.Parameters.AddWithValue("@retail_price", medicine.retailPrice);
                    cmd.Parameters.AddWithValue("@quantity_on_hand", medicine.quantity_on_hand);
                    cmd.Parameters.AddWithValue("@expiry_date", medicine.expiry_date);
                    cmd.Parameters.AddWithValue("@created_by", medicine.created_by);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        Response.Redirect("/Medicines/ManageMedicines/");
                    }
                }
            }
        }
    }

    public class categoriesInfosUp
    {
        public string category_id;
        public string category_name;
    }
    public class typesInfosUp
    {
        public string type_id;
        public string type_name;
    }
    public class suppliersInfosUp
    {
        public string supplier_id;
        public string supplier_name;
    }

    public class MedicinesDataToInsertUp
    {
        public string batchNo;
        public string name;
        public string specification;
        public string category_id;
        public string type_id;
        public string supplier_id;
        public string measurement;
        public string remarks;
        public string price;
        public string retailPrice;
        public string quantity_on_hand;
        public string expiry_date;
        public string created_by;
    }
}
