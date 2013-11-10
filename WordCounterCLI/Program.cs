
namespace WordCounterCLI
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using WordCounterCore;

  public class Program
  {
    public static void Main(string[] args)
    {
      LineArguments lineArguments = new LineArguments();
      if (lineArguments.Parse(args))
      {
        ProcessWordCounter(lineArguments);
      }
      else
      {
        DisplayUsage();
      }
    }

    private static void ProcessWordCounter(LineArguments lineArguments)
    {
      WordCounterInformation results;
      int? resultComparison = null;
      using (StreamReader reader = new StreamReader(lineArguments.SourceFile))
      {
        WordCounter wordCounter = new WordCounter(new ComplexSplit());
        results = wordCounter.CountWordsOnStreamText(reader);
      }

      WordCounterInformation compareTo = null;
      if (lineArguments.CompareTo)
      {
        using (StreamReader reader = new StreamReader(lineArguments.SourceFileToCompare))
        {
          WordCounter wordCounter = new WordCounter(new ComplexSplit());
          compareTo = wordCounter.CountWordsOnStreamText(reader);

          resultComparison = results.CompareTo(compareTo);
        }
      }

      if (results != null)
      {
        if (lineArguments.ThereIsFileOutput)
        {
          results.SerializeToFile(lineArguments.DestinationPath);
        }
        else
        {
          Console.WriteLine(results.ToString());
        }
      }

      if (compareTo != null)
      {
        if (lineArguments.ThereIsFileOutputForCompare)
        {
          compareTo.SerializeToFile(lineArguments.DestinationPathForCompare);
        }
        else
        {
          Console.WriteLine(compareTo.ToString());
        }
      }

      if (resultComparison.HasValue)
      {
        if (resultComparison.Value == 0)
        {
          Console.WriteLine("The files have the same words");
        }
        else
        {
          Console.WriteLine("The files are different");
        }
      }
    }

    private static void DisplayUsage()
    {
      Console.WriteLine(String.Empty);
      Console.WriteLine("Usage of WordCounter:");
      Console.WriteLine("wordcounter sourcefile [/compareto filetocompare] [/output destinationpath]");
      Console.WriteLine(String.Empty);
      Console.WriteLine("sourcefile is the file for which we want to count words for");
      Console.WriteLine(String.Empty);
      Console.WriteLine("/compareto filetocompare compares the word count of filetocompare to the sourcefile");
      Console.WriteLine(String.Empty);
      Console.WriteLine("/output destinationpath saves the word count information to destinationpath instead of displaying on screen");
      Console.WriteLine(String.Empty);
      Console.WriteLine("/outputcompare destinationpath saves the word count information of the compare file to destinationpath instead of displaying on screen");
    }
  }
}
