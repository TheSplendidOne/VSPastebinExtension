using System;
using System.Windows.Controls;

namespace VSPastebinExtension
{
    public class ButtonEnableWrapper : IDisposable
    {
        private readonly Button _button;

        public ButtonEnableWrapper(Button button)
        {
            _button = button;
            _button.IsEnabled = false;
        }

        public void Dispose()
        {
            _button.IsEnabled = true;
        }
    }
}
