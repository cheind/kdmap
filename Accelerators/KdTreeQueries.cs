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

namespace Accelerators
{
  public partial class KdTree<T> where T : IVector
  {
    
    /// <summary>
    /// Find the closest leaf to the given vector.
    /// This finds the closest leaf even if the vector is outside of the root bounding box.
    /// </summary>
    public KdNode<T> FindClosestLeaf(IVector x) {
      KdNode<T> n = _root;
      while (n.Intermediate) {
        if (x[n.SplitDimension] <= n.SplitLocation)
          n = n.Left;
        else
          n = n.Right;
      }
      return n;
    }
    
    /// <summary>
    /// Find the stored element corresponding to the given location. If multiple elements within
    /// the tolerance of eps exist, the first one encountered is returned. The parameter eps is here
    /// defined as the component-wise symmetric interval that two coordiantes are allowed to deviate from.
    /// </summary>
    public T Find(IVector x, float eps) {
      // If point is not within root-bounds we can exit early
      if (!this.Root.Bounds.Inside(x))
        return default(T);
      
      // Find the closest leaf. As the point is within root bounds the closest leaf is the 
      // smallest room that can contain the point
      KdNode<T> leaf = FindClosestLeaf(x);
      
      // Scan
      foreach(T t in leaf.Vectors) {
        if (VectorComparison.Close(x, t, eps))
          return t;
      }
     
      // Nothing found
      return default(T);      
    }
    
  }
  
  
}
