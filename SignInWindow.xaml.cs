using System;
using System.Windows;
using Microsoft.VisualStudio.PlatformUI;

namespace VSPastebinExtension
{
    public partial class SignInWindow : DialogWindow
    {
        public SignInWindow()
        {
            InitializeComponent();
        }

        private async void LoginOnClick(Object sender, RoutedEventArgs e)
        {
            using (ButtonEnableWrapper wrapper = new ButtonEnableWrapper(LoginButton))
            {
                PastebinHelper.ApplyDevKey();
                if (await AuthorizationManager.TryLogInAsync(Username.Text, Password.Password))
                    Close();
            }
        }
    }
}
