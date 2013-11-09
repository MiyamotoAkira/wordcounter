namespace WordCounterCore
{
  /// <summary>
  /// Naive splitter of words. Uses spaces as separator. 
  /// </summary>
  public class BasicSplit : ISplitStrategy
  {
    public string[] SplitText(string text)
    {
      return text.Split(' ');
    }
  }
}
