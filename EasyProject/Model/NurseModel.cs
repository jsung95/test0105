using System;


namespace EasyProject.Model
{
    public class NurseModel : Notifier
    {
        //
        private string nurse_no;
        public string Nurse_no
        {
            get { return nurse_no; }
            set
            {
                nurse_no = value;
                OnPropertyChanged("Nurse_no");
            }
        }

        private string nurse_name;
        public string Nurse_name
        {
            get { return nurse_name; }
            set
            {
                nurse_name = value;
                OnPropertyChanged("Nurse_name");
            }
        }

        private string nurse_auth;
        public string Nurse_auth
        {
            get { return nurse_auth; }
            set
            {
                nurse_auth = value;
                OnPropertyChanged("Nurse_auth");
            }
        }

        private string nurse_pw;
        public string Nurse_pw
        {
            get { return nurse_pw; }
            set
            {
                nurse_pw = value;
                OnPropertyChanged("Nurse_pw");
            }
        }

        private string nurse_re_pw;
        public string Nurse_re_pw
        {
            get { return nurse_re_pw; }
            set
            {
                nurse_re_pw = value;
                OnPropertyChanged("Nurse_re_pw");
            }
        }

        public int? Dept_id { get; set; }

    }
}
