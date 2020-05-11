using System;
using System.ComponentModel.Design;
using System.Windows;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace VSPastebinExtension
{
    internal sealed class MainCommand
    {
        public const Int32 CommandId = 0x0100;
        
        public static readonly Guid CommandSet = new Guid("c09c7394-49db-45eb-af9d-692690d35cb8");

        private readonly AsyncPackage package;

        private readonly MenuCommand attachedMenuCommand;

        public static MainCommand Instance { get; private set; }

        private IAsyncServiceProvider ServiceProvider => package;

        private MainCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(Execute, menuCommandId);
            attachedMenuCommand = menuItem;
            commandService.AddCommand(menuItem);
        }

        public static async Task InitializeAsync(AsyncPackage package)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new MainCommand(package, commandService);
        }

        private async void Execute(Object sender, EventArgs e)
        {
            using (MenuCommandEnableWrapper wrapper = new MenuCommandEnableWrapper(attachedMenuCommand))
            {
                DocumentModel model = await DocumentModel.GetActiveDocumentAsync(package) ?? DocumentModel.GetEmpty();
                new MainWindow(model).ShowModal();
            }
        }
    }
}
