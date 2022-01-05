using EasyProject.Dao;
using EasyProject.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Expression.Interactivity.Core;
using System.Linq;

namespace EasyProject.ViewModel
{
    public class UserAuthViewModel : Notifier
    {
        UsersDao dao = new UsersDao();
        // 콤보박스의 검색타입 리스트
        public string[] SearchTypeList { get; set; }
        //검색 텍스트박스로 부터 입력받은 데이터를 담을 프로퍼티
        private string normal_Keyword;
        public string Normal_Keyword
        {
            get { return normal_Keyword; }
            set
            {
                normal_Keyword = value;
                OnPropertyChanged("Normal_Keyword");
                OnNormalKeywordChanged();
            }
        }
        //public string Admin_Keyword { get; set; }

        private string admin_Keyword;
        public string Admin_Keyword
        {
            get { return admin_Keyword; }
            set
            {
                admin_Keyword = value;
                OnPropertyChanged("Admin_Keyword");
                OnAdminKeywordChanged();
            }
        }
        // 콤보박스에서 선택한 권한을 담을 프로퍼티
        public string NormalSearchType { get; set; }
        public string AdminSearchType { get; set; }

        // 사용자 검색 시 나온 사용자 정보를 담을 옵저버블컬렉션 프로퍼티
        public ObservableCollection<UserModel> Normals_searched { get; set; }

        public ObservableCollection<UserModel> Admins_searched { get; set; }

        public List<UserModel> Normal_users { get; set; }

        public List<UserModel> Admin_users { get; set; }

        public UserAuthViewModel()
        {
            SearchTypeList = new[] { "이름", "아이디", "부서" };
            Normals_searched = new ObservableCollection<UserModel>(dao.GetUserInfo("NORMAL")); // 화면에 보일 리스트
            Admins_searched = new ObservableCollection<UserModel>(dao.GetUserInfo("ADMIN"));  // 화면에 보일 리스트
            Normal_users = dao.GetUserInfo("NORMAL"); // 사용자들을 검색할 리스트
            Admin_users = dao.GetUserInfo("ADMIN");   // 사용자들을 검색할 리스트
            
        }//Constructor

        //private ActionCommand normalSearchCommand;
        //public ICommand NormalSearchCommand
        //{
        //    get
        //    {
        //        if (normalSearchCommand == null)
        //        {
        //            normalSearchCommand = new ActionCommand(NormalSearch);
        //        }
        //        return normalSearchCommand;
        //    }//get

        //}//Command

        //private ActionCommand adminSearchCommand;
        //public ICommand AdminSearchCommand
        //{
        //    get
        //    {
        //        if (adminSearchCommand == null)
        //        {
        //            adminSearchCommand = new ActionCommand(AdminSearch);
        //        }
        //        return adminSearchCommand;
        //    }//get

        //}//Command

        private ActionCommand moveRightCommand;
        public ICommand MoveRightCommand
        {
            get
            {
                if (moveRightCommand == null)
                {
                    moveRightCommand = new ActionCommand(MoveRight);
                }
                return moveRightCommand;
            }//get

        }//Command

        private ActionCommand moveLeftCommand;
        public ICommand MoveLeftCommand
        {
            get
            {
                if (moveLeftCommand == null)
                {
                    moveLeftCommand = new ActionCommand(MoveLeft);
                }
                return moveLeftCommand;
            }//get

        }//Command

        //public void NormalSearch() // 좌측 리스트(NORMAL) 검색
        //{
        //    Console.WriteLine("Normal 유저 검색");
        //    Normals_searched.Clear();
        //    List<UserModel> list = dao.SearchUser("NORMAL", NormalSearchType, Normal_Keyword);
        //    foreach (UserModel user in list)
        //    {
        //        Normals_searched.Add(user);
        //    }
        //}

        //public void AdminSearch() // 우측 리스트(ADMIN) 검색
        //{
        //    Console.WriteLine("Admin 유저 검색");
        //    Admins_searched.Clear();
        //    List<UserModel> list = dao.SearchUser("ADMIN", AdminSearchType, Admin_Keyword);
                        
        //    foreach (UserModel user in list)
        //    {
        //        Admins_searched.Add(user);
        //    }
        //}

