
namespace WordCounterGUI
{
  using Microsoft.Win32;

  public class OpenDialog : IDialog
  {
    public string ChooseFile(string title, string defaultExtension = "*.*", string filter = "All files (*.*)|*.*")
    {
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.Title = title;
      dialog.AddExtension = true;
      dialog.DefaultExt = defaultExtension;
      dialog.Filter = filter;
      dialog.ShowDialog();

      return dialog.FileName;
    }
  }
}
