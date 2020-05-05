using System;
using System.ComponentModel.Design;

namespace VSPastebinExtension
{
    public class MenuCommandEnableWrapper : IDisposable
    {
        private readonly MenuCommand _menuCommand;

        public MenuCommandEnableWrapper(MenuCommand menuCommand)
        {
            _menuCommand = menuCommand;
            _menuCommand.Enabled = false;
        }

        public void Dispose()
        {
            _menuCommand.Enabled = true;
        }
    }
}
