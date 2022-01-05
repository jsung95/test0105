using EasyProject.Model;
using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;

namespace EasyProject.Dao
{
    public class DashBoardDao : CommonDBConn, IDashBoardDao
    {
        public ObservableCollection<ProductShowModel> Prodcode_Info()     //prodcode 
        {
            ObservableCollection<ProductShowModel> list = new ObservableCollection<ProductShowModel>();
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
        public ObservableCollection<ProductShowModel> Prodtotal_Info()               //total
        {
            ObservableCollection<ProductShowModel> list = new ObservableCollection<ProductShowModel>();
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
                        cmd.CommandText = "select sum(prod_total) from product group by prod_code";
                        //cmd.CommandText = "SELECT * FROM NURSE WHERE nurse_no = :no AND nurse_pw = :pw";

                        //cmd.Parameters.Add(new OracleParameter("p_code", prod_dto.Prod_code));
                        //cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            int? Prod_total = reader.GetInt32(0);
                            ProductShowModel dto = new ProductShowModel()
                            {
                                Prod_total = Prod_total
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
        public List<ProductShowModel> Prodcodetotal_Info(DeptModel SelectedDept)               //code total 리스트
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
                        cmd.CommandText = "SELECT P.prod_code, sum(I.imp_dept_count) " +
                            "FROM PRODUCT P " +
                            "INNER JOIN IMP_DEPT I " +
                            "ON P.prod_id = I.prod_id " +
                            "WHERE I.dept_id = :dept_id " +
                            "GROUP BY(P.prod_code) ";
                        //cmd.CommandText = "SELECT * FROM NURSE WHERE nurse_no = :no AND nurse_pw = :pw";

                        cmd.Parameters.Add(new OracleParameter("dept_id", SelectedDept.Dept_id));
                        //cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string Prod_code = reader.GetString(0);
                            int? Prod_total = reader.GetInt32(1);
                            ProductShowModel dto = new ProductShowModel()
                            {
                                Prod_code = Prod_code,
                                Prod_total = Prod_total
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
        }///Prodcodetotal_info

        //카테고리별 --부서별/제품총수량 그래프 Dao
        public List<ImpDeptModel> Dept_Category_Mount(CategoryModel SelectedCategory)
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
                        cmd.CommandText = "SELECT C.category_name, D.dept_name, SUM(I.imp_dept_count) " +
                            "FROM IMP_DEPT I " +
                            "INNER JOIN PRODUCT P " +
                            "ON I.prod_id = P.prod_id " +
                            "INNER JOIN CATEGORY C " +
                            "ON P.category_id = C.category_id " +
                            "INNER JOIN DEPT D " +
                            "ON I.dept_id = D.dept_id " +
                            "where c.category_name = :category_name " +
                            "GROUP BY C.category_name, D.dept_name";


                        cmd.Parameters.Add(new OracleParameter("category_name", SelectedCategory.Category_name)); //category_name
                        //cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string Dept_name = reader.GetString(1);
                            int? SUM_dept = reader.GetInt32(2);
                            ImpDeptModel dto = new ImpDeptModel()
                            {
                                Dept_name = Dept_name,
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

        public List<ProductShowModel> Prodexpiretotal_Info(DeptModel SelectedDept, CategoryModel SelectedCategory)               //code total 리스트
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
                        cmd.CommandText = "SELECT TO_DATE(TO_CHAR(prod_expire, 'YYYYMMDD')) - TO_DATE(TO_CHAR(CURRENT_DATE, 'YYYYMMDD')), PROD_TOTAL"
                                         + "FROM PRODUCT";
                        //cmd.CommandText = "SELECT * FROM NURSE WHERE nurse_no = :no AND nurse_pw = :pw";

                        cmd.Parameters.Add(new OracleParameter("dept_id", SelectedDept.Dept_id));
                        //cmd.Parameters.Add(new OracleParameter("total", prod_dto.Prod_total));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string Prod_code = reader.GetString(0);
                            int? Prod_total = reader.GetInt32(1);
                            ProductShowModel dto = new ProductShowModel()
                            {
                                Prod_code = Prod_code,
                                Prod_total = Prod_total
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
        }///Prodexpiretotal_info

        public List<ProductInOutModel> ReleaseCases_Info(DateTime startDate, DateTime endDate) // 부서별 출고 횟수 정보를 담은 리스트
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
                        cmd.CommandText = "SELECT prod_out_from, " +
                                          "COUNT(CASE WHEN prod_out_type = '사용' THEN 1 END), " +
                                          "COUNT(CASE WHEN prod_out_type = '이관' THEN 1 END), " +
                                          "COUNT(CASE WHEN prod_out_type = '폐기' THEN 1 END) " +
                                          "FROM product_out " +
                                          "WHERE prod_out_date > :startDate " +
                                          "AND prod_out_date < :endDate + 1 " +
                                          "GROUP BY prod_out_from";

                        cmd.Parameters.Add(new OracleParameter("startDate", startDate));
                        cmd.Parameters.Add(new OracleParameter("endDate", endDate));
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string dept_name = reader.GetString(0);
                            int? prod_use_cases = reader.GetInt32(1);
                            int? prod_transferOut_cases = reader.GetInt32(2);
                            int? prod_discard_cases = reader.GetInt32(3);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Dept_name = dept_name,
                                prod_use_cases = prod_use_cases,
                                prod_transferOut_cases = prod_transferOut_cases,
                                prod_discard_cases = prod_discard_cases
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
        }//ReleaseCases_Info

        public List<ProductInOutModel> incomingCases_Info(DateTime startDate, DateTime endDate)
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
                        cmd.CommandText = "SELECT prod_in_to, " +
                                          "COUNT(CASE WHEN prod_in_type = '이관' THEN 1 END), " +
                                          "COUNT(CASE WHEN prod_in_type = '발주' THEN 1 END) " +
                                          "FROM product_in " +
                                          "WHERE prod_in_date > :startDate " +
                                          "AND prod_in_date < :endDate + 1 " +
                                          "GROUP BY prod_in_to";

                        cmd.Parameters.Add(new OracleParameter("startDate", startDate));
                        cmd.Parameters.Add(new OracleParameter("endDate", endDate));
                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string dept_name = reader.GetString(0);
                            int? prod_transferIn_cases = reader.GetInt32(1);
                            int? prod_order_cases = reader.GetInt32(2);

                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Dept_name = dept_name,
                                prod_transferIn_cases = prod_transferIn_cases,
                                prod_order_cases = prod_order_cases
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
        }//orderCases_Info



        public List<ProductInOutModel> GetDiscardTotalCount(DeptModel dept_dto)
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
                        cmd.CommandText = "SELECT P.prod_name , O.prod_out_count, O.prod_out_count * P.prod_price " +
                                          "FROM product_out O " +
                                          "INNER JOIN product P " +
                                          "ON O.prod_id = P.prod_id " +
                                          "INNER JOIN dept D " +
                                          "ON O.dept_id = D.dept_id " +
                                          "WHERE O.prod_out_type = '폐기'" +
                                          "AND D.dept_name = :dept_name";

                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            ProductInOutModel dto = new ProductInOutModel()
                            {
                                Prod_name = reader.GetString(0),
                                Prod_out_count = reader.GetInt32(1),
                                Prod_price = reader.GetInt32(2)
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
        }//GetDiscardTotalCount
        
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


    }//class
}//namespace
