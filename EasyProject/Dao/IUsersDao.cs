using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasyProject.Model;


namespace EasyProject.Dao
{
    public interface IUsersDao
    {
        //권한 변경 전 사용자들 정보 SELECT
        List<UserModel> GetUserInfo(string auth);

        //권한 변경 쿼리 UPDATE
        void UserAuthChange(string auth, List<UserModel> no);

        //사용자 검색
        List<UserModel> SearchUser(string auth, string searchType, string name);

        //사용자 정보 데이터 가져오기
        UserModel GetUserInfoWithDept(NurseModel nurse_dto);
    }// interface

}//namespace
