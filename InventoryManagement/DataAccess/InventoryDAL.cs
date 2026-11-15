using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Practical8_Inventory.DataAccess
{
    public class InventoryDAL
    {
        // Read the connection string configured in Web.config
        private readonly string cs = ConfigurationManager.ConnectionStrings["InventoryConn"].ConnectionString;

        // READ: Fetch all product records into a disconnected DataTable
        public DataTable GetAllProducts()
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ProductId, ProductName, Category, Quantity, UnitPrice FROM Products ORDER BY ProductId DESC", con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        // CREATE: Insert a new product record using parameterized queries
        public bool InsertProduct(string name, string category, int qty, decimal price)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO Products (ProductName, Category, Quantity, UnitPrice) VALUES (@Name, @Cat, @Qty, @Price)";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = name;
                    cmd.Parameters.Add("@Cat", SqlDbType.NVarChar, 50).Value = category;
                    cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = qty;
                    cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = price;

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // UPDATE: Modify an existing record by its Primary Key (ProductId)
        public bool UpdateProduct(int id, string name, string category, int qty, decimal price)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "UPDATE Products SET ProductName = @Name, Category = @Cat, Quantity = @Qty, UnitPrice = @Price WHERE ProductId = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 100).Value = name;
                    cmd.Parameters.Add("@Cat", SqlDbType.NVarChar, 50).Value = category;
                    cmd.Parameters.Add("@Qty", SqlDbType.Int).Value = qty;
                    cmd.Parameters.Add("@Price", SqlDbType.Decimal).Value = price;

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // DELETE: Remove a product record by its Primary Key (ProductId)
        public bool DeleteProduct(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM Products WHERE ProductId = @Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

                    con.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}