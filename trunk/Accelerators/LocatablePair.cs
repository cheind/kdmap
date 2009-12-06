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
  /// Represents a key/value pair insertable into a kd-tree
  /// </summary>
  public class LocatablePair<T, U> : Pair<T,U>, IVector where T : IVector {

    /// <summary>
    /// Construct from first and second parameter.
    /// </summary>
    public LocatablePair(T t, U u) : base(t, u) {}
    
    /// <summary>
    /// Construct from key/value pair 
    /// </summary>
    public LocatablePair(KeyValuePair<T,U> pair) : base (pair.Key, pair.Value) {}

    /// <summary>
    /// Access dimensionality of pair.
    /// </summary>
    public int Dimensions {
      // forward to first
      get { return this.First.Dimensions; }
    }

    /// <summary>
    /// Access individual coordinates by index.
    /// </summary>
    public double this[int index] {
      // forward to first
      get {
        return this.First[index];
      }
      set {
        this.First[index] = value;
      }
    }

    /// <summary>
    /// Explicit conversion from KeyValuePair to LocatablePair
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public static explicit operator LocatablePair<T,U>(KeyValuePair<T,U> other) {
      return new LocatablePair<T, U>(other);
    }

    /// <summary>
    /// Explicit conversion from LocatablePair to KeyValuePair
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public static explicit operator KeyValuePair<T,U>(LocatablePair<T,U> other) {
      return other.ToKeyValuePair();
    }

  }
}
