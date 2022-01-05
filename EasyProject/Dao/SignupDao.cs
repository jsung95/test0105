using EasyProject.Model;
using EasyProject.Util;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Windows;

namespace EasyProject.Dao
{
    public class SignupDao : CommonDBConn, ISignupDao //DB연결 Class 및 인터페이스 상속
    {

        public List<DeptModel> GetDeptModels()
        {
            List<DeptModel> list = new List<DeptModel>();
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

                        cmd.CommandText = "SELECT DEPT_NAME FROM DEPT";

                        OracleDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string dept_name = reader.GetString(0); // 선택된 데이터셋 0번 컬럼 = 부서 이름 -> 문자열 변수에 담음.

                            DeptModel dept = new DeptModel() // DeptModel 객체를 생성, 필드값에 dept_name 넣어줌
                            {
                                Dept_name = dept_name
                            };
                            list.Add(dept); // 리스트에 추가
                        }//while

                    }//using(cmd)

                }//using(conn)
                


            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return list; // DeptModel 객체들이 담긴 list 리턴

        } // GetDeptModels

        public void SignUp(NurseModel nurse_dto, DeptModel dept_dto)
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

                        cmd.CommandText = "INSERT INTO NURSE(NURSE_NO, NURSE_NAME, NURSE_PW, DEPT_ID) VALUES(:no, :name, :pw, (SELECT dept_id FROM DEPT WHERE dept_name = :dept_name))";

                        // INSERT INTO NURSE(NURSE_NO, NURSE_NAME, NURSE_PW, DEPT_ID) VALUES(:no, :name, :pw, :dept_id)
                        //파라미터 값 바인딩
                        cmd.Parameters.Add(new OracleParameter("no", nurse_dto.Nurse_no));
                        cmd.Parameters.Add(new OracleParameter("name", nurse_dto.Nurse_name));
                        //cmd.Parameters.Add(new OracleParameter("auth", "NORMAL")); // auth(회원 권한)은 테이블 default 제약에 의해 기본 NORMAL로 설정
                        cmd.Parameters.Add(new OracleParameter("pw", SHA256Hash.StringToHash(nurse_dto.Nurse_pw))); //비밀번호 암호화
                        cmd.Parameters.Add(new OracleParameter("dept_name", dept_dto.Dept_name));

                        cmd.ExecuteNonQuery();

                    }//using(cmd)

                }//using(conn)

            }//try
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

        }//SignUp(string sql)

        public NurseModel IdCheck(NurseModel nurse_dto)
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
                        cmd.CommandText = "SELECT nurse_no FROM nurse WHERE nurse_no = :no";
                        cmd.BindByName = false;
                        cmd.Parameters.Add(new OracleParameter("no", nurse_dto.Nurse_no));
                        OracleDataReader reader = cmd.ExecuteReader();

                        if (reader.Read() == false)
                        {
                            //MessageBox.Show("회원가입처리를 진행합니다.");
                        }
                        else
                        {
                            MessageBox.Show("중복입니다! 다시입력하세요.");
                            nurse_dto.Nurse_no = null;
                        }
                    }//using(cmd)
                }//using (conn)

            }//try
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }//catch

            return nurse_dto;

        }//IdCheck

    } // SignupDao
} // namespace
