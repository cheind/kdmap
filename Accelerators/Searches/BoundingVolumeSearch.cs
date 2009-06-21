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
  /// Search for all elements within bounding volume
  /// </summary>
  class BoundingVolumeSearch<T> : KdTreeSearch<T> where T : IVector {

     /// <summary>
    /// Initialize with the kd-tree node to start search at.
    /// </summary>
    public BoundingVolumeSearch(KdNode<T> tree) : base(tree) { }

    /// <summary>
    /// Search for elements within the volume defined by the bounding volume.
    /// </summary>
    public IEnumerable<T> FindInsideBoundingVolume(IBoundingVolume bv) {
      if (!bv.Intersect(this.Tree.InternalBounds))
        yield break;

      // Init search
      Stack<KdNode<T>> s = new Stack<KdNode<T>>();
      s.Push(this.Tree);
      int found = 0;
      // Run
      while (s.Count > 0 && found <= this.Limit) {
        KdNode<T> n = s.Pop();
        if (n.Leaf) {
          // Once we encounter a leaf an exhaustive search is performed for all elements inside
          // the bounding volume up to the adjusted limit.
          List<T> elements = this.ExhaustiveSearchLeaf(n, ref found, delegate(T obj) { return bv.Inside(obj); });
          foreach (T t in elements)
            yield return t;
        } else { // Intermediate node
          // Classify against split plane
          EPlanePosition pos = bv.ClassifyPlane(n.SplitDimension, n.SplitLocation);
          if (pos == EPlanePosition.LeftOfBV) {
            s.Push(n.Right);
          } else if (pos == EPlanePosition.RightOfBV) {
            s.Push(n.Left);
          } else { // Intersecting
            s.Push(n.Right);
            s.Push(n.Left);
          }
        }
      }
    }

  }
}
