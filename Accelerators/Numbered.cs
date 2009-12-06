// 
//  Copyright (c) 2009, Christoph Heindl
//  All rights reserved.
// 
//  Redistribution and use in source and binary forms, with or without modification, are 
//  permitted provided that the following conditions are met:
// 
//  Redistributions of source code must retain the above copyright notice, this list of 
//  conditions and the following disclaimer. 
//  Redistributions in binary form must reproduce the above copyright notice, this list 
//  of conditions and the following disclaimer in the documentation and/or other materials 
//  provided with the distribution. 
//  Neither the name Christoph Heindl nor the names of its contributors may be used to endorse 
//  or promote products derived from this software without specific prior written permission.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS 
//  OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
//  AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
//  DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
//  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
//  IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
//  OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 

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
