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
