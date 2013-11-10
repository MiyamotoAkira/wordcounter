
namespace WordCounterGUI
{
  using Microsoft.Win32;

  class SaveDialog : IDialog
  {
    #region IDialog Members

    public string ChooseFile(string title, string defaultExtension = "*.*", string filter = "All files (*.*)|*.*")
    {
      SaveFileDialog dialog = new SaveFileDialog();
      dialog.Title = title;
      dialog.AddExtension = true;
      dialog.DefaultExt = defaultExtension;
      dialog.Filter = filter;
      dialog.ShowDialog();

      return dialog.FileName;
    }

    #endregion
  }
}
