
namespace WordCounterGUI
{
  using System;
  using System.ComponentModel;
  using System.IO;
  using System.Linq.Expressions;
  using System.Windows;
  using System.Windows.Input;
  using WordCounterCore;

  public class MainWindowViewModel : INotifyPropertyChanged
  {
    #region Member Variables
    private const string JsonExtension = ".json";
    private const string JsonFilter = "Json Files (.json)|*.json";
    private ICommand exitCommand;
    private ICommand analyzeCommand;
    private ICommand saveResults;
    private ICommand openResults;
    private ICommand compareCommand;
    private string sourceFileResults;
    private string compareFileResults;
    private Visibility sourceFileVisible;
    private Visibility compareFileVisible;
    private GridLength sourceFileColumnWidth;
    private GridLength compareFileColumnWidth;
    private WordCounterInformation resultsSourceFile;
    private WordCounterInformation resultsCompareFile;
    private IDialog openDialog;
    private IDialog saveDialog;
    #endregion

    #region Constructors
    public MainWindowViewModel()
      : this(new OpenDialog(), new SaveDialog())
    {
    }

    public MainWindowViewModel(IDialog openDialog, IDialog saveDialog)
    {
      this.SourceFileVisible = Visibility.Collapsed;
      this.CompareFileVisible = Visibility.Collapsed;
      this.openDialog = openDialog;
      this.saveDialog = saveDialog;
    }
    #endregion

    #region Events
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region Properties
    public ICommand ExitCommand
    {
      get
      {
        if (this.exitCommand == null)
        {
          this.exitCommand = new RelayCommand<String>(param => this.OnExitCommand());
        }

        return this.exitCommand;
      }
    }

    public ICommand AnalyzeCommand
    {
      get
      {
        if (this.analyzeCommand == null)
        {
          this.analyzeCommand = new RelayCommand<String>(param => this.OnAnalyzeCommand());
        }

        return this.analyzeCommand;
      }
    }

    public ICommand CompareCommand
    {
      get
      {
        if (this.compareCommand == null)
        {
          this.compareCommand = new RelayCommand<String>(param => this.OnCompareCommand());
        }

        return this.compareCommand;
      }
    }

    public ICommand SaveResults
    {
      get
      {
        if (this.saveResults == null)
        {
          this.saveResults = new RelayCommand<String>(param => this.OnSaveResultsCommand());
        }

        return this.saveResults;
      }
    }

    public ICommand OpenResults
    {
      get
      {
        if (this.openResults == null)
        {
          this.openResults = new RelayCommand<String>(param => this.OnOpenResultsCommand());
        }

        return this.openResults;
      }
    }

    public Visibility SourceFileVisible
    {
      get
      {
        return this.sourceFileVisible;
      }

      set
      {
        this.sourceFileVisible = value;
        this.OnPropertyChanged(() => this.SourceFileVisible);
        this.ModifyColumnWidths();
      }
    }

    public Visibility CompareFileVisible
    {
      get
      {
        return this.compareFileVisible;
      }

      set
      {
        this.compareFileVisible = value;
        this.OnPropertyChanged(() => this.CompareFileVisible);
        this.ModifyColumnWidths();
      }
    }

    public GridLength SourceFileColumnWidth
    {
      get
      {
        return this.sourceFileColumnWidth;
      }

      set
      {
        this.sourceFileColumnWidth = value;
        this.OnPropertyChanged(() => this.SourceFileColumnWidth);
      }
    }

    public GridLength CompareFileColumnWidth
    {
      get
      {
        return this.compareFileColumnWidth;
      }

      set
      {
        this.compareFileColumnWidth = value;
        this.OnPropertyChanged(() => this.CompareFileColumnWidth);
      }
    }

    public String SourceFileResults
    {
      get
      {
        return this.sourceFileResults;
      }

      set
      {
        this.sourceFileResults = value;
        this.OnPropertyChanged(() => this.SourceFileResults);
        this.ModifySourceFileVisibility();
      }
    }

    public String CompareFileResults
    {
      get
      {
        return this.compareFileResults;
      }

      set
      {
        this.compareFileResults = value;
        this.OnPropertyChanged(() => this.CompareFileResults);
        this.ModifyCompareFileVisibility();
      }
    }

    /// <summary>
    ///   Gets a value indicating whether [data changed].
    /// </summary>
    /// <value>
    ///   <c>true</c> if [data changed]; otherwise, <c>false</c>.
    /// </value>
    public Boolean DataChanged { get; private set; }
    #endregion


