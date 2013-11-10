namespace WordCounterCLI
{
  public interface ILineArguments
  {
    /// <summary>
    /// Gets the path of the file for which we want to count the words.
    /// </summary>
    string SourceFile { get; }

    /// <summary>
    /// Gets a value indicating whether we want to compare the file to another file
    /// </summary>
    bool CompareTo { get; }

    /// <summary>
    /// Gets the path of the file to which we want to compare
    /// </summary>
    string SourceFileToCompare { get; }

    /// <summary>
    /// Gets a value indicating whether we want to write the output to a file or not.
    /// </summary>
    bool ThereIsFileOutput { get; }

    /// <summary>
    /// Gets the path of the file where we want to output the results.
    /// </summary>
    string DestinationPath { get; }

    /// <summary>
    /// Parses the arguments to fill the possible options.
    /// </summary>
    /// <param name="arguments">The arguments that we want to parse.</param>
    /// <returns>True if the parsing finished correctly. False if there is any issue with the arguments passed.</returns>
    bool Parse(string[] arguments);
  }
}