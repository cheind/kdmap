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

namespace Accelerators
{
  
  /// <summary>
  /// Enumerates the elements stored in a KdMap from left to right.
  /// </summary>
  class KdMapEnumerator<TKey, TValue> : IEnumerator<KeyValuePair<TKey, TValue>> where TKey : IVector
  {
    
    /// <summary>
    /// Construct from tree
    /// </summary>
    public KdMapEnumerator(KdMap<TKey, TValue> map)
    {
      _tree = map.KdTree;
      _enumerator = _tree.GetEnumerator();
    }

    /// <summary>
    /// Access current enumerated item
    /// </summary>
    public KeyValuePair<TKey, TValue> Current {
      get {
        return (KeyValuePair<TKey, TValue>)_enumerator.Current;
      }
    }

    /// <summary>
    /// Dispose enumerator.
    /// </summary>
    public void Dispose() {
      _tree = null;
      if (_enumerator != null)
        _enumerator.Dispose();
      _enumerator = null;
    }

    /// <summary>
    /// Access current enumerated item
    /// </summary>
    object System.Collections.IEnumerator.Current {
      get {
        return this.Current;
      }
    }

    /// <summary>
    /// Move enumerator forward
    /// </summary>
    public bool MoveNext() {
      return _enumerator.MoveNext();
    }

    /// <summary>
    /// Reset enumerator to initial state.
    /// </summary>
    public void Reset() {
      _enumerator = _tree.GetEnumerator();
    }

    private KdTree<LocatablePair<TKey, TValue>> _tree;
    private IEnumerator<LocatablePair<TKey, TValue>> _enumerator;
  }
}
