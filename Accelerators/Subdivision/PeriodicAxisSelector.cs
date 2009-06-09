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

namespace Accelerators.Subdivision
{
  
  /// <summary>
  /// Chooses a split dimension based on the depth of the node. At each depth level, d, 
  /// the d % dimensions axis is chosen. If the choses axis has no extension a single loop over
  /// all dimensions starting at the chosen one determines the next axis that has a positive extension.
  /// If no such axis can be found the dataset is degenerate.
  /// </summary>
  public class PeriodicAxisSelector : ISplitDimensionSelector
  {
    /// <summary>
    /// Select axis by cycling through dimensions at each depth level of tree
    /// </summary>
    public int Select<T> (KdNode<T> target) where T : IVector {
        
      IVector diagonal = target.Bounds.Diagonal;
      int dims = target.Bounds.Dimensions;
      
      // Setup initial axis
      // On root level this is zero on each other level it the depth of the node module its dimensions
      int axis = target.Depth % dims;
      double spread = diagonal[axis];
      int i = 0;
      while (FloatComparison.CloseZero(spread, FloatComparison.DefaultEps) && i < dims) {
        axis = (axis + 1) % dims;
        spread = diagonal[axis];
        i += 1;
      }
      
      if (i == dims) {
        // Cylce completed without finding an axis that has a spread greater than zero
        throw new DegenerateDatasetException();
      }
      
      return axis;
    }
  }
}
