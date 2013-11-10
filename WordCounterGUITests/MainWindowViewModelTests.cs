

namespace WordCounterGUITests
{
  using FluentAssertions;
  using NSubstitute;
  using NUnit.Framework;
  using WordCounterGUI;

  [TestFixture]
  public class MainWindowViewModelTests
  {
    
    public void OnAnalyzeCommand_NoFileSelected_NothingHasBeenSet()
    {
      IDialog openDialog = Substitute.For<IDialog>();
      IDialog saveDialog = Substitute.For<IDialog>();
      MainWindowViewModel viewModel = new MainWindowViewModel(openDialog, saveDialog);
      viewModel.OnAnalyzeCommand();
    }
  }
}
