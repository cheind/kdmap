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
