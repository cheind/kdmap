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
      _trivial_resolver = new NoOperationResolver();
    }
    
    public SubdivisionPolicyConnector(int max_bucket_size, ISplitDimensionSelector dim_select, ISplitLocationSelector loc_select) {
      _max_bucket_size = max_bucket_size;
      _dim_selector = dim_select;
      _loc_selector = loc_select;
      _trivial_resolver = new NoOperationResolver();
    }

    public SubdivisionPolicyConnector(int max_bucket_size, ISplitDimensionSelector dim_select, ISplitLocationSelector loc_select, ITrivialSplitResolver triv_resolver) {
      _max_bucket_size = max_bucket_size;
      _dim_selector = dim_select;
      _loc_selector = loc_select;
      _trivial_resolver = triv_resolver;
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
    
      // Find axis of split plane
      int split_dim = _dim_selector.Select(target);
      // Find location of split plane
      double split_loc = _loc_selector.Select(target, split_dim);
      
      // Possibly resolve a trivial split
      ETrivialSplitType split_type = this.IsTrivialSplit(target, split_dim, split_loc);
      if (split_type == ETrivialSplitType.EmptyLeft || split_type == ETrivialSplitType.EmptyRight) {
        split_loc = _trivial_resolver.Resolve(target, split_dim, split_loc, split_type);
      }

      // Pass over vectors, create leaves (one is possibly empty) and update parent
      KdNode<T> left = target.SetLeftChild(new KdNode<T>());
      KdNode<T> right = target.SetRightChild(new KdNode<T>());
      // Split AABB
      AABB left_split_aabb, right_split_aabb;
      target.SplitBounds.Split(split_dim, split_loc, out left_split_aabb, out right_split_aabb);
      left.SplitBounds = left_split_aabb;
      right.SplitBounds = right_split_aabb;
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

      // Update internal bounds
      left.InternalBounds = new AABB(left.SplitBounds.Dimensions);
      left.InternalBounds.Enlarge<T>(left.Vectors);

      right.InternalBounds = new AABB(right.SplitBounds.Dimensions);
      right.InternalBounds.Enlarge<T>(right.Vectors);
      
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
    public static ISubdivisionPolicy CreatePolicy<DimSelector,LocSelector, TrivResolver>(int max_bucket_size) 
      where DimSelector : Subdivision.ISplitDimensionSelector, new()
      where LocSelector : Subdivision.ISplitLocationSelector, new()
      where TrivResolver : Subdivision.ITrivialSplitResolver, new()
    {
      return new SubdivisionPolicyConnector(max_bucket_size, new DimSelector(), new LocSelector(), new TrivResolver());
    }
    
    /// <summary>
    /// Determine if the chosen split is a trivial one.
    /// </summary>
    private ETrivialSplitType IsTrivialSplit<T>(KdNode<T> target, int split_dimension, double split_location) where T : IVector {
      // Test for trivial split O(n)
      int count_left = 0;
      int count_right = 0;
      int i = 0;
      
      // Loop over vectors and try to early exit the loop when both left and right are assigned at least one element.
      while ((i < target.Vectors.Count) && (count_left == 0 || count_right == 0)){
        T t = target.Vectors[i];
        if (t[split_dimension] <= split_location)
          count_left += 1;
        else
          count_right += 1;
        ++i;  
      }

      if (count_left == 0)
        return ETrivialSplitType.EmptyLeft;
      else if (count_right == 0)
        return ETrivialSplitType.EmptyRight;
      else
        return ETrivialSplitType.NoTrivial;
      
    }

    private int _max_bucket_size;
    private Subdivision.ISplitDimensionSelector _dim_selector;
    private Subdivision.ISplitLocationSelector _loc_selector;
    private Subdivision.ITrivialSplitResolver _trivial_resolver;
  }
}
