using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TransportManagerment.Model;

namespace TransportManagerment.DataAccess
{
    enum Proxy
    {
        Admin = 0,
        Staff = 1,
        User = 2
    }
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set => instance = value;
        }

        private AccountDAO() { }

        public bool Login(string userName, string password)
        {
            try
            {
                var checkAcc = DataProvider.Instance.db.Accounts.Where(x => x.username == userName && x.password == password);
                if (checkAcc.Count() == 1)
                {
                    DataProvider.Instance.curUser = checkAcc.First();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void AddNewAcc(string username, string displayName, byte? proxy)
        {
            var temp = DataProvider.Instance.db.Accounts.Where(x => x.username == username).Count();
            if (temp > 0)
            {
                MessageBox.Show("Tên đăng nhập đã tồn tại.");
                return;
            }

            var newAcc = new Account()
            {
                username = username,
                displayName = displayName,
                password = "1",
                proxy = proxy
            };
            DataProvider.Instance.db.Accounts.Add(newAcc);
            DataProvider.Instance.db.SaveChanges();
        }

        public void UpdateAcc(Account selectedAcc, string displayName, byte? proxy, bool isAdmin)
        {
            var acc = DataProvider.Instance.db.Accounts.Where(x => x.username == selectedAcc.username).SingleOrDefault();
            acc.displayName = displayName;
            if (isAdmin)
                acc.proxy = proxy;
            DataProvider.Instance.db.SaveChanges();
        }

        public void DeleteAcc(Account selectedAcc)
        {
            DataProvider.Instance.db.Accounts.Remove(selectedAcc);
            DataProvider.Instance.db.SaveChanges();
        }

        public List<Account> GetListAccount()
        {
            return DataProvider.Instance.db.Accounts.ToList();
        }

        public List<int> GetListProxy()
        {
            return new List<int>() { (int)Proxy.Admin, (int)Proxy.Staff, (int)Proxy.User };
        }

        private bool ChangePass(string newpass)
        {
            var upt = DataProvider.Instance.db.Accounts.Where(x => x.username == DataProvider.Instance.curUser.username).SingleOrDefault();
            if (upt != null)
            {
                upt.password = newpass;
                DataProvider.Instance.curUser.password = newpass;
                DataProvider.Instance.db.SaveChanges();
                return true;
            }
            return false;
        }

        public bool CheckNewPass(string cur, string newP, string reP)
        {
            if (DataProvider.Instance.curUser.password == cur)
            {
                if (DataProvider.Instance.curUser.password != newP)
                {
                    if (newP == reP)
                    {
                        return ChangePass(newP);
                    }
                    else
                    {
                        MessageBox.Show("Nhập lại mật khẩu không chính xác.");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Mật khẩu mới không được giống mật khẩu hiện tại.");
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Mật khẩu không chính xác.");
                return false;
            }
        }
    }
}
