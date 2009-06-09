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

namespace Accelerators {

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
  /// Offers commonly used operations on split policies.
  /// </summary>
  public abstract class SubdivisionPolicyBase : ISubdivisionPolicy {

    /// <summary>
    /// Default constructor
    /// </summary>
    public SubdivisionPolicyBase() {
      _max_bucket_size = 25;
    }

    /// <summary>
    /// Construct with maximum bucket size.
    /// </summary>
    public SubdivisionPolicyBase(int max_bucket_size) {
      _max_bucket_size = max_bucket_size;
    }

    /// <summary>
    /// Default collapsing strategy
    /// </summary>
    public void Collapse<T>(KdNode<T> parent) where T : IVector {
      throw new NotImplementedException("The method or operation is not implemented.");
    }

    /// <summary>
    /// No default splitting policy.
    /// </summary>
    public abstract void Split<T>(KdNode<T> target) where T : IVector;

    /// <summary>
    /// Test if splitting a node with respect to number of elements contained
    /// and node type is possible.
    /// </summary>
    protected void TestDefaultSplitConstraints<T>(KdNode<T> node) where T : IVector {
      if (node.Intermediate)
        throw new IntermediateNodeException();

      if (node.Vectors.Count <= this.MaximumBucketSize)
        throw new BucketSizeException();
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

    private int _max_bucket_size;
  }
}
