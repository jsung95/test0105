using EasyProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyProject.Dao
{
    internal interface IOrderDao
    {
        List<DeptModel> GetDeptModels();
    }
}
