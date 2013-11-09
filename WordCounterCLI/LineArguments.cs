
namespace WordCounterCLI
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class LineArguments
  {
    public string SourceFile { get; private set; }
    public bool CompareTo { get; private set; }
    public string SourceFileToCompare { get; private set; }
    public bool ThereIsFileOutput { get; private set; }
    public string DestinationPath { get; private set; }


    public bool Parse(string[] arguments)
    {
      if (arguments == null || arguments.Length == 0)
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

  }
}
