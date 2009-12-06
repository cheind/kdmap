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

  public partial class KdMap<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IVector {

    /// <summary>
    /// Add item to kd-map if not contained. 
    /// </summary>
    public void Add(KeyValuePair<TKey, TValue> item) {
      if (this.ContainsKey(item.Key)) {
        throw new ArgumentException("Key already contained in Kd-Map.");
      }
      _kdtree.Add((LocatablePair<TKey, TValue>)item);
    }

    /// <summary>
    /// Remove all items from the kd-map 
    /// </summary>
    public void Clear() {
      _kdtree.Clear();
    }

    /// <summary>
    /// Test if item is contained in kd-map. 
    /// </summary>
    public bool Contains(KeyValuePair<TKey, TValue> item) {
      return this.ContainsKey(item.Key);
    }

    /// <summary>
    /// Copy all elements to array starting at the provided index.
    /// </summary>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
      foreach(KeyValuePair<TKey, TValue> p in this) {
        array[arrayIndex] = p;
        arrayIndex += 1;
      }
    }

    /// <value>
    /// Return the number of elements contained 
    /// </value>
    public int Count {
      get { return _kdtree.Count; }
    }

    /// <value>
    /// Test if this kd-map is read-only 
    /// </value>
    public bool IsReadOnly {
      get { return _kdtree.IsReadOnly; }
    }

    /// <summary>
    /// Remove the given item 
    /// </summary>
    public bool Remove(KeyValuePair<TKey, TValue> item) {
      return _kdtree.Remove(new LocatablePair<TKey, TValue>(item));
    }
  }
}
