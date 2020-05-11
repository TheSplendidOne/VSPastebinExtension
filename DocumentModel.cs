using System;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;

namespace VSPastebinExtension
{
    public class DocumentModel
    {
        public String Name { get; }

        public String Language { get; }

        public String Text { get; }

        private DocumentModel(String name, String language, String text)
        {
            Name = name;
            Language = language;
            Text = text;
        }

        public static async Task<DocumentModel> GetActiveDocumentAsync(AsyncPackage package)
        {
            try
            {
                await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);
                DTE dte = (DTE) await package.GetServiceAsync(typeof(DTE));
                return dte.ActiveDocument == null
                    ? null
                    : new DocumentModel(dte.ActiveDocument.Name, dte.ActiveDocument.Language, GetCurrentTextFile(dte.ActiveDocument));
            }
            catch
            {
                return null;
            }
        }

        public static DocumentModel GetEmpty()
        {
            return new DocumentModel(String.Empty, String.Empty, String.Empty);
        }

        private static String GetCurrentTextFile(Document document)
        {
            ThreadHelper.ThrowIfNotOnUIThread(nameof(GetCurrentTextFile));
            TextDocument textDocument = (TextDocument)document.Object("TextDocument");
            return textDocument.StartPoint.CreateEditPoint().GetText(textDocument.EndPoint);
        }
    }
}
