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

namespace Accelerators.Searches {

  /// <summary>
  /// Base class for all spatial kd-tree searches.
  /// </summary>
  public class KdTreeSearch<T> where T : IVector {

    /// <summary>
    /// Initialize with the kd-tree node to start search at.
    /// </summary>
    /// <param name="tree"></param>
    public KdTreeSearch(KdNode<T> tree) {
      _tree = tree;
      this.CountLimit = Int32.MaxValue;
      this.DistanceLimit = Double.MaxValue;
    }

    /// <summary>
    /// Initialize with the kd-tree node to start search at and a custom limit
    /// </summary>
    /// <param name="tree"></param>
    public KdTreeSearch(KdNode<T> tree, int count_limit, double distance_limit) {
      _tree = tree;
      this.CountLimit = count_limit;
      this.DistanceLimit = distance_limit;
    }

    /// <summary>
    /// Access the kd-tree node this search is started at.
    /// </summary>
    public KdNode<T> Tree {
      get { return _tree; }
      set { _tree = value; }
    }

    /// <summary>
    /// Limit the number of elements found to the given value
    /// </summary>
    public int CountLimit {
      get { return _limit; }
      set { _limit = value; }
    }
    
    /// <value>
    /// Access the distance (L2norm) the query results are limited to. 
    /// </value>
    public double DistanceLimit {
      get {return _limit_distance;}
      set {
        // Make sure provided value fits into double
        if (value < _sqrt_max_double) {
          _limit_distance = value;
          _limit_distance2 = value * value;
        } else {
          _limit_distance = _sqrt_max_double;
          _limit_distance2 = Double.MaxValue;
        }
      }
    }
    
    /// <value>
    /// Access the squared maximum distance 
    /// </value>
    protected double SquaredDistanceLimit {
      get {
        return _limit_distance2;
      }
    }

    /// <summary>
    /// Searches for all vectors fulfilling a provided predicate
    /// Search is stopped when the number of elements found reaches the limit.
    /// </summary>
    protected List<T> ExhaustiveSearchLeaf(KdNode<T> leaf, ref int found, Predicate<T> pred) {
      if (leaf.Intermediate) {
        throw new InvalidOperationException();
      }

      List<T> list = new List<T>();
      using (IEnumerator<T> e = leaf.Vectors.GetEnumerator()) {
        while (e.MoveNext() && found < this.CountLimit) {
          if (pred(e.Current)) {
            list.Add(e.Current);
            found += 1;
          }
        }
      }
      return list;
    }

    protected class ExactMatchPredicate {
      public ExactMatchPredicate(IVector reference) {
        _reference = reference;
      }

      public bool Test(T obj) {
        return VectorComparison.Equal(_reference, obj);
      }

      private IVector _reference;
    }

    private int _limit;
    private double _limit_distance, _limit_distance2;
    private KdNode<T> _tree;
    private double _sqrt_max_double = Math.Sqrt(Double.MaxValue);
  }
}
