using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace PharmacyInventoryManagementSystem.Pages.Medicines
{
	public class SellMedicineModel : PageModel
    {
        public List<MedecinesList> listMedicines = new List<MedecinesList>();
        public MedecinesList med = new MedecinesList();

        public ListOfSolds sold = new ListOfSolds();
        public List<ListOfSolds> medecineSolds = new List<ListOfSolds>();

        string connstring = @"server=localhost;user=root;password=mugishathi;database=pharmacyInventoryMgtSystem_db";

        public void OnGet()
        {
            MySqlConnection conn = null;

            // get medecines informations
            using (conn = new MySqlConnection(connstring))
            {
                conn.Open();
                String qry = "SELECT batch_no, measurement, retail_price, quantity_on_hand FROM tbl_medicine";
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            MedecinesList medicine = new MedecinesList();

                            medicine.batch_no = reader.GetString(0);
                            medicine.measurement = "" + reader.GetString(1);
                            medicine.retail_price = "" + reader.GetString(2);
                            medicine.quantity_on_hand = "" + reader.GetString(3);

                            listMedicines.Add(medicine);
                        }
                    }
                }
            }

            // get sold medecines information
            using (conn = new MySqlConnection(connstring))
            {
                conn.Open();
                String qry = "SELECT sold_id, tbl_sold.batch_no_fk, tbl_medicine.name, tbl_sold.amount, (tbl_medicine.retail_price * tbl_sold.amount) as earned, tbl_sold.created_by FROM tbl_sold JOIN tbl_medicine ON tbl_medicine.batch_no = tbl_sold.batch_no_fk";
                using (MySqlCommand cmd = new MySqlCommand(qry, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListOfSolds sold = new ListOfSolds();

                            sold.sold_id = "" + reader.GetString(0);
                            sold.batch_no = reader.GetString(1);
                            sold.medecine_name = reader.GetString(2);
                            sold.amount = "" + reader.GetString(3);
                            sold.earned = "" + reader.GetString(4);
                            sold.creator = reader.GetString(5);

                            medecineSolds.Add(sold);
                        }
                    }
                }
            }
        }

        public void OnPost()
        {
            sold.batch_no = Request.Form["batch_number"];
            sold.amount = Request.Form["amount"];
            sold.earned = Request.Form["earned"];

            using (MySqlConnection conn = new MySqlConnection(connstring))
            {
                // Insert query
                String query = "INSERT INTO tbl_sold (batch_no_fk, amount, created_by) VALUES(@batch_no_fk, @amount, @creator_id)";

                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    Console.WriteLine("Batch No: ", sold.batch_no);
                    cmd.Parameters.AddWithValue("@batch_no_fk", sold.batch_no);
                    cmd.Parameters.AddWithValue("@amount", sold.amount);
                    cmd.Parameters.AddWithValue("@creator_id", 2);

                    int rowsAffected = cmd.ExecuteNonQuery();
                }

            }
            using (MySqlConnection conn = new MySqlConnection(connstring))
            {
                // update query
                String queryUpdateStock = "UPDATE `pharmacyInventoryMgtSystem_db`.`tbl_medicine` SET `quantity_on_hand` = quantity_on_hand-@amount WHERE `batch_no` = @batch_no";

                conn.Open();
                using (MySqlCommand cmd = new MySqlCommand(queryUpdateStock, conn))
                {
                    cmd.Parameters.AddWithValue("@amount", sold.amount);
                    cmd.Parameters.AddWithValue("@batch_no", sold.batch_no);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        Response.Redirect("/Medicines/SellMedicine/");
                    }
                }

            }
        }
    }

    public class MedecinesList
    {
        public string batch_no;
        public string retail_price;
        public string quantity_on_hand;
        public string measurement;
    }
    public class ListOfSolds
    {
        public string sold_id;
        public string batch_no;
        public string medecine_name;
        public string amount;
        public string earned;
        public string creator;
    }
}
