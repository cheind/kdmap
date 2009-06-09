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

namespace Accelerators.Subdivision {

  /// <summary>
  /// Exception thrown when a node with less then maximum bucket size elements
  /// is attempted to be split or a parent with more than maximum bucket size elements
  /// is attempted to be collapsed.
  /// </summary>
  public class BucketSizeException : SubdivisionException { }

  /// <summary>
  /// Exception thrown when an intermediate node is attempted to be splitted.
  /// </summary>
  public class IntermediateNodeException : SubdivisionException { }

  /// <summary>
  /// Exception thrown when a leaf node is attempted to be collapsed.
  /// </summary>
  public class LeafNodeException : SubdivisionException { }

  /// <summary>
  /// Exception thrown when the dataset to be split is degenerate in some geometric sense.
  /// </summary>
  public class DegenerateDatasetException : SubdivisionException { }


  /// <summary>
  /// Offers an implementation of ISubdivisionPolicy by delegating the split operation to
  /// a class that chooses the split dimension and one that chooses a split location. Provides
  /// a default implementation for collapsing a node.
  /// </summary>
  public class SubdivisionPolicyConnector : ISubdivisionPolicy {

    /// <summary>
    /// Default constructor
    /// </summary>
    public SubdivisionPolicyConnector() {
      _max_bucket_size = 25;
      _dim_selector = new Accelerators.Subdivision.PeriodicAxisSelector();
      _loc_selector = new Accelerators.Subdivision.MidpointSelector();
    }

    /// <summary>
    /// Construct with maximum bucket size.
    /// </summary>
    public SubdivisionPolicyConnector(int max_bucket_size) {
      _max_bucket_size = max_bucket_size;
      _dim_selector = new Accelerators.Subdivision.PeriodicAxisSelector();
      _loc_selector = new Accelerators.Subdivision.MidpointSelector();
    }
    
    public SubdivisionPolicyConnector(int max_bucket_size, Subdivision.ISplitDimensionSelector dim_select, Subdivision.ISplitLocationSelector loc_select) {
      _max_bucket_size = max_bucket_size;
      _dim_selector = dim_select;
      _loc_selector = loc_select;
    }

    /// <summary>
    /// Splits a node based on bucket size, the chosen split dimension selector and chosen split location selector
    /// </summary>
    public void Split<T>(KdNode<T> target) where T : IVector {
      // Sanity check node
      if (target.Intermediate)
        throw new IntermediateNodeException();

      if (target.Vectors.Count <= this.MaximumBucketSize)
        throw new BucketSizeException();
    
      int split_dim = _dim_selector.Select(target);
      double split_loc = _loc_selector.Select(target, split_dim);
      
      // Pass over vectors, create leaves (one is possibly empty) and update parent
      KdNode<T> left = target.SetLeftChild(new KdNode<T>());
      KdNode<T> right = target.SetRightChild(new KdNode<T>());
      // Split AABB
      AABB left_aabb, right_aabb;
      target.Bounds.Split(split_dim, split_loc, out left_aabb, out right_aabb);
      left.Bounds = left_aabb;
      right.Bounds = right_aabb;
      // Assign vectors
      left.Vectors = new List<T>();
      right.Vectors = new List<T>();
      
      // Classify each vector
      foreach (T t in target.Vectors) {
        if (t[split_dim] <= split_loc)
          left.Vectors.Add(t);
        else
          right.Vectors.Add(t);
      }
      
      // Update target
      target.SplitDimension = split_dim;
      target.SplitLocation = split_loc;
    }
    
    /// <summary>
    /// Default collapsing strategy
    /// </summary>
    public void Collapse<T>(KdNode<T> parent) where T : IVector {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    
    /// <value>
    /// Access the bucket size. When a node contains more
    /// than maximum bucket size elements it is considered for
    /// split
    /// </value>
    public int MaximumBucketSize {
      get {
        return _max_bucket_size;
      }
      set {
        _max_bucket_size = value;
      }
    }
    
    /// <summary>
    /// Create a policy from a ISplitDimensionSelector and ISplitLocationSelector
    /// </summary>
    public static ISubdivisionPolicy CreatePolicy<DimSelector,LocSelector>(int max_bucket_size) 
      where DimSelector : Subdivision.ISplitDimensionSelector, new()
      where LocSelector : Subdivision.ISplitLocationSelector, new()
    {
      return new SubdivisionPolicyConnector(max_bucket_size, new DimSelector(), new LocSelector());
    }

    private int _max_bucket_size;
    private Subdivision.ISplitDimensionSelector _dim_selector;
    private Subdivision.ISplitLocationSelector _loc_selector;
  }
}
