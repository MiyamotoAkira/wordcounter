namespace WordCounterCore
{
  /// <summary>
  /// Naive word counter. Uses spaces as separator. 
  /// </summary>
  public class BasicSplit : ISplitStrategy
  {
    public string[] SplitText(string text)
    {
      return text.Split(' ');
    }
  }
}
