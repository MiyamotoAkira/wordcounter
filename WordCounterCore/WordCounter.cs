
namespace WordCounterCore
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;

  public class WordCounter
  {
    private ISplitStrategy strategy;
    private WordCounterInformation results;

    public WordCounter(ISplitStrategy strategy)
    {
      this.strategy = strategy;
    }

    public WordCounterInformation CountWordsOnText(string text)
    {
      this.results = new WordCounterInformation();

      this.CountWordsOnSingleLine(text);

      return results;
    }

    public WordCounterInformation CountWordsOnStreamText(StreamReader reader)
    {
      this.results = new WordCounterInformation();
      String line;
      while ((line = reader.ReadLine()) != null)
      {
        this.CountWordsOnSingleLine(line);
      }

      return results;
    }

    private void CountWordsOnSingleLine(string text)
    {
      if (String.IsNullOrWhiteSpace(text))
      {
        return;
      }

      string[] operands = this.strategy.SplitText(text);

      operands.ToList().ForEach(this.results.Add);
    }


  }
}
