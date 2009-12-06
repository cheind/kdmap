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

namespace Accelerators.Searches {

  /// <summary>
  /// Search for elements with exact matching coordinates
  /// </summary>
  public class ExactSearch<T> : KdTreeSearch<T> where T : IVector {

    /// <summary>
    /// Initialize with the kd-tree node to start search at.
    /// </summary>
    public ExactSearch(KdNode<T> tree) : base(tree) {
      _cls = new ClosestLeafSearch<T>(tree);
    }

    /// <summary>
    /// Search for elements with exact matching coordinates
    /// </summary>
    public IEnumerable<T> FindExact(IVector x) {
      // If point is not within root-bounds we can exit early
      if (!this.Tree.InternalBounds.Inside(x))
        yield break;

      // Else we fetch the leaf which x resides in
      KdNode<T> leaf = _cls.FindClosestLeaf(x);

      // Search through leaf and report all found up to the limit.
      int found = 0;
      ExactMatchPredicate emp = new KdTreeSearch<T>.ExactMatchPredicate(x);
      foreach (T t in ExhaustiveSearchLeaf(leaf, ref found, emp.Test)) {
        yield return t;
      }
    }

    /// <summary>
    /// Specialized exact search for the first matching item. This method
    /// does not carry the overhead of creating IEnumerables for its result set.
    /// </summary>
    public bool TryFindExactFirst(IVector x, out T first) {
      // If point is not within root-bounds we can exit early
      if (!this.Tree.InternalBounds.Inside(x)) {
        first = default(T);
        return false;
      }
        
      // Else we fetch the leaf x possibly resides in
      KdNode<T> leaf = _cls.FindClosestLeaf(x);
      // And test for containment
      int index = leaf.Vectors.FindIndex(delegate(T obj) { return VectorComparison.Equal(x, obj); });
      if (index < 0) {
        first = default(T);
        return false;
      } else {
        first = leaf.Vectors[index];
        return true;
      }
    }
    
    /// <summary>
    /// Test if element with same coordinates is contained 
    /// </summary>
    public bool Contains(IVector x) {
      T tmp;
      return this.TryFindExactFirst(x, out tmp);
    }
    
    private ClosestLeafSearch<T> _cls;
  }
}
