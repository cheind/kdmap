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
  /// Enumerates the elements stored in a kd-tree from left to right.
  /// </summary>
  class KdTreeEnumerator<T> : IEnumerator<T> where T : IVector
  {
    
    /// <summary>
    /// Construct from tree
    /// </summary>
    public KdTreeEnumerator(KdTree<T> tree)
    {
      _root = tree.Root;
      _leaves = new List<KdNode<T>>(_root.Leaves);
      _current_leaf = _leaves.GetEnumerator();
      _current_element = null;
    }
    
    /// <summary>
    /// Dispose enumerator
    /// </summary>
    public void Dispose() {
      _root = null;
      _leaves = null;
      if (_current_element != null) {
        _current_element.Dispose();
      }
      _current_leaf.Dispose();
    }
    
    /// <value>
    /// Access current element.
    /// </value>
    public T Current {
      get {return _current_element.Current;}
    }
    
    /// <value>
    /// Access current element.
    /// </value>
    object System.Collections.IEnumerator.Current {
      get {return _current_element.Current;}
    }
    
    /// <summary>
    /// Move iterator to next element.
    /// </summary>
    public bool MoveNext() {
      bool can_move = false;
      if (_current_element == null) {
        can_move = this.MoveToFirstOfNextLeaf();
      } else {
        can_move = _current_element.MoveNext();
        if (!can_move) {
          can_move = this.MoveToFirstOfNextLeaf();
        }
      }
      return can_move;
    }
    
    /// <summary>
    /// Try to move iterators to the first element of the next leaf, skipping empty leaves.
    /// </summary>
    private bool MoveToFirstOfNextLeaf() {
      if (_current_element != null)
        _current_element.Dispose();
      
      bool found_non_empty = false;
      while (!found_non_empty && _current_leaf.MoveNext()) {
        found_non_empty = _current_leaf.Current.Vectors.Count > 0;
      }
      
      if (found_non_empty) {
        _current_element = _current_leaf.Current.Vectors.GetEnumerator();
        return _current_element.MoveNext();
      } else {
        return false;
      }
    }
    
    /// <summary>
    /// Reset the enumerator
    /// </summary>
    public void Reset() {
      if (_current_element != null)
        _current_element.Dispose();
      _current_leaf.Dispose();
      _current_element = null;
      _current_leaf = _leaves.GetEnumerator();
    }
    
    
    private KdNode<T> _root;
    private List<KdNode<T>> _leaves;
    private IEnumerator<KdNode<T>> _current_leaf;
    private IEnumerator<T> _current_element;
  }
}
