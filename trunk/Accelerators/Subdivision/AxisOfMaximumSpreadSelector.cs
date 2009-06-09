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
  /// Selects the axis of maximum spread for a split.
  /// </summary>
  public class AxisOfMaximumSpreadSelector : ISplitDimensionSelector
  {
    /// <summary>
    /// Select axis of maximum spread and test for degenerate data sets.
    /// </summary>
    public int Select<T> (KdNode<T> target) where T : IVector {
      IVector diagonal = target.Bounds.Diagonal;
      int max_spread_id = VectorReductions.IndexNormInf(diagonal);
      
      // Sanity check for degenerate data-sets
      if (FloatComparison.CloseZero(diagonal[max_spread_id], FloatComparison.DefaultEps))
        throw new DegenerateDatasetException();
      
      return max_spread_id;
    }
  }
}
