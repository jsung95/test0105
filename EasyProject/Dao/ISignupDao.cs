using EasyProject.Model;
using System;
using System.Collections.Generic;


namespace EasyProject.Dao
{

    public interface ISignupDao
    {
        List<DeptModel> GetDeptModels();
        void SignUp(NurseModel nurse_dto, DeptModel dept_dto);

        NurseModel IdCheck(NurseModel nurse_dto);
    } // interface 
} // namespace
