using System;
using System.Windows;
using System.Windows.Documents;
using Microsoft.VisualStudio.PlatformUI;
using PastebinAPI;

namespace VSPastebinExtension
{
    public partial class MainWindow : DialogWindow
    {
        public MainWindow(DocumentModel model)
        {
            InitializeComponent();
            InitializeComboBoxes();
            ApplyAuthorizedUserAbilities();
            PasteName.Text = model.Name;
            PasteExpiration.Text = PastebinHelper.ExpirationToStringDictionary[Expiration.Never];
            SyntaxHighlighting.Text = PastebinHelper.IdentifyLanguage(model.Language).ToUpper();
            PasteText.Inlines.Add(model.Text);
        }

        private void InitializeComboBoxes()
        {
            foreach(var language in PastebinAPI.Language.All)
            {
                SyntaxHighlighting.Items.Add(language.ToString().ToUpper());
            }
            foreach (var expiration in PastebinHelper.StringToExpirationDictionary.Keys)
            {
                PasteExpiration.Items.Add(expiration);
            }
        }

        private void ApplyAuthorizedUserAbilities()
        {
            if (AuthorizationManager.CurrentUser != null)
            {
                AuthorizationButton.Content = "Log Out";
                Username.Text = AuthorizationManager.CurrentUser.Name;
                PasteAsAGuestCheckBox.IsEnabled = true;
                SetPasteExposureItems(PastebinAPI.Visibility.Public, PastebinAPI.Visibility.Unlisted, PastebinAPI.Visibility.Private);
            }
            else
            {
                AuthorizationButton.Content = "Sign In";
                Username.Text = "Not authorized";
                PasteAsAGuestCheckBox.IsChecked = false;
                PasteAsAGuestCheckBox.IsEnabled = false;
                SetPasteExposureItems(PastebinAPI.Visibility.Public, PastebinAPI.Visibility.Unlisted);
            }
            PasteExposure.SelectedIndex = 0;
        }

        private void SetPasteExposureItems(params PastebinAPI.Visibility[] items)
        {
            PasteExposure.Items.Clear();
            foreach (var visibility in items)
            {
                PasteExposure.Items.Add(visibility);
            }
        }

        private async void PasteOnClick(Object sender, RoutedEventArgs e)
        {
            using (ButtonEnableWrapper wrapper = new ButtonEnableWrapper(PasteButton))
            {
                PastebinHelper.ApplyDevKey();
                try
                {
                    Paste paste;
                    if(AuthorizationManager.Authorized && !(Boolean)PasteAsAGuestCheckBox.IsChecked)
                        paste = await AuthorizationManager.CurrentUser.CreatePasteAsync(
                            ((Run)PasteText.Inlines.FirstInline).Text,
                            PasteName.Text,
                            PastebinAPI.Language.Parse(SyntaxHighlighting.Text.ToLower()),
                            (PastebinAPI.Visibility)Enum.Parse(typeof(PastebinAPI.Visibility), PasteExposure.Text),
                            PastebinHelper.StringToExpirationDictionary[PasteExpiration.Text]);
                    else
                        paste = await Paste.CreateAsync(
                            ((Run)PasteText.Inlines.FirstInline).Text,
                            PasteName.Text,
                            PastebinAPI.Language.Parse(SyntaxHighlighting.Text.ToLower()),
                            (PastebinAPI.Visibility)Enum.Parse(typeof(PastebinAPI.Visibility), PasteExposure.Text),
                            PastebinHelper.StringToExpirationDictionary[PasteExpiration.Text]);
                    if(paste.Url.StartsWith("https://pastebin.com/"))
                        PasteUrl.Text = paste.Url;
                    else
                        MessageBox.Show(paste.Url, PastebinHelper.DefaultCaption);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, PastebinHelper.DefaultCaption);
                }
            }
        }

        private void AuthorizationOnClick(Object sender, RoutedEventArgs e)
        {
            using (ButtonEnableWrapper wrapper = new ButtonEnableWrapper(AuthorizationButton))
            {
                if(AuthorizationManager.Authorized)
                    AuthorizationManager.LogOut();
                else
                    new SignInWindow().ShowModal();
                ApplyAuthorizedUserAbilities();
            }
        }
    }
}
