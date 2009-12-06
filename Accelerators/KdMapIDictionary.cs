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
using System.Collections.ObjectModel;

namespace Accelerators {

  /// <summary>
  /// K-dimensional dictionary
  /// </summary>
  public partial class KdMap<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IVector {

    /// <summary>
    /// Add item to the kd-map. 
    /// </summary>
    public void Add(TKey key, TValue value) {
      if (this.ContainsKey(key)) {
        throw new ArgumentException("Key already contained in Kd-Map.");
      }
      _kdtree.Add(new LocatablePair<TKey,TValue>(key, value));
    }

    /// <summary>
    /// Test if kd-map contains the specified key. 
    /// </summary>
    public bool ContainsKey(TKey key) {
      return _es.Contains(key);
    }

    /// <summary>
    /// Remove item by key from the kd-map.
    /// </summary>
    public bool Remove(TKey key) {
      return _kdtree.Remove(new LocatablePair<TKey, TValue>(key, default(TValue)));
    }

    /// <summary>
    /// Try accessing the item hold by the given key 
    /// </summary>
    public bool TryGetValue(TKey key, out TValue value) {
      return this.TryFindFirst(key, out value);
    }

    /// <summary>
    /// Get a collection of the contained values
    /// </summary>
    public ICollection<TValue> Values {
      get {
        List<TValue> vals  = new List<TValue>();
        foreach(LocatablePair<TKey, TValue> p in _kdtree) {
          vals.Add(p.Second);
        }
        return vals;
      }
    }

    /// <value>
    /// Get a read-only collection of the contained keys 
    /// </value>
    public ICollection<TKey> Keys {
      get {
        List<TKey> keys = new List<TKey>();
        foreach (LocatablePair<TKey, TValue> p in _kdtree) {
          keys.Add(p.First);
        }
        return new ReadOnlyCollection<TKey>(keys);
      }
    }

    /// <summary>
    /// Access item by key
    /// </summary>
    public TValue this[TKey key] {
      get {
        TValue val;
        if (!this.TryFindFirst(key, out val))
          throw new KeyNotFoundException(String.Format("Key {0} was not found in dictionary.", key));
        else
          return val;
      }
      set {
        LocatablePair<TKey, TValue> pair;
        if (!_es.TryFindExactFirst(key, out pair)) {
          this.Add(key, value);
        } else {
          // LocatablePair is a reference type, so we can update it.
          pair.Second = value;
        }
      }
    }
  }
}
