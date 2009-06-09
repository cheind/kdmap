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
  /// Subdivision policy based on the midpoint of the axis of maximum spread.
  /// This leads to square nodes.
  /// </summary>
  public class MidpointSubdivisionPolicy : SubdivisionPolicyBase {

    /// <summary>
    /// Construct with default bucket size.
    /// </summary>
    public MidpointSubdivisionPolicy() : base (25) {
    }

    /// <summary>
    /// Construct with maximum bucket size.
    /// </summary>
    public MidpointSubdivisionPolicy(int max_bucket_size)
      : base(max_bucket_size) {
    }

    /// <summary>
    /// Split based on midpoint of axis of maximum spread
    /// </summary>
    public override void Split<T>(KdNode<T> target) {
      // Sanity check
      this.TestDefaultSplitConstraints(target);

      IVector diagonal = target.Bounds.Diagonal;
      int max_spread_id = VectorReductions.IndexNormInf(diagonal);
      double spread = diagonal[max_spread_id];

      // Sanity check for degenerate data-sets
      if (FloatComparison.CloseZero(spread, FloatComparison.DefaultEps))
        throw new DegenerateDatasetException();

      // Split
      double split = target.Bounds.Lower[max_spread_id] + diagonal[max_spread_id] * 0.5;

      List<T> left_vecs = new List<T>();
      List<T> right_vecs = new List<T>();
      
      // Classify each vector
      foreach (T t in target.Vectors) {
        if (t[max_spread_id] <= split)
          left_vecs.Add(t);
        else
          right_vecs.Add(t);
      }
      
      // Test for degenerate case
      if (left_vecs.Count == 0 || right_vecs.Count == 0)
        throw new DegenerateDatasetException();
      
      // Split AABB
      AABB left_aabb, right_aabb;
      target.Bounds.Split(max_spread_id, split, out left_aabb, out right_aabb);

      // Finalize the node
      KdNode<T> left = target.SetLeftChild(new KdNode<T>());
      KdNode<T> right = target.SetRightChild(new KdNode<T>());
      left.Vectors = left_vecs;
      right.Vectors = right_vecs;
      left.Bounds = left_aabb;
      right.Bounds = right_aabb;

      target.SplitDimension = max_spread_id;
      target.SplitLocation = split;
    }
  }
}
