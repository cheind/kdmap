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
  
  /// <summary>
  /// Axis-aligned bounding box
  /// </summary>
  public class AABB
  {
    
    /// <summary>
    /// Create an empty AABB in n-dimensions (min > max).
    /// </summary>
    public AABB(int dimensions)
    {
      _min = new Vector(dimensions, Single.MaxValue);
      _max = new Vector(dimensions, -Single.MaxValue);
    }
    
    /// <summary>
    /// Copy construct AABB
    /// </summary>
    public AABB(AABB other) {
      _min = new Vector(other._min);
      _max = new Vector(other._max);
    }
    
    /// <summary>
    /// Construct from two corner points.
    /// </summary>
    public AABB(IVector lower, IVector upper) {
      _min = new Vector(lower);
      _max = new Vector(upper);
    }
    
    /// <summary>
    /// Enlarge AABB to contain the given vectors.
    /// </summary>
    public void Enlarge<T>(IEnumerable<T> values) where T : IVector {
      foreach(IVector v in values) {
        this.Enlarge(v);
      }
    }
    
    /// <summary>
    /// Enlarge AABB to contain given vector.
    /// </summary>
    public void Enlarge<T>(T v) where T : IVector {
      for(int i = 0; i < this.Dimensions; ++i) {
        float vi = v[i];
        if (vi < _min[i]) _min[i] = vi;
        if (vi > _max[i]) _max[i] = vi;
      }
    }
    
    /// <summary>
    /// Split AABB into two parts using an axis aligned splitting plane. The plane
    /// is specified by the dimension it is orthogonal to and a position value on the
    /// axis.
    /// </summary>
    public void Split(int dimension, float position, out AABB left, out AABB right) {
      
      if (!Inside(dimension, position))
        throw new ArgumentException("Split plane is outside of AABB");
      
      // -> x
      // |           upperL     upperR
      // v  +--------+--------+
      // y  |        |        |
      //    | left   | right  |
      //    |        |        |
      //    |        |        |
      //    +--------+--------+
      //    lowerL     lowerR
      
      left = new AABB(this);
      right = new AABB(this);
      left.Upper[dimension] = position;
      right.Lower[dimension] = position;
    }
    
    /// <value>
    /// Test if AABB is empty. 
    /// </value>
    public bool Empty {
      get {
        return 
          VectorComparison.Equal(_min, new Vector(this.Dimensions, Single.MaxValue)) &&
          VectorComparison.Equal(_max, new Vector(this.Dimensions, -Single.MaxValue));
      }
    }
    
    /// <summary>
    /// Reset to empty state.
    /// </summary>
    public void Reset() {
      VectorOperations.Fill(_min, Single.MaxValue);
      VectorOperations.Fill(_max, -Single.MaxValue);
    }
    
    /// <value>
    /// Access the number of dimensions 
    /// </value>
    public int Dimensions {
      get {
        return _min.Dimensions;
      }
    }
    
    /// <value>
    /// Lower corner of AABB.
    /// </value>
    public IVector Lower {
      get {
        return _min;
      }
    }
    
    
    /// <value>
    /// Upper corner of AABB.
    /// </value>
    public IVector Upper {
      get {
        return _max;
      }
    }
    
    /// <value>
    /// Diagonal of AABB as vector from lower corner to upper corner 
    /// </value>
    public IVector Diagonal {
      get {
        return (_max - _min);
      }
    }
    
    /// <summary>
    /// Return the AABBs extension in the given dimension
    /// </summary>
    public float Extension(int dimension) {
      return _max[dimension] - _min[dimension];
    }
    
    /// <summary>
    /// Test if given vector is contained in AABB
    /// </summary>
    public bool Inside(IVector x) {
      for (int i = 0; i < this.Dimensions; ++i) {
        if (!Inside(i, x[i]))
          return false;
      }
      return true;
    }
    
    /// <summary>
    /// Test if the given axis aligned plane crosses the AABB
    /// </summary>
    private bool Inside(int dimension, float position) {
      return this.Lower[dimension] <= position && this.Upper[dimension] >= position;
    }
      
    
    private Vector _min;
    private Vector _max;
  }
}
