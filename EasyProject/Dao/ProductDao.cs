using EasyProject.Model;
using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;

namespace EasyProject.Dao
{
    public class ProductDao : CommonDBConn, IProductDao //DB연결 Class 및 인터페이스 상속
    {

        public List<ProductShowModel> GetProducts()
        {
            Console.WriteLine("GetProducts !");
            List<ProductShowModel> list = new List<ProductShowModel>();
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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, I.imp_dept_count, P.prod_expire, P.prod_id, I.imp_dept_id " +
                                          "FROM PRODUCT P " +
                                          "INNER JOIN IMP_DEPT I " +
                                          "ON P.prod_id = I.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "INNER JOIN DEPT D " +
                                          "ON I.dept_id = D.dept_id " +
                                          "WHERE D.dept_status != '폐지' " +
                                          "AND D.dept_name = (select dept_name from dept where dept_id = :dept_id) " +
                                          "ORDER BY P.prod_expire, P.prod_name";

                        cmd.Parameters.Add(new OracleParameter("dept_id", App.nurse_dto.Dept_id));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? imp_dept_count = reader.GetInt32(4);
                            DateTime prod_expire = reader.GetDateTime(5);
                            int? prod_id = reader.GetInt32(6);
                            int? imp_dept_id = reader.GetInt32(7);


                            ProductShowModel dto = new ProductShowModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Imp_dept_count = imp_dept_count,
                                Prod_expire = prod_expire,
                                Prod_id = prod_id,
                                Imp_dept_id = imp_dept_id
                            };

