using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyProject.Model;

namespace EasyProject.Dao
{
    public interface ILoginDao
    {
        //로그인 사용자 정보 얻기
        NurseModel LoginUserInfo(NurseModel nurse_dto);
        bool IdPasswordCheck(string nurse_no, string nurse_pw);
        bool IdPasswordCheck(NurseModel nurse_dto);

        void PasswordChange(NurseModel nurse_dto, string newPassword);

        //로그인 시에 로그도 같이 쌓기
        void Login_Logging(NurseModel nurse_dto);

        //로그아웃 시에 로그도 같이 쌓기
        void Logout_Logging(NurseModel nurse_dto);

    }//interface

}//namespace
