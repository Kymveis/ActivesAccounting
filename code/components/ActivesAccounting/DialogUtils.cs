using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;

using Microsoft.Win32;

namespace ActivesAccounting;

internal static class DialogUtils
{
    private const string EXTENSION = "aacco";

    public static bool TryOpenFile([MaybeNullWhen(false)] out FileInfo aFile) =>
        tryGetDialogFile(new OpenFileDialog {CheckFileExists = true}, out aFile);

    public static bool TrySaveFile([MaybeNullWhen(false)] out FileInfo aFile) =>
        tryGetDialogFile(new SaveFileDialog {CheckFileExists = false}, out aFile);

    public static bool Ask(string aTitle, string aQuestion) =>
        MessageBox.Show(aQuestion, aTitle, MessageBoxButton.YesNo, MessageBoxImage.Question) is MessageBoxResult.Yes;

    public static void Error(string aTitle, string aText) =>
        MessageBox.Show(aText, aTitle, MessageBoxButton.OK, MessageBoxImage.Error);

    private static bool tryGetDialogFile(FileDialog aDialog, [MaybeNullWhen(false)] out FileInfo aFile)
    {
        initializeDialog();

        if (aDialog.ShowDialog() is true)
        {
            aFile = new FileInfo(aDialog.FileName);
            return true;
        }

        aFile = default;
        return false;

        void initializeDialog()
        {
            aDialog.Title = "Select file";
            aDialog.DefaultExt = $".{EXTENSION}";
            aDialog.Filter = $"Actives accounting files (.{EXTENSION})|*.{EXTENSION}";
        }
    }
}