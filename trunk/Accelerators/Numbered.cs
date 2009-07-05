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
      return Numbered.Nth(e, 0);
    }

    /// <summary>
    /// First element of enumeration
    /// </summary>
    public static T Second<T>(IEnumerable<T> e) {
      return Numbered.Nth(e, 1);
    }

    /// <summary>
    /// First element of enumeration
    /// </summary>
    public static T Third<T>(IEnumerable<T> e) {
      return Numbered.Nth(e, 2);
    }
    
    /// <summary>
    /// Try accessing first element or return false
    /// </summary>
    public static bool TryFirst<T>(IEnumerable<T> e, out T value) {
      return Numbered.TryNth(e, 0, out value);
    }
    
    /// <summary>
    /// Try accessing second element or return false
    /// </summary>
    public static bool TrySecond<T>(IEnumerable<T> e, out T value) {
      return Numbered.TryNth(e, 1, out value);
    }

    
    /// <summary>
    /// Try accessing third element or return false
    /// </summary>
    public static bool TryThird<T>(IEnumerable<T> e, out T value) {
      return Numbered.TryNth(e, 2, out value);
    }
   
    /// <summary>
    /// First element of enumeration
    /// </summary>
    public static T FirstOrDefault<T>(IEnumerable<T> e) {
      return Numbered.NthOrDefault(e, 0);
    }

    /// <summary>
    /// First element of enumeration
    /// </summary>
    public static T SecondOrDefault<T>(IEnumerable<T> e) {
      return Numbered.NthOrDefault(e, 1);
    }

    /// <summary>
    /// First element of enumeration
    /// </summary>
    public static T ThirdOrDefault<T>(IEnumerable<T> e) {
      return Numbered.NthOrDefault(e, 2);
    }

    /// <summary>
    /// Test if enumerable is empty
    /// </summary>
    public static bool Empty<T>(IEnumerable<T> e) {
      using (IEnumerator<T> en = e.GetEnumerator()) {
        return !en.MoveNext();
      }
    }

    /// <summary>
    /// N-th element of enumeration
    /// </summary>
    public static T Nth<T>(IEnumerable<T> enumerable, int nth) {
      using (IEnumerator<T> e = enumerable.GetEnumerator()) {
        if (!Advance(e, nth))
          throw new InvalidOperationException(String.Format("IEnumerable contains less than {0} elements", nth));
        return e.Current;
      }
    }
    
    /// <summary>
    /// Try getting the N-th element of enumeration
    /// </summary>
    public static bool TryNth<T>(IEnumerable<T> enumerable, int nth, out T value) {
      using (IEnumerator<T> e = enumerable.GetEnumerator()) {
        if (Advance(e, nth)) {
          value = e.Current;
          return true;
        } else {
          value = default(T);
          return false;
        }
      }
    }

    /// <summary>
    /// N-th element of enumeration or default if enumeration contains less the n elements
    /// </summary>
    public static T NthOrDefault<T>(IEnumerable<T> enumerable, int nth) {
      T val = default(T);
      using (IEnumerator<T> e = enumerable.GetEnumerator()) {
        if (Advance(e, nth))
          val = e.Current;
      }
      return val;
    }
    
    /// <summary>
    /// Try linear forwarding of enumerator to desired position 
    /// </summary>
    private static bool Advance<T>(IEnumerator<T> e, int nth) {
      int pos = -1;
      while (pos != nth && e.MoveNext()) {
          pos += 1;
      }
      return pos == nth;
    }
  }
}
