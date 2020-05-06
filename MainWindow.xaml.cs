using System;
using System.Windows;
using Microsoft.VisualStudio.PlatformUI;
using PastebinAPI;

namespace VSPastebinExtension
{
    public partial class MainWindow : DialogWindow
    {
        private readonly String _text;

        public MainWindow(DocumentModel model)
        {
            InitializeComponent();
            InitializeComboBoxes();
            PasteName.Text = model.Name;
            PasteExposure.Text = PastebinHelper.ExpirationToStringDictionary[Expiration.OneMonth];
            SyntaxHighlighting.Text = PastebinHelper.IdentifyLanguage(model.Language).ToUpper();
            _text = model.Text;
        }

        private void InitializeComboBoxes()
        {
            foreach(var language in PastebinAPI.Language.All)
            {
                SyntaxHighlighting.Items.Add(language.ToString().ToUpper());
            }
            foreach (var expiration in PastebinHelper.StringToExpirationDictionary.Keys)
            {
                PasteExposure.Items.Add(expiration);
            }
        }

        private async void PasteOnClick(Object sender, RoutedEventArgs e)
        {
            using (ButtonEnableWrapper wrapper = new ButtonEnableWrapper(PasteButton))
            {
                Pastebin.DevKey = "4c8e82e3e03bef45028b783ecc14ace5";
                Paste paste = await Paste.CreateAsync(
                    _text,
                    PasteName.Text,
                    PastebinAPI.Language.Parse(SyntaxHighlighting.Text.ToLower()),
                    PastebinAPI.Visibility.Public,
                    PastebinHelper.StringToExpirationDictionary[PasteExposure.Text]);
                if (paste.Url.StartsWith("https://pastebin.com/"))
                    PasteUrl.Text = paste.Url;
                else
                    MessageBox.Show(paste.Url, "Pastebin Extension");
            }
        }
    }
}