        public void MoveRight()
        {
            Console.WriteLine("MoveRight");
            ObservableCollection<UserModel> tempObject = new ObservableCollection<UserModel>();
            List<UserModel> updateList = new List<UserModel>(); // 업데이트 할 객체들을 담을 임시리스트

            foreach (var item in Normals_searched)
            {
                if (item.IsChecked)// item.IsChecked = true -> 권한 변경할 객체들 
                {
                    item.IsChecked = false;
                    Admins_searched.Add(item); // 화면에 보이는 Admin_searched 목록에 있는 리스트                   
                    Admin_users.Add(item);
                    Normal_users.Remove(item);
                    item.Nurse_auth = "ADMIN";
                    updateList.Add(item);                    
                }
                else tempObject.Add(item); // 체크 박스 선택되지 않은 리스트
            }

            dao.UserAuthChange("ADMIN", updateList); // 업데이트 실행
            updateList.Clear();

            Normals_searched.Clear(); // 기존의 검색 목록을 비움.

            foreach (var item in tempObject)
            {
                Normals_searched.Add(item); // 선택되지 않은 리스트만 검색 목록에 다시 넣어줌
            }
            Normal_users = dao.GetUserInfo("NORMAL"); // 사용자들을 검색할 리스트 갱신
            Admin_users = dao.GetUserInfo("ADMIN");   // 사용자들을 검색할 리스트 갱신
        }//MoveRight

        public void MoveLeft()
        {
            Console.WriteLine("MoveLeft");          
            ObservableCollection<UserModel> tempObject =new  ObservableCollection<UserModel>();
            List<UserModel> updateList = new List<UserModel>(); // 업데이트 할 객체들을 담을 임시리스트

            foreach (var item in Admins_searched)
            {
                if (item.IsChecked)
                {
                    item.IsChecked = false;
                    Normals_searched.Add(item);
                    Normal_users.Add(item);
                    Admin_users.Remove(item);
                    item.Nurse_auth = "NORMAL";
                    updateList.Add(item);
                }
                else tempObject.Add(item);
            }

            dao.UserAuthChange("NORMAL", updateList);
            updateList.Clear();

            Admins_searched.Clear();

            foreach (var item in tempObject)
            {
                Admins_searched.Add(item);
            }
            Normal_users = dao.GetUserInfo("NORMAL"); // 사용자들을 검색할 리스트 갱신
            Admin_users = dao.GetUserInfo("ADMIN");   // 사용자들을 검색할 리스트 갱신
        }//MoveLeft

        public void OnNormalKeywordChanged()
        {
            Console.WriteLine("OnNormalKeywordChanged()");
            IEnumerable<UserModel> temp = new List<UserModel>();

            switch (NormalSearchType)
            {
                case "이름":
                    temp = Normal_users.Where(user => user.Nurse_name.Contains(Normal_Keyword));
                    break;
                case "아이디":
                    temp = Normal_users.Where(user => user.Nurse_no.ToString().Contains(Normal_Keyword));
                    break;
                case "부서":
                    temp = Normal_users.Where(user => user.Dept_name.Contains(Normal_Keyword));
                    break;
            }
        
            Normals_searched.Clear();
            foreach (var item in temp)
            {
                Normals_searched.Add(item);
            }         
        }

        public void OnAdminKeywordChanged()
        {
            Console.WriteLine("OnAdminKeywordChanged()");
            IEnumerable<UserModel> temp = new List<UserModel>();

            switch (AdminSearchType)
            {
                case "이름":
                    temp = Admin_users.Where(user => user.Nurse_name.Contains(Admin_Keyword));
                    break;
                case "아이디":
                    temp = Admin_users.Where(user => user.Nurse_no.ToString().Contains(Admin_Keyword));
                    break;
                case "부서":
                    temp = Admin_users.Where(user => user.Dept_name.Contains(Admin_Keyword));
                    break;
            }
            Admins_searched.Clear();
            foreach (var item in temp)
            {
                Admins_searched.Add(item);
            }
        }
        //public IEnumerable<UserModel> FindProducts(string searchString)
        //{
        //    return Normals_searched.Where(user => user.Nurse_name.Contains(searchString));
        //}
    }//class

}//namespace
