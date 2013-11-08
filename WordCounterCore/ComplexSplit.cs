
namespace WordCounterCore
{
  using System.Text.RegularExpressions;

  public class ComplexSplit : ISplitStrategy
  {
    public string[] SplitText(string text)
    {
      Regex regex = new Regex(@"\W");
      return regex.Split(text);
    }
  }
}