                            list.Add(dto);

                        }// while

                    } //using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//GetProduct()


        public bool IsProductDuplicateCheck(ProductModel product_dto)
        {
            bool isDuplicated = false;
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
                        cmd.CommandText = "SELECT prod_code " +
                                          "FROM PRODUCT " +
                                          "WHERE prod_code = :prod_code " +
                                          "AND prod_name = :prod_name " +
                                          "AND category_id = :category_id " +
                                          "AND prod_price = :prod_price " +
                                          "AND prod_expire = TO_DATE(:expire, 'YYYYMMDD') ";
                        cmd.Parameters.Add(new OracleParameter("prod_code", product_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("prod_name", product_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("category_id", product_dto.Category_id));
                        cmd.Parameters.Add(new OracleParameter("prod_price", product_dto.Prod_price));
                        //cmd.Parameters.Add(new OracleParameter("prod_expire", product_dto.Prod_expire));

                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = product_dto.Prod_expire.Month.ToString();
                        if (product_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + product_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = product_dto.Prod_expire.Day.ToString();
                        if (product_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + product_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expire = product_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expire);
                        cmd.Parameters.Add(new OracleParameter("expire", expire));


                        Console.WriteLine(product_dto.Prod_expire);
                        Console.WriteLine(product_dto.Category_id + "카페고리id");
                        OracleDataReader reader = cmd.ExecuteReader();

                        if (reader.Read() == false)
                        {
                            //없는 제품을 추가하려고 하는 경우(access)
                            isDuplicated = false;
                        }
                        else
                        {
                            //이미 입력된 제품을 추가하려고 하는 경우(deny)
                            isDuplicated = true;
                        }
                    }//using(cmd)
                }//using (conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return isDuplicated;

        }//IdCheck


        public bool IsProductDuplicateCheck(ProductModel product_dto, CategoryModel cateogry_dto)//오버로딩
        {
            bool isDuplicated = false;
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
                        cmd.CommandText = "SELECT prod_code " +
                                          "FROM PRODUCT " +
                                          "WHERE prod_code = :prod_code " +
                                          "AND prod_name = :prod_name " +
                                          "AND category_id = (SELECT category_id FROM category WHERE category_name = :category_name) " +
                                          "AND prod_price = :prod_price " +
                                          "AND prod_expire = TO_DATE(:expire, 'YYYYMMDD') ";
                        cmd.Parameters.Add(new OracleParameter("prod_code", product_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("prod_name", product_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("category_name", cateogry_dto.Category_name));
                        cmd.Parameters.Add(new OracleParameter("prod_price", product_dto.Prod_price));
                        //cmd.Parameters.Add(new OracleParameter("prod_expire", product_dto.Prod_expire));

                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = product_dto.Prod_expire.Month.ToString();
                        if (product_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + product_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = product_dto.Prod_expire.Day.ToString();
                        if (product_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + product_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expire = product_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expire);
                        cmd.Parameters.Add(new OracleParameter("expire", expire));



                        Console.WriteLine(product_dto.Prod_expire);
                        Console.WriteLine(product_dto.Category_id+"카페고리id");
                        OracleDataReader reader = cmd.ExecuteReader();

                        if (reader.Read() == false)
                        {
                            //없는 제품을 추가하려고 하는 경우(access)
                            isDuplicated = false;
                        }
                        else
                        {
                            //이미 입력된 제품을 추가하려고 하는 경우(deny)
                            isDuplicated = true;
                        }
                    }//using(cmd)
                }//using (conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return isDuplicated;

        }//IdCheck


        public bool IsProductDuplicateCheck(ProductModel product_dto, string category_name) //오버로딩
        {
            bool isDuplicated = false;
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
                        cmd.CommandText = "SELECT prod_code " +
                                          "FROM PRODUCT " +
                                          "WHERE prod_code = :prod_code " +
                                          "AND prod_name = :prod_name " +
                                          "AND category_id = (SELECT category_id FROM category WHERE category_name = :category_name) " +
                                          "AND prod_price = :prod_price " +
                                          "AND prod_expire = TO_DATE(:expire, 'YYYYMMDD') ";
                        cmd.Parameters.Add(new OracleParameter("prod_code", product_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("prod_name", product_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("category_name", category_name));
                        cmd.Parameters.Add(new OracleParameter("prod_price", product_dto.Prod_price));
                        //cmd.Parameters.Add(new OracleParameter("prod_expire", product_dto.Prod_expire));

                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = product_dto.Prod_expire.Month.ToString();
                        if (product_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + product_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = product_dto.Prod_expire.Day.ToString();
                        if (product_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + product_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expire = product_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expire);
                        cmd.Parameters.Add(new OracleParameter("expire", expire));



                        Console.WriteLine(product_dto.Prod_expire);
                        Console.WriteLine(product_dto.Category_id + "카페고리id");
                        OracleDataReader reader = cmd.ExecuteReader();

                        if (reader.Read() == false)
                        {
                            //없는 제품을 추가하려고 하는 경우(access)
                            isDuplicated = false;
                        }
                        else
                        {
                            //이미 입력된 제품을 추가하려고 하는 경우(deny)
                            isDuplicated = true;
                        }
                    }//using(cmd)
                }//using (conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return isDuplicated;

        }//IdCheck



        public List<ProductShowModel> GetProductsByDept(DeptModel dept_dto)
        {
            List<ProductShowModel> list = new List<ProductShowModel>();
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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, I.imp_dept_count, P.prod_expire, P.prod_id, I.imp_dept_id " +
                                          "FROM PRODUCT P " +
                                          "INNER JOIN IMP_DEPT I " +
                                          "ON P.prod_id = I.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "INNER JOIN DEPT D " +
                                          "ON I.dept_id = D.dept_id " +
                                          "WHERE D.dept_status != '폐지' " +
                                          "AND D.dept_name = :dept_name " +
                                          "AND I.imp_dept_count != 0 " +
                                          "ORDER BY P.prod_expire, P.prod_name";

                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? imp_dept_count = reader.GetInt32(4);
                            DateTime prod_expire = reader.GetDateTime(5);
                            int? prod_id = reader.GetInt32(6);
                            int? imp_dept_id = reader.GetInt32(7);


                            ProductShowModel dto = new ProductShowModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Imp_dept_count = imp_dept_count,
                                Prod_expire = prod_expire,
                                Prod_id = prod_id,
                                Imp_dept_id = imp_dept_id
                            };

                            list.Add(dto);

                        }// while

                    } //using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;
        }//GetProductsByDept



        public List<CategoryModel> GetCategoryModels()
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

                        cmd.CommandText = "SELECT CATEGORY_NAME FROM CATEGORY";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string category_name = reader.GetString(0);
                            CategoryModel category = new CategoryModel()
                            {
                                Category_name = category_name
                            };
                            list.Add(category);

                        }// while

                    } //using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }// GetCategoryModels(string sql)

        public void AddProduct(ProductModel prod_dto, CategoryModel category_dto)
        {
            CategoryDao category_dao = new CategoryDao();
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

                        cmd.CommandText = "INSERT INTO PRODUCT(PROD_CODE, PROD_NAME, PROD_PRICE, PROD_TOTAL, PROD_EXPIRE, CATEGORY_ID) " +
                                          "VALUES(:code, :name, :price, :total, TO_DATE(:expire, 'YYYYMMDD'), (SELECT category_id FROM CATEGORY WHERE category_name = :category_name) )";


                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("code", prod_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("name", prod_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("price", prod_dto.Prod_price));
                        cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = prod_dto.Prod_expire.Month.ToString();
                        if (prod_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + prod_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = prod_dto.Prod_expire.Day.ToString();
                        if (prod_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + prod_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expire = prod_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expire);
                        cmd.Parameters.Add(new OracleParameter("expire", expire));
                        ////////////////////////////////////////////////////////////////////////////

                        cmd.Parameters.Add(new OracleParameter("category_name", category_dto.Category_name));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }//AddProduct

        public void AddProductForExcel(ProductShowModel prod_dto, String categoryName)
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

                        cmd.CommandText = "INSERT INTO PRODUCT(PROD_CODE, PROD_NAME, PROD_PRICE, PROD_TOTAL, PROD_EXPIRE, CATEGORY_ID) " +
                                          "VALUES(:code, :name, :price, :total, TO_DATE(:expire, 'YYYYMMDD'), (SELECT category_id FROM CATEGORY WHERE category_name = :category_name) )";


                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("code", prod_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("name", prod_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("price", prod_dto.Prod_price));
                        cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));


                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = prod_dto.Prod_expire.Month.ToString();
                        if (prod_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + prod_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = prod_dto.Prod_expire.Day.ToString();
                        if (prod_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + prod_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expireText = prod_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expireText);
                        ////////////////////////////////////////////////////////////////////////////
                        

                        /*String expireText = prod_dto.Prod_expire.Year.ToString()+
                            prod_dto.Prod_expire.Month.ToString()+ prod_dto.Prod_expire.Day.ToString();*/

                        cmd.Parameters.Add(new OracleParameter("expire", expireText));
                        cmd.Parameters.Add(new OracleParameter("category_name", categoryName));

                        cmd.ExecuteNonQuery();
                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch
        }

        public void AddProduct(ProductModel prod_dto, string category_name) //오버로딩
        {
            CategoryDao category_dao = new CategoryDao();
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

                        cmd.CommandText = "INSERT INTO PRODUCT(PROD_CODE, PROD_NAME, PROD_PRICE, PROD_TOTAL, PROD_EXPIRE, CATEGORY_ID) " +
                                          "VALUES(:code, :name, :price, :total, TO_DATE(:expire, 'YYYYMMDD'), (SELECT category_id FROM CATEGORY WHERE category_name = :category_name) )";


                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("code", prod_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("name", prod_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("price", prod_dto.Prod_price));
                        cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = prod_dto.Prod_expire.Month.ToString();
                        if (prod_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + prod_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = prod_dto.Prod_expire.Day.ToString();
                        if (prod_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + prod_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expire = prod_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expire);
                        cmd.Parameters.Add(new OracleParameter("expire", expire));
                        ////////////////////////////////////////////////////////////////////////////

                        cmd.Parameters.Add(new OracleParameter("category_name", category_name));

                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }//AddProduct



        public void StoredProduct(ProductModel prod_dto, NurseModel nurse_dto)
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

                        cmd.CommandText = "INSERT INTO PRODUCT_IN(PROD_IN_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_IN_FROM, PROD_IN_TO, PROD_IN_TYPE) " +
                                          "VALUES(:count, PROD_SEQ.CURRVAL, :nurse_no, :dept_id1, :in_from, (SELECT dept_name FROM DEPT WHERE dept_id = :dept_id2), :in_type)";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.Prod_total));
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("dept_id1", nurse_dto.Dept_id));

                        cmd.Parameters.Add(new OracleParameter("in_from", "발주처"));
                        cmd.Parameters.Add(new OracleParameter("dept_id2", nurse_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("in_type", "발주"));

                        cmd.ExecuteNonQuery();
                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }// StoredProduct()

        public void StoredProductForExcel(ProductShowModel prod_dto, NurseModel nurse_dto)
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

                        cmd.CommandText = "INSERT INTO PRODUCT_IN(PROD_IN_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_IN_FROM, PROD_IN_TO, PROD_IN_TYPE) " +
                                          "VALUES(:count, PROD_SEQ.CURRVAL, :nurse_no, :dept_id1, :in_from, (SELECT dept_name FROM DEPT WHERE dept_id = :dept_id2), :in_type)";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.Prod_total));
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("dept_id1", nurse_dto.Dept_id));

                        cmd.Parameters.Add(new OracleParameter("in_from", "발주처"));
                        cmd.Parameters.Add(new OracleParameter("dept_id2", nurse_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("in_type", "발주"));

                        cmd.ExecuteNonQuery();
                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }// StoredProduct()


        public List<ProductInOutModel> GetProductIn()
        {
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type, D.dept_name " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "ORDER BY I.prod_in_date, P.prod_name";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);
                            string dept_name = reader.GetString(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type,
                                Dept_name = dept_name
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//GetProductIn

        public List<ProductInOutModel> GetProductIn(DeptModel dept_dto) // 오버로딩
        {
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type, D.dept_name " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE I.prod_in_to = :name " +
                                          "ORDER BY I.prod_in_date, P.prod_name";

                        cmd.Parameters.Add(new OracleParameter("name", dept_dto.Dept_name));
                        Console.WriteLine("ProductDao - GetProductIn() 부서명 파라미터: " + dept_dto.Dept_name);
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);
                            string dept_name = reader.GetString(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type,
                                Dept_name = dept_name
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//GetProductIn


        public List<ProductInOutModel> GetProductIn(DeptModel dept_dto, DateTime? start_date, DateTime? end_date) // 오버로딩
        {
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type, D.dept_name " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE I.prod_in_to = :name " +
                                          "AND I.prod_in_date BETWEEN :start_date AND :end_date + 1 " +
                                          "ORDER BY I.prod_in_date, P.prod_name ";

                        cmd.Parameters.Add(new OracleParameter("name", dept_dto.Dept_name));
                        cmd.Parameters.Add(new OracleParameter("start_date", start_date));
                        cmd.Parameters.Add(new OracleParameter("end_date", end_date));

                        Console.WriteLine("ProductDao - GetProductIn() 부서명 파라미터: " + dept_dto.Dept_name);
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);
                            string dept_name = reader.GetString(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type,
                                Dept_name = dept_name
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//GetProductIn

        public List<ProductInOutModel> GetProductIn(DeptModel dept_dto, string search_type, string search_text) // 오버로딩
        {
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type, D.dept_name " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE " +
                                            "((:search_combo = '제품코드' AND I.prod_in_to = :dept_name ) AND (P.prod_code LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '제품명' AND I.prod_in_to = :dept_name ) AND (P.prod_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '품목/종류' AND I.prod_in_to = :dept_name ) AND (C.category_name LIKE '%'||:search_text||'%')) " +
                                          "ORDER BY I.prod_in_date, P.prod_name ";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));
                        cmd.Parameters.Add(new OracleParameter("search_combo", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_text));

                        Console.WriteLine("dept_name : " + dept_dto.Dept_name);
                        Console.WriteLine("search_combo : " + search_type);
                        Console.WriteLine("search_text : " + search_text);
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);
                            string dept_name = reader.GetString(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type,
                                Dept_name = dept_name
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//GetProductIn



        public List<ProductInOutModel> GetProductIn(DeptModel dept_dto, string search_type, string search_text, DateTime? start_date, DateTime? end_date) // 오버로딩
        {
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type, D.dept_name " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE " +
                                            "((:search_combo = '제품코드' AND I.prod_in_to = :dept_name AND I.prod_in_date BETWEEN :start_date AND :end_date +1 ) AND (P.prod_code LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '제품명' AND I.prod_in_to = :dept_name AND I.prod_in_date BETWEEN :start_date AND :end_date  +1 ) AND (P.prod_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '품목/종류' AND I.prod_in_to = :dept_name AND I.prod_in_date BETWEEN :start_date AND :end_date +1 ) AND (C.category_name LIKE '%'||:search_text||'%')) " +
                                          "ORDER BY I.prod_in_date, P.prod_name";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));
                        cmd.Parameters.Add(new OracleParameter("search_combo", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_text));

                        cmd.Parameters.Add(new OracleParameter("start_date", start_date));
                        cmd.Parameters.Add(new OracleParameter("end_date", end_date));

                        Console.WriteLine("dept_name : " + dept_dto.Dept_name);
                        Console.WriteLine("search_combo : " + search_type);
                        Console.WriteLine("search_text : " + search_text);
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);
                            string dept_name = reader.GetString(11);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type,
                                Dept_name = dept_name
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//GetProductIn


        public List<ProductInOutModel> GetProductOut()
        {
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, O.prod_out_count, O.prod_out_date, O.prod_out_type, D.dept_name, N.nurse_name, O.prod_out_from, O.prod_out_to " +
                                          "FROM PRODUCT_OUT O " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON O.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON O.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? prod_out_count = reader.GetInt32(4);
                            DateTime prod_out_date = reader.GetDateTime(5);
                            string prod_out_type = reader.GetString(6);
                            string dept_name = reader.GetString(7);
                            string nurse_name = reader.GetString(8);
                            string prod_out_from = reader.GetString(9);
                            string prod_out_to = reader.GetString(10);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Prod_out_count = prod_out_count,
                                Prod_out_date = prod_out_date,
                                Prod_out_type = prod_out_type,
                                Dept_name = dept_name,
                                Nurse_name = nurse_name,
                                Prod_out_from = prod_out_from,
                                Prod_out_to = prod_out_to
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//GetProductOut

        public List<ProductInOutModel> GetProductOut(DeptModel dept_dto) // 오버로딩
        {
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, O.prod_out_count, O.prod_out_date, O.prod_out_type, D.dept_name, N.nurse_name, O.prod_out_from, O.prod_out_to " +
                                          "FROM PRODUCT_OUT O " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON O.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON O.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE O.prod_out_from = :name";

                        cmd.Parameters.Add(new OracleParameter("name", dept_dto.Dept_name));
                        Console.WriteLine("ProductDao - GetProductOut() 부서명 파라미터: " + dept_dto.Dept_name);

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? prod_out_count = reader.GetInt32(4);
                            DateTime prod_out_date = reader.GetDateTime(5);
                            string prod_out_type = reader.GetString(6);
                            string dept_name = reader.GetString(7);
                            string nurse_name = reader.GetString(8);
                            string prod_out_from = reader.GetString(9);
                            string prod_out_to = reader.GetString(10);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Prod_out_count = prod_out_count,
                                Prod_out_date = prod_out_date,
                                Prod_out_type = prod_out_type,
                                Dept_name = dept_name,
                                Nurse_name = nurse_name,
                                Prod_out_from = prod_out_from,
                                Prod_out_to = prod_out_to
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//GetProductOut

        public List<ProductInOutModel> GetProductOut(DeptModel dept_dto, string search_type, string search_text)
        {
            List<ProductInOutModel> list = new List<ProductInOutModel>();

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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, O.prod_out_count, O.prod_out_date, O.prod_out_type, D.dept_name, N.nurse_name, O.prod_out_from, O.prod_out_to " +
                                          "FROM PRODUCT_OUT O " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON O.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON O.nurse_no = N.nurse_no " +
                                          "INNER JOIN DEPT D " +
                                          "ON N.dept_id = D.dept_id " +
                                          "WHERE " +
                                            "((:search_combo = '제품코드' AND O.prod_out_from = :dept_name ) AND (P.prod_code LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '제품명' AND O.prod_out_from = :dept_name ) AND (P.prod_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '품목/종류' AND O.prod_out_from = :dept_name ) AND (C.category_name LIKE '%'||:search_text||'%')) " +
                                          "AND D.dept_status != '폐지'";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));
                        cmd.Parameters.Add(new OracleParameter("search_combo", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_text));

                        Console.WriteLine("dept_name : " + dept_dto.Dept_name);
                        Console.WriteLine("search_combo : " + search_type);
                        Console.WriteLine("search_text : " + search_text);
                    
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? prod_out_count = reader.GetInt32(4);
                            DateTime prod_out_date = reader.GetDateTime(5);
                            string prod_out_type = reader.GetString(6);
                            string dept_name = reader.GetString(7);
                            string nurse_name = reader.GetString(8);
                            string prod_out_from = reader.GetString(9);
                            string prod_out_to = reader.GetString(10);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Prod_out_count = prod_out_count,
                                Prod_out_date = prod_out_date,
                                Prod_out_type = prod_out_type,
                                Dept_name = dept_name,
                                Nurse_name = nurse_name,
                                Prod_out_from = prod_out_from,
                                Prod_out_to = prod_out_to
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//GetProductOut

        public void AddImpDept(ProductModel prod_dto, NurseModel nurse_dto)
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

                        cmd.CommandText = "INSERT INTO IMP_DEPT(imp_dept_count, dept_id, prod_id) " +
                                          "VALUES(:count, :dept_id, PROD_SEQ.CURRVAL) ";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.Prod_total));
                        cmd.Parameters.Add(new OracleParameter("dept_id", nurse_dto.Dept_id));



                        cmd.ExecuteNonQuery();
                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }//AddImpDept

        public void AddImpDeptForExcel(ProductShowModel prod_dto, NurseModel nurse_dto)
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

                        cmd.CommandText = "INSERT INTO IMP_DEPT(imp_dept_count, dept_id, prod_id) " +
                                          "VALUES(:count, :dept_id, PROD_SEQ.CURRVAL) ";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.Prod_total));
                        cmd.Parameters.Add(new OracleParameter("dept_id", nurse_dto.Dept_id));



                        cmd.ExecuteNonQuery();
                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }//AddImpDept

        public ObservableCollection<ProductInOutModel> GetProductInByNurse(NurseModel nurse_dto)
        {
            ObservableCollection<ProductInOutModel> list = new ObservableCollection<ProductInOutModel>();
            Console.WriteLine("GetProductInByNurse 실행");
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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_expire, P.prod_price, I.prod_in_count, N.nurse_name, I.prod_in_date, I.prod_in_from, I.prod_in_to, I.prod_in_type " +
                                          "FROM PRODUCT_IN I " +
                                          "INNER JOIN PRODUCT P " +
                                          "ON I.prod_id = P.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "LEFT OUTER JOIN NURSE N " +
                                          "ON I.nurse_no = N.nurse_no " +
                                          "WHERE I.nurse_no = :no " +
                                          "ORDER BY I.prod_in_date DESC";

                        cmd.Parameters.Add(new OracleParameter(":no", nurse_dto.Nurse_no));
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            DateTime prod_expire = reader.GetDateTime(3);
                            int? prod_price = reader.GetInt32(4);
                            int? prod_in_count = reader.GetInt32(5);
                            string nurse_name = reader.GetString(6);
                            DateTime prod_in_date = reader.GetDateTime(7);
                            string prod_in_from = reader.GetString(8);
                            string prod_in_to = reader.GetString(9);
                            string prod_in_type = reader.GetString(10);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_expire = prod_expire,
                                Prod_price = prod_price,
                                Prod_in_count = prod_in_count,
                                Nurse_name = nurse_name,
                                Prod_in_date = prod_in_date,
                                Prod_in_from = prod_in_from,
                                Prod_in_to = prod_in_to,
                                Prod_in_type = prod_in_type
                            };

                            list.Add(dto);

                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;
        }//GetProductInByNurse


        public List<ProductShowModel> SearchProducts(DeptModel dept_dto, string search_type, string search_text)
        {
            List<ProductShowModel> list = new List<ProductShowModel>();
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

                        cmd.CommandText = "SELECT P.prod_code, P.prod_name, C.category_name, P.prod_price, I.imp_dept_count, P.prod_expire, P.prod_id, I.imp_dept_id " +
                                          "FROM PRODUCT P " +
                                          "INNER JOIN IMP_DEPT I " +
                                          "ON P.prod_id = I.prod_id " +
                                          "INNER JOIN CATEGORY C " +
                                          "ON P.category_id = C.category_id " +
                                          "INNER JOIN DEPT D " +
                                          "ON I.dept_id = D.dept_id " +
                                          "WHERE " +
                                            "((:search_combo = '제품코드' AND D.dept_name = :dept_name ) AND (P.prod_code LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '제품명' AND D.dept_name = :dept_name ) AND (P.prod_name LIKE '%'||:search_text||'%')) " +
                                          "OR " +
                                            "((:search_combo = '품목/종류' AND D.dept_name = :dept_name ) AND (C.category_name LIKE '%'||:search_text||'%')) " +
                                          "AND D.dept_status != '폐지' " +
                                          "ORDER BY P.prod_expire, P.prod_name";

                        cmd.BindByName = true;

                        cmd.Parameters.Add(new OracleParameter("search_combo", search_type));
                        cmd.Parameters.Add(new OracleParameter("search_text", search_text));
                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));

                        Console.WriteLine("dept_name : " + dept_dto.Dept_name);
                        Console.WriteLine("search_combo : " + search_type);
                        Console.WriteLine("search_text : " + search_text);
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string prod_code = reader.GetString(0);
                            string prod_name = reader.GetString(1);
                            string category_name = reader.GetString(2);
                            int? prod_price = reader.GetInt32(3);
                            int? imp_dept_count = reader.GetInt32(4);
                            DateTime prod_expire = reader.GetDateTime(5);
                            int? prod_id = reader.GetInt32(6);
                            int? imp_dept_id = reader.GetInt32(7);


                            ProductShowModel dto = new ProductShowModel()
                            {
                                Prod_code = prod_code,
                                Prod_name = prod_name,
                                Category_name = category_name,
                                Prod_price = prod_price,
                                Imp_dept_count = imp_dept_count,
                                Prod_expire = prod_expire,
                                Prod_id = prod_id,
                                Imp_dept_id = imp_dept_id
                            };

                            list.Add(dto);
                            
                        }// while

                    } //using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list;

        }//SearchProducts()


        public void ChangeProductInfo(ProductShowModel prod_dto)
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

                        cmd.CommandText = "UPDATE PRODUCT SET " +
                                          "prod_code = :code, " +
                                          "prod_name = :name, " +
                                          "category_id = (SELECT category_id FROM CATEGORY WHERE category_name = :category_name), " +
                                          "prod_expire = TO_DATE(:expire, 'YYYYMMDD'), " +
                                          "prod_price = :pirce, " +
                                          "prod_total = :total " +
                                          "WHERE prod_id = :id ";

                        cmd.Parameters.Add(new OracleParameter("code", prod_dto.Prod_code));
                        cmd.Parameters.Add(new OracleParameter("name", prod_dto.Prod_name));
                        cmd.Parameters.Add(new OracleParameter("category_name", prod_dto.Category_name));


                        // 날짜형식을 -> String 타입으로 변경 후 바인딩
                        string month = prod_dto.Prod_expire.Month.ToString();
                        if (prod_dto.Prod_expire.Month < 10)
                        {
                            month = "0" + prod_dto.Prod_expire.Month.ToString();
                        }// 선택한 월이 1자리 라면 앞에 0을 붙임

                        string day = prod_dto.Prod_expire.Day.ToString();
                        if (prod_dto.Prod_expire.Day < 10)
                        {
                            day = "0" + prod_dto.Prod_expire.Day.ToString();
                        }// 선택한 일이 1자리 라면 앞에 0을 붙임

                        string expire = prod_dto.Prod_expire.Year.ToString() + month + day;
                        Console.WriteLine("Insert DATE : {0}", expire);
                        cmd.Parameters.Add(new OracleParameter("expire", expire));
                        ////////////////////////////////////////////////////////////////////////////


                        cmd.Parameters.Add(new OracleParameter("price", prod_dto.Prod_price));
                        cmd.Parameters.Add(new OracleParameter("total", prod_dto.Imp_dept_count));
                        cmd.Parameters.Add(new OracleParameter("id", prod_dto.Prod_id));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch
        }//ChangeProductInfo()

        public void ChangeProductInfo_IMP_DEPT(ProductShowModel prod_dto)
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

                        cmd.CommandText = "UPDATE IMP_DEPT SET " +
                                          "imp_dept_count = :imp_total " +
                                          "WHERE imp_dept_id = :imp_id";

                        cmd.Parameters.Add(new OracleParameter("imp_total", prod_dto.Imp_dept_count));
                        cmd.Parameters.Add(new OracleParameter("imp_id", prod_dto.Imp_dept_id));



                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch
        }//ChangeProductInfo_IMP_DEPT()

        public void OutProduct(int? InputOutCount, ProductShowModel prod_dto, NurseModel nurse_dto, string SelectedOutType, DeptModel dept_dto)
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

                        cmd.CommandText = "INSERT INTO PRODUCT_OUT(PROD_OUT_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_OUT_FROM, PROD_OUT_TO, PROD_OUT_TYPE) " +
                                          "VALUES(:count, :prod_id, :nurse_no, :dept_id1, (SELECT dept_name FROM DEPT WHERE dept_id = :dept_id2), :out_to, :out_type)";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("prod_id", prod_dto.Prod_id));
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("dept_id1", nurse_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("dept_id2", nurse_dto.Dept_id));

                        ///////////////////////////////////////////////////////////////////////
                        if (SelectedOutType.Equals("이관"))
                        {
                            cmd.Parameters.Add(new OracleParameter("out_to", dept_dto.Dept_name)); //출고된 곳은 콤보박스에서 선택한 부서로 출고
                            OutProduct_FromTo(InputOutCount, prod_dto, nurse_dto, SelectedOutType, dept_dto); // 이관 시에 이관받은 부서에서 입고도 함께 진행
                            OutProduct_FromTo_IMP_DEPT(InputOutCount, prod_dto, dept_dto); // 이관되어서 해당부서는 입고받았기 때문에 imp_dept 테이블에도 추가
                        }
                        else if (SelectedOutType.Equals("사용")) //출고된 곳이 '사용'이라면
                        {
                            cmd.Parameters.Add(new OracleParameter("out_to", GetNurseDeptName(nurse_dto)));
                        }
                        else // 폐기 일때
                        {
                            cmd.Parameters.Add(new OracleParameter("out_to", SelectedOutType));
                        }
                        ///////////////////////////////////////////////////////////////////////
                        
                        cmd.Parameters.Add(new OracleParameter("out_type", SelectedOutType));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }// OutProduct()

        public void OutProduct(ProductShowModel prod_dto, NurseModel nurse_dto)//오버로딩
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

                        cmd.CommandText = "INSERT INTO PRODUCT_OUT(PROD_OUT_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_OUT_FROM, PROD_OUT_TO, PROD_OUT_TYPE) " +
                                          "VALUES(:count, :prod_id, :nurse_no, :dept_id1, (SELECT dept_name FROM DEPT WHERE dept_id = :dept_id2), :out_to, :out_type)";

                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("prod_id", prod_dto.Prod_id));
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("dept_id1", nurse_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("dept_id2", nurse_dto.Dept_id));

                        ///////////////////////////////////////////////////////////////////////
                        if (prod_dto.SelectedOutType.Equals("이관"))
                        {
                            cmd.Parameters.Add(new OracleParameter("out_to", prod_dto.SelectedOutDept.Dept_name)); //출고된 곳은 콤보박스에서 선택한 부서로 출고
                            OutProduct_FromTo(prod_dto, nurse_dto); // 이관 시에 이관받은 부서에서 입고도 함께 진행
                            OutProduct_FromTo_IMP_DEPT(prod_dto); // 이관되어서 해당부서는 입고받았기 때문에 imp_dept 테이블에도 추가
                        }
                        else if (prod_dto.SelectedOutType.Equals("사용")) //출고 유형이 '사용'이라면
                        {
                            cmd.Parameters.Add(new OracleParameter("out_to", GetNurseDeptName(nurse_dto)));
                        }
                        else // 폐기 일때
                        {
                            cmd.Parameters.Add(new OracleParameter("out_to", prod_dto.SelectedOutType));
                        }
                        ///////////////////////////////////////////////////////////////////////

                        cmd.Parameters.Add(new OracleParameter("out_type", prod_dto.SelectedOutType));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)


            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }// OutProduct()
        public string GetNurseDeptName(NurseModel nurse_dto)
        {
            string dept_name = "";
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
                        cmd.CommandText = "SELECT dept_name FROM DEPT WHERE dept_id = :dept_id ";

                        cmd.Parameters.Add(new OracleParameter("dept_id", nurse_dto.Dept_id));

                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            dept_name = reader.GetString(0);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return dept_name;

        }//GetNurseDeptName()





        public void OutProduct_FromTo(int? InputOutCount, ProductShowModel prod_dto, NurseModel nurse_dto, string SelectedOutType, DeptModel dept_dto)
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

                        cmd.CommandText = "INSERT INTO PRODUCT_IN(PROD_IN_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_IN_FROM, PROD_IN_TO, PROD_IN_TYPE) " +
                                          "VALUES(:count, :prod_id, :nurse_no, :dept_id, :in_from, :in_to, :in_type) ";

                        cmd.Parameters.Add(new OracleParameter("count", InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("prod_id", prod_dto.Prod_id));
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("dept_id", nurse_dto.Dept_id));

                        cmd.Parameters.Add(new OracleParameter("in_from", GetNurseDeptName(nurse_dto))); // 출고한 사원 소속 부서
                        cmd.Parameters.Add(new OracleParameter("in_to", dept_dto.Dept_name)); // 입고받은 부서명
                        cmd.Parameters.Add(new OracleParameter("in_type", SelectedOutType));

                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)

            }//try
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }//OutPorduct_FromTo()

        public void OutProduct_FromTo(ProductShowModel prod_dto, NurseModel nurse_dto) //오버로딩
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

                        cmd.CommandText = "INSERT INTO PRODUCT_IN(PROD_IN_COUNT, PROD_ID, NURSE_NO, DEPT_ID, PROD_IN_FROM, PROD_IN_TO, PROD_IN_TYPE) " +
                                          "VALUES(:count, :prod_id, :nurse_no, :dept_id, :in_from, :in_to, :in_type) ";

                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("prod_id", prod_dto.Prod_id));
                        cmd.Parameters.Add(new OracleParameter("nurse_no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("dept_id", nurse_dto.Dept_id));

                        cmd.Parameters.Add(new OracleParameter("in_from", GetNurseDeptName(nurse_dto))); // 출고한 사원 소속 부서
                        cmd.Parameters.Add(new OracleParameter("in_to", prod_dto.SelectedOutDept.Dept_name)); // 입고받은 부서명
                        cmd.Parameters.Add(new OracleParameter("in_type", prod_dto.SelectedOutType));

                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }//OutPorduct_FromTo()

        public void OutProduct_FromTo_IMP_DEPT(int? InputOutCount, ProductShowModel prod_dto, DeptModel dept_dto)
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

                        /*                        cmd.CommandText = "INSERT INTO IMP_DEPT(IMP_DEPT_COUNT, DEPT_ID, PROD_ID) " +
                                                                  "VALUES(:count, :dept_id, :pord_id) ";*/

                        cmd.CommandText = "MERGE INTO IMP_DEPT A " +
                                          "USING dual " +
                                          "ON(:prod_id IN(SELECT prod_id FROM IMP_DEPT WHERE dept_id = :dept_id AND prod_id = :prod_id) " +
                                          "   AND :dept_id IN(SELECT dept_id FROM IMP_DEPT WHERE dept_id = :dept_id AND prod_id = :prod_id)) " +
                                          "WHEN MATCHED THEN " +
                                          "UPDATE SET A.imp_dept_count = A.imp_dept_count + :count " +
                                          "WHERE A.dept_id = :dept_id AND A.prod_id = :prod_id " +
                                          "WHEN NOT MATCHED THEN " +
                                          "INSERT(A.imp_dept_count, A.dept_id, A.prod_id) " +
                                          "VALUES(:count, :dept_id, :prod_id)";

                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));
                        cmd.Parameters.Add(new OracleParameter("dept_id", dept_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));

                        cmd.Parameters.Add(new OracleParameter("dept_id", dept_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("dept_id", dept_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));

                        cmd.Parameters.Add(new OracleParameter("count", InputOutCount));

                        cmd.Parameters.Add(new OracleParameter("dept_id", dept_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));

                        cmd.Parameters.Add(new OracleParameter("count", InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("dept_id", dept_dto.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));
                        


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch
            
        }//OutProduct_FromTo_IMP_DEPT

        public void OutProduct_FromTo_IMP_DEPT(ProductShowModel prod_dto)//오버로딩
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

                        /*                        cmd.CommandText = "INSERT INTO IMP_DEPT(IMP_DEPT_COUNT, DEPT_ID, PROD_ID) " +
                                                                  "VALUES(:count, :dept_id, :pord_id) ";*/

                        cmd.CommandText = "MERGE INTO IMP_DEPT A " +
                                          "USING dual " +
                                          "ON(:prod_id IN(SELECT prod_id FROM IMP_DEPT WHERE dept_id = :dept_id AND prod_id = :prod_id) " +
                                          "   AND :dept_id IN(SELECT dept_id FROM IMP_DEPT WHERE dept_id = :dept_id AND prod_id = :prod_id)) " +
                                          "WHEN MATCHED THEN " +
                                          "UPDATE SET A.imp_dept_count = A.imp_dept_count + :count " +
                                          "WHERE A.dept_id = :dept_id AND A.prod_id = :prod_id " +
                                          "WHEN NOT MATCHED THEN " +
                                          "INSERT(A.imp_dept_count, A.dept_id, A.prod_id) " +
                                          "VALUES(:count, :dept_id, :prod_id)";

                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));
                        cmd.Parameters.Add(new OracleParameter("dept_id", prod_dto.SelectedOutDept.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));

                        cmd.Parameters.Add(new OracleParameter("dept_id", prod_dto.SelectedOutDept.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("dept_id", prod_dto.SelectedOutDept.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));

                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.InputOutCount));

                        cmd.Parameters.Add(new OracleParameter("dept_id", prod_dto.SelectedOutDept.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));

                        cmd.Parameters.Add(new OracleParameter("count", prod_dto.InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("dept_id", prod_dto.SelectedOutDept.Dept_id));
                        cmd.Parameters.Add(new OracleParameter("pord_id", prod_dto.Prod_id));



                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }//OutProduct_FromTo_IMP_DEPT

        public void ChangeProductInfo_IMP_DEPT_ForOut(int? InputOutCount, ProductShowModel prod_dto)
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

                        cmd.CommandText = "UPDATE IMP_DEPT SET " +
                                          "imp_dept_count = imp_dept_count - :imp_total " +
                                          "WHERE imp_dept_id = :imp_id";

                        cmd.Parameters.Add(new OracleParameter("imp_total", InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("imp_id", prod_dto.Imp_dept_id));



                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }//ChangeProductInfo_IMP_DEPT_ForOut

        public void ChangeProductInfo_IMP_DEPT_ForOut(ProductShowModel prod_dto)//오버로딩
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

                        cmd.CommandText = "UPDATE IMP_DEPT SET " +
                                          "imp_dept_count = imp_dept_count - :imp_total " +
                                          "WHERE imp_dept_id = :imp_id";

                        cmd.Parameters.Add(new OracleParameter("imp_total", prod_dto.InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("imp_id", prod_dto.Imp_dept_id));



                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }//ChangeProductInfo_IMP_DEPT_ForOut
        public void ChangeProductInfo_ForOut(int? InputOutCount, ProductShowModel prod_dto)
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

                        cmd.CommandText = "UPDATE PRODUCT SET " +
                                          "prod_total = prod_total - :total " +
                                          "WHERE prod_id = :id ";

                        cmd.Parameters.Add(new OracleParameter("total", InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("id", prod_dto.Prod_id));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }//ChangeProductInfo_ForOut

        public void ChangeProductInfo_ForOut(ProductShowModel prod_dto) //오버로딩
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

                        cmd.CommandText = "UPDATE PRODUCT SET " +
                                          "prod_total = prod_total - :total " +
                                          "WHERE prod_id = :id ";

                        cmd.Parameters.Add(new OracleParameter("total", prod_dto.InputOutCount));
                        cmd.Parameters.Add(new OracleParameter("id", prod_dto.Prod_id));


                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)
            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }//ChangeProductInfo_ForOut 
        public List<ProductShowModel> Prodcode_Info()     //prodcode 
        {
            List<ProductShowModel> list = new List<ProductShowModel>();
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
                        cmd.CommandText = "select distinct(prod_code) from product";
                        //cmd.CommandText = "SELECT * FROM NURSE WHERE nurse_no = :no AND nurse_pw = :pw";

                        //cmd.Parameters.Add(new OracleParameter("p_code", prod_dto.Prod_code));
                        //cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string Prod_code = reader.GetString(0);
                            //int? Prod_total = reader.GetInt32(1);
                            ProductShowModel dto = new ProductShowModel()
                            {
                                Prod_code = Prod_code
                            };
                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch
            return list;
        }///product_info
        

        public List<ProductShowModel> Get_Dept_Category_RemainExpire(DeptModel SelectedDept, CategoryModel SelectedCategory)
        {
            Console.WriteLine("start");
            List<ProductShowModel> list = new List<ProductShowModel>();
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
                        cmd.CommandText = "SELECT prod_code, TO_CHAR(TO_DATE(TO_CHAR(prod_expire, 'YYYYMMDD')) - TO_DATE(TO_CHAR(CURRENT_DATE, 'YYYYMMDD'))) " +
                            "FROM PRODUCT P " +
                            "INNER JOIN CATEGORY C " +
                            "ON P.Category_id = c.category_id " +
                            "INNER JOIN IMP_DEPT I " +
                            "ON P.prod_id = I.prod_id " +
                            "INNER JOIN DEPT D " +
                            "ON I.dept_id = D.dept_id " +
                            "WHERE d.dept_name= :dept_name and C.category_name = :category_name " +
                            "order by TO_CHAR(TO_DATE(TO_CHAR(prod_expire, 'YYYYMMDD')) - TO_DATE(TO_CHAR(CURRENT_DATE, 'YYYYMMDD'))) asc";


                        cmd.Parameters.Add(new OracleParameter("dept_name", SelectedDept.Dept_name));
                        cmd.Parameters.Add(new OracleParameter("category_name", SelectedCategory.Category_name)); //category_name

                        //cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string Prod_code = reader.GetString(0);
                            string Prod_remainexpire = reader.GetString(1);
                            ProductShowModel dto = new ProductShowModel()
                            {
                                Prod_code = Prod_code,
                                Prod_remainexpire = Convert.ToInt32(Prod_remainexpire)
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch
            return list;
        }///Get_Dept_Category_RemainExpire


        //부서별 카테고리별//제품 총수량 그래프 
        public List<ImpDeptModel> Dept_Category_Mount(DeptModel SelectedDepts)
        {
            List<ImpDeptModel> list = new List<ImpDeptModel>();
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
                        cmd.CommandText = "SELECT D.dept_name, C.category_name, SUM(I.imp_dept_count) " +
                            "FROM IMP_DEPT I " +
                            "INNER JOIN PRODUCT P " +
                            "ON I.prod_id = P.prod_id " +
                            "INNER JOIN CATEGORY C " +
                            "ON P.category_id = C.category_id " +
                            "INNER JOIN DEPT D " +
                            "ON I.dept_id = D.dept_id " +
                            "WHERE D.dept_name = :dept_name " +
                            "GROUP BY C.category_name, D.dept_name";


                        cmd.Parameters.Add(new OracleParameter("dept_name", SelectedDepts.Dept_name)); //category_name
                        //cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string Category_name = reader.GetString(1);
                            int? SUM_dept = reader.GetInt32(2);
                            ImpDeptModel dto = new ImpDeptModel()
                            {
                                Category_name = Category_name,
                                Imp_dept_count = SUM_dept
                            };

                            list.Add(dto);
                        }//while

                    }//using(cmd)

                }//using(conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch
            return list;
        }///Dept_Category_Mount


    }//class



}//namespace
