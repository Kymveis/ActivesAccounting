using System.IO;

using Microsoft.Win32;

namespace ActivesAccounting;

internal static class DialogUtils
{
    private const string EXTENSION = "aacco";

    public static bool TryOpenFile(out FileInfo? aFile) =>
        tryGetDialogFile(new OpenFileDialog {CheckFileExists = true}, out aFile);

    public static bool TrySaveFile(out FileInfo? aFile) =>
        tryGetDialogFile(new SaveFileDialog {CheckFileExists = false}, out aFile);

    private static bool tryGetDialogFile(FileDialog aDialog, out FileInfo? aFile)
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