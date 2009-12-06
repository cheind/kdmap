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
