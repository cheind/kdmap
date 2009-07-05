//  
//  Copyright (C) 2009 Christoph Heindl
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
