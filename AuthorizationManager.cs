using System;
using System.Threading.Tasks;
using System.Windows;
using PastebinAPI;

namespace VSPastebinExtension
{
    public static class AuthorizationManager
    {
        public static User CurrentUser { get; private set; }

        public static Boolean Authorized => CurrentUser != null;

        public static async Task<Boolean> TryLogInAsync(String username, String password)
        {
            PastebinHelper.ApplyDevKey();
            try
            {
                CurrentUser = await Pastebin.LoginAsync(username, password);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, PastebinHelper.DefaultCaption);
                return false;
            }
        }

        public static void LogOut()
        {
            CurrentUser = null;
        }
    }
}
