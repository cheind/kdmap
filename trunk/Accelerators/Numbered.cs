using System;
using System.Collections.Generic;
using System.Text;

namespace Accelerators {

  /// <summary>
  /// Provides numbered access to elements of an enumerable.
  /// </summary>
  public class Numbered {

    /// <summary>
    /// First element of enumeration
    /// </summary>
    public static T First<T>(IEnumerable<T> e) {
      return Numbered.Nth(e, 1);
    }

    /// <summary>
    /// First element of enumeration
    /// </summary>
    public static T Second<T>(IEnumerable<T> e) {
      return Numbered.Nth(e, 2);
    }

    /// <summary>
    /// First element of enumeration
    /// </summary>
    public static T Third<T>(IEnumerable<T> e) {
      return Numbered.Nth(e, 3);
    }

    /// <summary>
    /// First element of enumeration
    /// </summary>
    public static T FirstOrDefault<T>(IEnumerable<T> e) {
      return Numbered.NthOrDefault(e, 1);
    }

    /// <summary>
    /// First element of enumeration
    /// </summary>
    public static T SecondOrDefault<T>(IEnumerable<T> e) {
      return Numbered.NthOrDefault(e, 2);
    }

    /// <summary>
    /// First element of enumeration
    /// </summary>
    public static T ThirdOrDefault<T>(IEnumerable<T> e) {
      return Numbered.NthOrDefault(e, 3);
    }

    /// <summary>
    /// N-th element of enumeration
    /// </summary>
    public static T Nth<T>(IEnumerable<T> enumerable, int nth) {
      int pos = -1;
      using (IEnumerator<T> e = enumerable.GetEnumerator()) {
        while (pos != (nth - 1) && e.MoveNext()) {
          pos += 1;
        }
        if (pos != (nth - 1))
          throw new InvalidOperationException(String.Format("IEnumerable contains less than {0} elements", nth));
        return e.Current;
      }
    }

    /// <summary>
    /// N-th element of enumeration or default if enumeration contains less the n elements
    /// </summary>
    public static T NthOrDefault<T>(IEnumerable<T> enumerable, int nth) {
      int pos = -1;
      T val = default(T);
      using (IEnumerator<T> e = enumerable.GetEnumerator()) {
        while (pos != (nth - 1) && e.MoveNext()) {
          pos += 1;
        }
        if (pos == (nth - 1))
          val = e.Current;
      }
      return val;
    }
  }
}
