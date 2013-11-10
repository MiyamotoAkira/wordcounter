
namespace WordCounterCLI
{
  using Extensions;

  public class LineArguments : ILineArguments
  {
    #region Properties
    /// <summary>
    /// Gets the path of the file for which we want to count the words.
    /// </summary>
    public string SourceFile { get; private set; }

    /// <summary>
    /// Gets a value indicating whether we want to compare the file to another file
    /// </summary>
    public bool CompareTo { get; private set; }

    /// <summary>
    /// Gets the path of the file to which we want to compare
    /// </summary>
    public string SourceFileToCompare { get; private set; }

    /// <summary>
    /// Gets a value indicating whether we want to write the output to a file or not.
    /// </summary>
    public bool ThereIsFileOutput { get; private set; }

    /// <summary>
    /// Gets the path of the file where we want to output the results.
    /// </summary>
    public string DestinationPath { get; private set; }
    #endregion

    #region Methods
    /// <summary>
    /// Parses the arguments to fill the possible options.
    /// </summary>
    /// <param name="arguments">The arguments that we want to parse.</param>
    /// <returns>True if the parsing finished correctly. False if there is any issue with the arguments passed.</returns>
    public bool Parse(string[] arguments)
    {
      if (arguments.IsNullOrEmpty())
      {
        return false;
      }

      if (this.CheckArgumentIsOption(arguments, 0))
      {
        return false;
      }

      this.SourceFile = arguments[0];

      for (int currentArgument = 1; currentArgument < arguments.Length; currentArgument += 2)
      {
        int nextArgument = currentArgument + 1;

        switch (arguments[currentArgument])
        {
          case "/compareto":
            if (!CheckThereIsNextArgument(arguments, nextArgument))
            {
              return false;
            }

            if (this.CheckArgumentIsOption(arguments, nextArgument))
            {
              return false;
            }

            this.CompareTo = true;
            this.SourceFileToCompare = arguments[nextArgument];

            break;
          case "/destinationpath":
            if (!CheckThereIsNextArgument(arguments, nextArgument))
            {
              return false;
            }

            if (this.CheckArgumentIsOption(arguments, nextArgument))
            {
              return false;
            }

            this.ThereIsFileOutput = true;
            this.DestinationPath = arguments[nextArgument];
            break;
          default:
            return false;
        }
      }

      return true;
    }

    private bool CheckThereIsNextArgument(string[] arguments, int nextArgument)
    {
      return arguments.Length > nextArgument;
    }

    private bool CheckArgumentIsOption(string[] arguments, int argumentIndex)
    {
      return arguments[argumentIndex].StartsWith("/");
    }
    #endregion
  }
}
