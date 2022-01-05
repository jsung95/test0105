using EasyProject.Model;
using EasyProject.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace EasyProject.Dao
{
    public class CategoryDao : CommonDBConn
    {
        public List<CategoryModel> GetCategories()
        {
            List<CategoryModel> list = new List<CategoryModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT category_name FROM CATEGORY";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            CategoryModel dto = new CategoryModel()
                            {
                                Category_name = reader.GetString(0)
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//conn

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//GetCategoris()

        public int GetCategoryID(string category_name)
        {
            int category_id = 0;

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT category_id FROM CATEGORY WHERE category_name = :category_name";

                        cmd.Parameters.Add(new OracleParameter("category_name", category_name));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            category_id = reader.GetInt32(0);
                        }//while

                    }//using(cmd)

                }//conn

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return category_id;
        }//GetCategoryID

        public List<CategoryModel> GetCategoriesvalues()
        {
            List<CategoryModel> list = new List<CategoryModel>();

            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * FROM CATEGORY";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            CategoryModel dto = new CategoryModel()
                            {
                                Category_id = reader.GetInt32(0),
                                Category_name = reader.GetString(1)
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//conn

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//GetCategoris()




        public bool IsExistsCategory(string Category_name)
        {
            bool result = false;
            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "SELECT * FROM category WHERE category_name = :category_name ";

                        cmd.Parameters.Add(new OracleParameter("category_name", Category_name));

                        OracleDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            result = true;
                        }
                        else
                        {
                            result = false;
                        }//if-else
                    }//using(cmd)
                }//using(conn)

            }//try
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return result;

        }//IsExistsCategory


        public void AddCategory(string Category_name)
        {
            try
            {
                OracleConnection conn = new OracleConnection(connectionString);
                OracleCommand cmd = new OracleCommand();

                using (conn)
                {
                    conn.Open();

                    using (cmd)
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "INSERT INTO CATEGORY (category_name) " +
                                          "VALUES (:category_name) ";

                        cmd.Parameters.Add(new OracleParameter("category_name", Category_name));

                        cmd.ExecuteNonQuery();

                    }//using(cmd)
                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch
        }//AddCategory


    }//class

}//namespace
