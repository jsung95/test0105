using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyProject.Model;

namespace EasyProject.Dao
{
    public interface IDashBoardDao
    {

        //파이차트 - 부서별로 폐기된 제품의 총 수량 및 총 가격 정보
        List<ProductInOutModel> GetDiscardTotalCount(DeptModel dept_dto);
    }//interface

}//namespace
