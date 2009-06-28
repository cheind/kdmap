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

    /// <value>
    /// Get a collection of the contained keys 
    /// </value>
    public ICollection<TKey> Keys {
      get { throw new Exception("The method or operation is not implemented."); }
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

    public ICollection<TValue> Values {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public TValue this[TKey key] {
      get {
        throw new Exception("The method or operation is not implemented.");
      }
      set {
        
        throw new Exception("The method or operation is not implemented.");
      }
    }
  }
}
