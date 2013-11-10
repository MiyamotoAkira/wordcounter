
namespace WordCounterGUI
{
  public interface IDialog
  {
    string ChooseFile(string title, string defaultExtension = "*.*", string filter = "All files (*.*)|*.*");
  }
}
