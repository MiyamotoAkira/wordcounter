/// Based on the Relay Command by Josh Smith

namespace WordCounterGUI
{
  #region

  using System;
  using System.Diagnostics;
  using System.Threading;
  using System.Windows.Input;

  #endregion

  /// <summary>
  /// Definition of RelayCommand. Used for ...
  /// </summary>
  /// <typeparam name="T">
  /// The type of the parameter we are passing to the command.
  /// </typeparam>
  public class RelayCommand<T> : ICommand
  {
    #region Fields

    /// <summary>
    ///   The action that holds if the command is available or not.
    /// </summary>
    private readonly Predicate<T> canExecute;

    /// <summary>
    ///   The action that holds the command to execute.
    /// </summary>
    private readonly Action<T> execute;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class. 
    /// Initializes a new instance of the RelayCommand class.
    /// </summary>
    /// <param name="execute">
    /// The action delegate we want to execute when invoking the command.
    /// </param>
    public RelayCommand(Action<T> execute)
      : this(execute, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class. 
    /// Initializes a new instance of the RelayCommand class.
    /// </summary>
    /// <param name="execute">
    /// The execution logic.
    /// </param>
    /// <param name="canExecute">
    /// The execution status logic.
    /// </param>
    public RelayCommand(Action<T> execute, Predicate<T> canExecute)
    {
      if (execute == null)
      {
        throw new ArgumentNullException("execute");
      }

      this.execute = execute;
      this.canExecute = canExecute;
    }

    #endregion

    #region Public Events

    /// <summary>
    ///   Event raised when the CanExecute has change its value.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add
      {
        CommandManager.RequerySuggested += value;
      }

      remove
      {
        CommandManager.RequerySuggested -= value;
      }
    }

    #endregion

    #region Events

    /// <summary>
    ///   Event raised when the CanExecute has change its value.
    /// </summary>
    private event EventHandler LocalCanExecuteChanged;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// Checks if we can execute this command.
    /// </summary>
    /// <param name="parameter">
    /// The parameter we are passing to the delegate.
    /// </param>
    /// <returns>
    /// True if we can execute the command. False otherwise.
    /// </returns>
    [DebuggerStepThrough]
    public Boolean CanExecute(Object parameter)
    {
      return this.canExecute == null ? true : this.canExecute((T)parameter);
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter">
    /// The parameter we want to pass to the delegate.
    /// </param>
    public void Execute(Object parameter)
    {
      this.execute((T)parameter);
    }

    /// <summary>
    /// The fire can execute changed.
    /// </summary>
    public void FireCanExecuteChanged()
    {
      if (this.LocalCanExecuteChanged != null)
      {
        this.LocalCanExecuteChanged(this, null);
      }
    }

    #endregion
  }

  /// <summary>
  ///   The class for the command definitions
  /// </summary>
  public class RelayCommand : ICommand
  {
    #region Fields

    /// <summary>
    ///   The action that holds if the command is available or not.
    /// </summary>
    private readonly Predicate<Object> canExecute;

    /// <summary>
    ///   The action that holds the command to execute.
    /// </summary>
    private readonly Action<Object> execute;

    #endregion

    #region Constructors and Destructors

    /// <summary>
    /// Initializes a new instance of the RelayCommand class.
    /// </summary>
    /// <param name="execute">
    /// The action delegate we want to execute when invoking the command.
    /// </param>
    public RelayCommand(Action<Object> execute)
      : this(execute, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the RelayCommand class.
    /// </summary>
    /// <param name="execute">
    /// The execution logic.
    /// </param>
    /// <param name="canExecute">
    /// The execution status logic.
    /// </param>
    public RelayCommand(Action<Object> execute, Predicate<Object> canExecute)
    {
      if (execute == null)
      {
        throw new ArgumentNullException("execute");
      }

      this.execute = execute;
      this.canExecute = canExecute;
    }

    #endregion

    #region Public Events

    /// <summary>
    ///   Event raised when the CanExecute has change its value.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add
      {
        if (this.canExecute != null)
        {
          EventHandler handler2;
          EventHandler canExecuteChanged = this.LocalCanExecuteChanged;
          do
          {
            handler2 = canExecuteChanged;
            EventHandler handler3 = (EventHandler)Delegate.Combine(handler2, value);
            canExecuteChanged = Interlocked.CompareExchange(ref this.LocalCanExecuteChanged, handler3, handler2);
          }
          while (canExecuteChanged != handler2);

          CommandManager.RequerySuggested += value;
        }
      }

      remove
      {
        if (this.canExecute != null)
        {
          EventHandler handler2;
          EventHandler canExecuteChanged = this.LocalCanExecuteChanged;
          do
          {
            handler2 = canExecuteChanged;
            EventHandler handler3 = (EventHandler)Delegate.Remove(handler2, value);
            canExecuteChanged = Interlocked.CompareExchange(ref this.LocalCanExecuteChanged, handler3, handler2);
          }
          while (canExecuteChanged != handler2);

          CommandManager.RequerySuggested -= value;
        }
      }
    }

    #endregion

    #region Events

    /// <summary>
    ///   Event raised when the CanExecute has change its value.
    /// </summary>
    private event EventHandler LocalCanExecuteChanged;

    #endregion

    #region Public Methods and Operators

    /// <summary>
    /// Checks if we can execute this command.
    /// </summary>
    /// <param name="parameter">
    /// The parameter we are passing to the delegate.
    /// </param>
    /// <returns>
    /// True if we can execute the command. False otherwise.
    /// </returns>
    [DebuggerStepThrough]
    public Boolean CanExecute(Object parameter)
    {
      return this.canExecute == null ? true : this.canExecute(parameter);
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter">
    /// The parameter we want to pass to the delegate.
    /// </param>
    public void Execute(Object parameter)
    {
      this.execute(parameter);
    }

    /// <summary>
    /// The fire can execute changed.
    /// </summary>
    public void FireCanExecuteChanged()
    {
      if (this.LocalCanExecuteChanged != null)
      {
        this.LocalCanExecuteChanged(this, null);
      }
    }

    #endregion
  }
}