    #region Methods
    private void OnExitCommand()
    {
      Application.Current.Shutdown();
    }

    private void OnAnalyzeCommand()
    {
      string fileToAnalyze = this.openDialog.ChooseFile("File To Analyze");
      if (!string.IsNullOrWhiteSpace(fileToAnalyze))
      {
        WordCounter wordCounter = new WordCounter(new ComplexSplit());
        StreamReader reader = new StreamReader(fileToAnalyze);
        this.resultsSourceFile = wordCounter.CountWordsOnStreamText(reader);
        this.SourceFileResults = this.resultsSourceFile.ToString();
      }
    }

    private void OnCompareCommand()
    {
      string fileToAnalyze = this.openDialog.ChooseFile("File To Analyze");
      if (!string.IsNullOrWhiteSpace(fileToAnalyze))
      {
        WordCounter wordCounter = new WordCounter(new ComplexSplit());
        StreamReader reader = new StreamReader(fileToAnalyze);
        this.resultsSourceFile = wordCounter.CountWordsOnStreamText(reader);
        this.SourceFileResults = this.resultsSourceFile.ToString();
      }

      fileToAnalyze = this.openDialog.ChooseFile("File To Compare To");
      if (!string.IsNullOrWhiteSpace(fileToAnalyze))
      {
        WordCounter wordCounter = new WordCounter(new ComplexSplit());
        StreamReader reader = new StreamReader(fileToAnalyze);
        this.resultsCompareFile = wordCounter.CountWordsOnStreamText(reader);
        this.CompareFileResults = this.resultsCompareFile.ToString();
      }
    }

    private void OnSaveResultsCommand()
    {
      string saveFileName = this.saveDialog.ChooseFile("Save results", JsonExtension, JsonFilter);
      if (!string.IsNullOrWhiteSpace(saveFileName))
      {
        this.resultsSourceFile.SerializeToFile(saveFileName);
      }
    }

    private void OnOpenResultsCommand()
    {
      string fileToAnalyze = this.openDialog.ChooseFile("Open Results", JsonExtension, JsonFilter);
      if (!string.IsNullOrWhiteSpace(fileToAnalyze))
      {
        using (StreamReader reader = new StreamReader(fileToAnalyze))
        {
          this.resultsSourceFile = WordCounterInformation.DeSerializeFromReader(reader);
          this.SourceFileResults = this.resultsSourceFile.ToString();
        }
      }
    }

    /// <summary>
    /// The on property changed.
    /// </summary>
    /// <param name="property"> The property. </param>
    /// <typeparam name="T"> Property type </typeparam>
    private void OnPropertyChanged<T>(Expression<Func<T>> property)
    {
      String propertyName = (property.Body as MemberExpression).Member.Name;

      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        this.DataChanged = true;
      }
    }

    /// <summary>
    /// Invokes the event property changed.
    /// </summary>
    /// <param name="propertyName"> The property for which we want to specify we have made a change. </param>
    private void RaisePropertyChanged(String propertyName)
    {
      if (this.PropertyChanged != null)
      {
        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
      }
    }

    private void ModifyColumnWidths()
    {
      int numberOfColumnsToShow = 0;
      if (this.SourceFileVisible != Visibility.Collapsed)
      {
        numberOfColumnsToShow++;
      }

      if (this.CompareFileVisible != Visibility.Collapsed)
      {
        numberOfColumnsToShow++;
      }

      if (numberOfColumnsToShow == 0)
      {
        return;
      }

      Double percentage = 10 / numberOfColumnsToShow;

      if (this.SourceFileVisible != Visibility.Collapsed)
      {
        this.SourceFileColumnWidth = new GridLength(percentage, GridUnitType.Star);
      }

      if (this.CompareFileVisible != Visibility.Collapsed)
      {
        this.CompareFileColumnWidth = new GridLength(percentage, GridUnitType.Star);
      }
    }

    private void ModifySourceFileVisibility()
    {
      if (String.IsNullOrWhiteSpace(this.sourceFileResults))
      {
        this.SourceFileVisible = Visibility.Collapsed;
      }
      else
      {
        this.SourceFileVisible = Visibility.Visible;
      }
    }

    private void ModifyCompareFileVisibility()
    {
      if (String.IsNullOrWhiteSpace(this.compareFileResults))
      {
        this.CompareFileVisible = Visibility.Collapsed;
      }
      else
      {
        this.CompareFileVisible = Visibility.Visible;
      }
    }
    #endregion
  }
}
