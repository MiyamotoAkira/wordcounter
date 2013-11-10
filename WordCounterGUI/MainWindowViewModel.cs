
namespace WordCounterGUI
{
  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.IO;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Input;
  using Microsoft.Win32;
  using WordCounterCore;

  public class MainWindowViewModel : INotifyPropertyChanged
  {
    #region Member Variables
    private ICommand exitCommand;
    private ICommand analyzeCommand;
    private ICommand saveResults;
    private string sourceFileResults;
    private string compareFileResults;
    private Visibility sourceFileVisible;
    private Visibility compareFileVisible;
    private GridLength sourceFileColumnWidth;
    private GridLength compareFileColumnWidth;
    private WordCounterInformation resultsSourceFile;
    #endregion

    #region Constructors
    public MainWindowViewModel()
    {
      this.SourceFileVisible = Visibility.Collapsed;
      this.CompareFileVisible = Visibility.Collapsed;
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
      OpenFileDialog dialog = new OpenFileDialog();
      dialog.ShowDialog();
      string fileToAnalyze = dialog.FileName;
      if (!string.IsNullOrWhiteSpace(fileToAnalyze))
      {
        WordCounter wordCounter = new WordCounter(new ComplexSplit());
        StreamReader reader = new StreamReader(fileToAnalyze);
        this.resultsSourceFile = wordCounter.CountWordsOnStreamText(reader);
        this.SourceFileResults = this.resultsSourceFile.ToString();
      }
    }

    private void OnSaveResultsCommand()
    {
      SaveFileDialog dialog = new SaveFileDialog();
      dialog.ShowDialog();
      string saveFileName = dialog.FileName;
      if (!string.IsNullOrWhiteSpace(saveFileName))
      {
        this.resultsSourceFile.SerializeToFile(saveFileName);
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
