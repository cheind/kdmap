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
  /// Exception thrown when collapsing the root node is attempted.
  /// </summary>
  public class RootNodeException : SubdivisionException { }

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
      left.InternalBounds = new AABB(target.InternalBounds.Dimensions);
      left.InternalBounds.Enlarge<T>(left.Vectors);

      right.InternalBounds = new AABB(target.InternalBounds.Dimensions);
      right.InternalBounds.Enlarge<T>(right.Vectors);
      
      // Update target
      target.SplitDimension = split_dim;
      target.SplitLocation = split_loc;
    }
    
    /// <summary>
    /// Default collapsing strategy
    /// </summary>
    public void Collapse<T>(KdNode<T> target) where T : IVector {
      if (!target.Leaf)
        throw new IntermediateNodeException();
      if (target.Root)
        throw new RootNodeException();

      KdNode<T> parent = target.Parent;
      if (parent.Left.Leaf && parent.Right.Leaf) {
        // When both children are leaves we push their content to the parent
        int capacity = parent.Left.Vectors.Count + parent.Right.Vectors.Count;
        parent.Vectors = new List<T>(capacity);
        parent.Vectors.AddRange(parent.Left.Vectors);
        parent.Vectors.AddRange(parent.Right.Vectors);
        AABB aabb = parent.InternalBounds;
        AABB.Union(parent.Left.InternalBounds, parent.Right.InternalBounds, ref aabb);
        parent.UnSetLeftChild();
        parent.UnSetRightChild();
      } else {
        // The sibling of 'target' is not a leaf. We replace the content of parent with the content of the sibling.
        KdNode<T> sibling = (parent.ContainsInLeftSubTree(target) ? parent.Right : parent.Left);
        parent.UnSetLeftChild();
        parent.UnSetRightChild();

        // Copy kdtree content
        parent.SplitDimension = sibling.SplitDimension;
        parent.SplitLocation = sibling.SplitLocation;
        parent.Vectors = sibling.Vectors;
        parent.InternalBounds = sibling.InternalBounds;

        // Copy structural info
        KdNode<T> sibling_left = sibling.UnSetLeftChild();
        KdNode<T> sibling_right = sibling.UnSetRightChild();
        parent.Left = sibling_left;
        parent.Right = sibling_right;
      }

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
