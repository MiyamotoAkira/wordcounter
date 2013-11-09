
namespace WordCounterCore
{
  using System.Text.RegularExpressions;

  /// <summary>
  /// A slightly better separator. Uses regex.
  /// </summary>
  public class ComplexSplit : ISplitStrategy
  {
    public string[] SplitText(string text)
    {
      Regex regex = new Regex(@"\W");
      return regex.Split(text);
    }
  }
}
