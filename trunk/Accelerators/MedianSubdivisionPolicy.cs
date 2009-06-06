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

namespace Accelerators
{
  
  public class IntermediateNodeException: SplitException {}
  public class BucketSizeNotReachedException : SplitException {}
  public class DegenerateDatasetException : SplitException {}
  
  
  /// <summary>
  /// Subdivision policy based on median of the axis of maximum spread.
  /// </summary>
  public class MedianSubdivisionPolicy : ISubdivisionPolicy
  {
    
    public MedianSubdivisionPolicy() {
      _max_bucket_size = 25;
    }
    
    public MedianSubdivisionPolicy(int max_bucket_size) {
      _max_bucket_size = max_bucket_size;
    }
    
    public int MaximumBucketSize {
      get {
        return _max_bucket_size;
      }
      set {
        _max_bucket_size = value;
      }
    }

    #region ISubdivisionPolicy implementation
    public void Split<T> (KdNode<T> target) where T : IVector
    {
      // Sanity checks first
      if (target.Intermediate)
        throw new IntermediateNodeException();
      if (target.Vectors.Count <= this.MaximumBucketSize)
        throw new BucketSizeNotReachedException();
      
      // Find axis of maximum spread
      IVector diagonal = target.Bounds.Diagonal;
      int max_spread_id = VectorReductions.IndexNormInf(diagonal);
      float spread = diagonal[max_spread_id];
      
      // Sanity check for degenerate data-sets
      if (FloatComparison.CloseZero(spread, FloatComparison.DefaultEps))
        throw new DegenerateDatasetException();
      
      // Perform split
      
      // Sort based on chosen split-dimension
      List<T> vecs = target.Vectors;    
      vecs.Sort(new SingleDimensionComparer<T>(max_spread_id));                  
      // Fetch median
      int median_location = MedianLocation(vecs.Count);
      float median_value = vecs[median_location][max_spread_id];
      
      // Need to scroll backward starting from median_location until we find an element < median_value
      // [0,median_location] -> left_child (<=median), (median_location, end) -> right_child (>median)
      while (median_location < vecs.Count && vecs[median_location][max_spread_id] == median_value) {
        ++median_location;
      }
      
      // If no value bigger than median is found, we have a degenerate split
      // Assume the values [1,2,3,3,3,3,3] where the median is 3. Splitting at the median position would
      // result in a right child that only contains 3s.
      if (median_location == vecs.Count)
        throw new DegenerateDatasetException();
      
      // Instance children and update
      KdNode<T> left = target.SetLeftChild(new KdNode<T>());
      KdNode<T> right = target.SetRightChild(new KdNode<T>());
      left.Vectors = vecs.GetRange(0, median_location);
      right.Vectors = vecs.GetRange(median_location, vecs.Count - median_location);      
      
      AABB left_aabb, right_aabb;
      target.Bounds.Split(max_spread_id, median_value, out left_aabb, out right_aabb);
      left.Bounds = left_aabb;
      right.Bounds = right_aabb;
      
      target.SplitDimension = max_spread_id;
      target.SplitLocation = median_value;
    }
    
    public void Collapse<T> (KdNode<T> parent) where T : IVector
    {
      throw new System.NotImplementedException();
    }
    #endregion
    
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
      
      #region IComparer<T> implementation
      public int Compare (T x, T y)
      {
        return x[_dimension].CompareTo(y[_dimension]);
      }
      #endregion
      
      private int _dimension;
    }
    
    
    private int _max_bucket_size;
  }
}
