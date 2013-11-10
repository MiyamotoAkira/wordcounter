
namespace Extensions
{
  using System.Collections.Generic;

  public static class IDictionaryExtensions
  {
    public static bool IsNullOrEmpty<T,V>(this IDictionary<T,V> dictionary)
    {
      return dictionary == null || dictionary.Count == 0;
    }
  }
}
