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
  /// Search for the closest leaf to a given element.
  /// </summary>
  public class ClosestLeafSearch<T> : KdTreeSearch<T> where T : IVector {

    /// <summary>
    /// Initialize with the kd-tree node to start search at.
    /// </summary>
    public ClosestLeafSearch(KdNode<T> tree) : base(tree, 1, Double.MaxValue) {}

    /// <summary>
    /// Find the closest leaf to the given element.
    /// </summary>
    /// <remarks>
    /// This finds the closest leaf even if the vector is outside of the root bounding box.
    /// </remarks>
    public KdNode<T> FindClosestLeaf(IVector x) {
      KdNode<T> n = this.Tree;
      while (n.Intermediate) {
        if (x[n.SplitDimension] <= n.SplitLocation)
          n = n.Left;
        else
          n = n.Right;
      }
      return n;
    }
  }
}
