using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyProject.Model;

namespace EasyProject.Dao
{
    public interface ICategoryDao
    {
        //전첼 카테고리 리스트 가져오기
        List<CategoryModel> GetCategories();

        //카테고리 이름으로 카테고리 번호 가져오기
        int GetCategoryID(string category_name);

        // = 카테고리 추가 =
        //기존에 존재하는 카테고리인지 판별 
        bool IsExistsCategory(CategoryModel category_dto);
        //카테고리 추가
        void AddCategory(CategoryModel category_dto);

    }//interface

}//namespace
