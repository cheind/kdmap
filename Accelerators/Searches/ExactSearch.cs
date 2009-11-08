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