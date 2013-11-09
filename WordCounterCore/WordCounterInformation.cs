
namespace WordCounterCore
{
  using System;
  using System.Collections;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Linq;
  using Newtonsoft.Json.Schema;

  public class WordCounterInformation : IComparable, IComparable<WordCounterInformation>, ICollection
  {
    ////    static JsonSchema schema = JsonSchema.Parse(@"{
    ////  'type': 'object',
    ////  'properties': {
    ////    'name': {'type':'string'},
    ////    'hobbies': {'type': 'array'}
    ////  }
    ////}");

    ////    string schemaJson = @"{
    //// 2  'description': 'WordCounter',
    //// 3  'type': 'object',
    //// 4  'properties':
    //// 5  {
    //// 6    'name': {'type':'string'},
    //// 7    'CountedWords': {
    //// 8      'type': 'array',
    //// 9      'word': {'type':'string'}
    ////        'counter':
    ////10    }
    ////11  }
    ////12}";
    #region Member Variables
    private SortedDictionary<string, int> countedWords;
    #endregion

    #region Constructor
    public WordCounterInformation()
    {
      countedWords = new SortedDictionary<string, int>();
    }
    #endregion

    #region properties
    public string FileName { get; set; }

    public int TotalWords
    {
      get
      {
        return this.countedWords != null && this.countedWords.Count > 0
          ? this.countedWords.Values.ToList().Aggregate((total, value) => total += value)
          : 0;
      }
    }
    #endregion

    ////public static bool ValidateAgainstSchema(String textToValidate)
    ////{
    ////  JsonSchemaType.Array

    ////  JObject possibleCountedWord = JObject.Parse(textToValidate);

    ////  bool valid = possibleCountedWord.IsValid(schema);
    ////}

    #region Methods
    public static WordCounterInformation DeSerializeFromString(String serializedObject)
    {
      return JsonConvert.DeserializeObject<WordCounterInformation>(serializedObject);
    }

    public string SerializeToString()
    {
      return JsonConvert.SerializeObject(this);
    }

    public void SerializeToFile(String filePathAndName)
    {

    }

    public void Add(string operand)
    {
      if (String.IsNullOrWhiteSpace(operand))
      {
        return;
      }

      if (this.countedWords.ContainsKey(operand))
      {
        this.countedWords[operand]++;
      }
      else
      {
        this.countedWords.Add(operand, 1);
      }
    }

    public int CompareTo(object obj)
    {
      if (obj is WordCounterInformation)
      {
        return this.CompareTo(obj as WordCounterInformation);
      }
      else
      {
        throw new InvalidCastException("The object passed is not of type " + this.GetType().ToString());
      }
    }

    public int CompareTo(WordCounterInformation other)
    {
      if (this == other)
      {
        return 0;
      }

      if (this.countedWords == null && other.countedWords == null)
      {
        return 0;
      }

      if (this.countedWords == null)
      {
        return -1;
      }

      if (other.countedWords == null)
      {
        return 1;
      }

      if (this.countedWords.Count != other.countedWords.Count)
      {
        return this.countedWords.Count.CompareTo(other.countedWords.Count);
      }

      for (int currentWordIndex = 0; currentWordIndex < this.countedWords.Keys.Count; currentWordIndex++)
      {
        if (!this.countedWords.Keys.ToList()[currentWordIndex].Equals(other.countedWords.Keys.ToList()[currentWordIndex]))
        {
          return this.countedWords.Keys.ToList()[currentWordIndex].CompareTo(other.countedWords.Keys.ToList()[currentWordIndex]);
        }
      }

      for (int currentWordIndex = 0; currentWordIndex < this.countedWords.Keys.Count; currentWordIndex++)
      {
        string firstKey = this.countedWords.Keys.ToList()[currentWordIndex];
        string secondKey = other.countedWords.Keys.ToList()[currentWordIndex];

        int comparisonResult = this.countedWords[firstKey].CompareTo(other.countedWords[secondKey]);

        if (comparisonResult != 0)
        {
          return comparisonResult;
        }
      }

      return 0;
    }

    public int this[string index]
    {
      get
      {
        return this.countedWords[index];
      }
    }

    public int this[int index]
    {
      get
      {
        string key = this.countedWords.Keys.ToList()[index];
        return this.countedWords[key];
      }
    }

    public IEnumerator GetEnumerator()
    {
      return this.countedWords.GetEnumerator();
    }

    public bool ContainsWord(String wordToCheck)
    {
      if (this.countedWords == null || this.countedWords.Count == 0)
      {
        return false;
      }

      return this.countedWords.Keys.Contains(wordToCheck);
    }

    public override string ToString()
    {
      StringBuilder builder = new StringBuilder();
      builder.AppendLine(string.Format("Filename: {0}", this.FileName));
      builder.AppendLine(string.Format("Unique Words: {0}", this.Count));
      builder.AppendLine(string.Format("Total Words: {0}", this.TotalWords));
      foreach (var countedWord in this.countedWords)
      {
        builder.AppendLine(string.Format("{0} : {1}", countedWord.Key, countedWord.Value));
      }

      return builder.ToString();
    }

    public void CopyTo(Array array, int index)
    {
      throw new NotImplementedException();
    }

    public int Count
    {
      get
      {
        return this.countedWords.Count;
      }
    }

    public bool IsSynchronized
    {
      get
      {
        return false;
      }
    }

    public object SyncRoot
    {
      get
      {
        return null;
      }
    }

    #endregion
  }
}
