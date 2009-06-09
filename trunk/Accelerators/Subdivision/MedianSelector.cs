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

namespace Accelerators.Subdivision
{
  
  
  /// <summary>
  /// Selects the split location based on the median of the coordinates of all 
  /// elements in the split dimension.
  /// </summary>
  public class MedianSelector : ISplitLocationSelector
  {

    /// <summary>
    /// Selects the split location based on the median of the coordinates of all 
    /// elements in the split dimension.
    /// </summary>
    public double Select<T>(KdNode<T> target, int split_dimension) where T : IVector
    {
      // Sort based on chosen split-dimension
      List<T> vecs = target.Vectors;    
      vecs.Sort(new SingleDimensionComparer<T>(split_dimension));
      
      // Fetch median
      int median_location = MedianLocation(vecs.Count);
      double median_value = vecs[median_location][split_dimension];
      
      // Verify that the coordinate of the last element has a value greater than the 
      // median value.
      if (FloatComparison.Close(vecs[vecs.Count-1][split_dimension], median_value, FloatComparison.DefaultEps)) {
        throw new DegenerateDatasetException();
      }
      
      return median_value;
    }
    
    /// <summary>
    /// Calculate the median position a given count.
    /// </summary>
    private int MedianLocation(int nr_elements) {
      if (nr_elements % 2 == 0) {
        return (nr_elements + 1) / 2;
      } else {
        return nr_elements / 2;
      }
    }
    
     /// <summary>
    /// Compare vectors based on a single dimension
    /// </summary>
    private class SingleDimensionComparer<T> : IComparer<T> where T : IVector {
      
      public SingleDimensionComparer(int dimension) {
        _dimension = dimension;
      }
      
      public int Compare (T x, T y)
      {
        return x[_dimension].CompareTo(y[_dimension]);
      }
      
      private int _dimension;
    }
  }
}
