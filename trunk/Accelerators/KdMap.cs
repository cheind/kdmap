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
  /// K-dimensional dictionary
  /// </summary>
  public partial class KdMap<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IVector {

    /// <summary>
    /// Construct default kd-map with n-dimensions.
    /// </summary>
    public KdMap(int dimensions) {
      _kdtree = new KdTree<LocatablePair<TKey, TValue>>(dimensions, new Subdivision.SubdivisionPolicyConnector());
      _es = new Searches.ExactSearch<LocatablePair<TKey, TValue>>(_kdtree.Root);
      _es.CountLimit = 1;
    }

    public KdMap(int dimensions, Subdivision.ISubdivisionPolicy policy) {
      _kdtree = new KdTree<LocatablePair<TKey, TValue>>(dimensions, policy);
      _es = new Searches.ExactSearch<LocatablePair<TKey, TValue>>(_kdtree.Root);
      _es.CountLimit = 1;
    }
    
    /// <summary>
    /// Access the underlying kd-tree.
    /// </summary>
    public KdTree<LocatablePair<TKey, TValue>> KdTree {
      get { return _kdtree; }
    }
    
    /// <summary>
    /// Try to find the first element that matches the given key. 
    /// </summary>
    private bool TryFindFirst(TKey key, out TValue value) {
      LocatablePair<TKey, TValue> tmp;
      if (_es.TryFindExactFirst(key, out tmp)) {
        value = tmp.Second;
        return true;
      } else {
        value = default(TValue);
        return false;
      }
    }

    private KdTree<LocatablePair<TKey, TValue>> _kdtree;
    private Searches.ExactSearch<LocatablePair<TKey, TValue>> _es;
  }
}
