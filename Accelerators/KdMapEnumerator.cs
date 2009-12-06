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
