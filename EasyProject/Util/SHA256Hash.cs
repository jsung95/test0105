using System;
using System.Security.Cryptography;
using System.Text;

namespace EasyProject.Util
{
    //회원 - 비밀번호 암호화 알고리즘
    public static class SHA256Hash
    {
        public static string StringToHash(string userPW)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(userPW));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }//foreach
            return stringBuilder.ToString();
        }//StringToHash(string)
    }//class
}//namespace
