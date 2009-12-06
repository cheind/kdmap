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

namespace Accelerators
{
  
  /// <summary>
  /// Axis-aligned bounding box
  /// </summary>
  public class AABB : IBoundingVolume, IEquatable<AABB>
  {
    
    /// <summary>
    /// Create an empty AABB in n-dimensions (min > max).
    /// </summary>
    public AABB(int dimensions)
    {
      _min = new Vector(dimensions, Double.MaxValue);
      _max = new Vector(dimensions, Double.MinValue);
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
        double vi = v[i];
        if (vi < _min[i]) _min[i] = vi;
        if (vi > _max[i]) _max[i] = vi;
      }
    }

    /// <summary>
    /// Enlarge AABBs lower corner to contain the given vector.
    /// </summary>
    public void EnlargeLower<T>(T v) where T : IVector {
      for (int i = 0; i < this.Dimensions; ++i) {
        double vi = v[i];
        if (vi < _min[i]) _min[i] = vi;
      }
    }

    /// <summary>
    /// Enlarge AABBs upper corner to contain the given vector.
    /// </summary>
    public void EnlargeUpper<T>(T v) where T : IVector {
      for (int i = 0; i < this.Dimensions; ++i) {
        double vi = v[i];
        if (vi > _max[i]) _max[i] = vi;
      }
    }

   
    
    /// <summary>
    /// Split AABB into two parts using an axis aligned splitting plane. The plane
    /// is specified by the dimension it is orthogonal to and a position value on the
    /// axis.
    /// </summary>
    public void Split(int dimension, double position, out AABB left, out AABB right) {
      
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
          VectorComparison.Equal(_min, new Vector(this.Dimensions, Double.MaxValue)) &&
          VectorComparison.Equal(_max, new Vector(this.Dimensions, Double.MinValue));
      }
    }
    
    /// <summary>
    /// Reset to empty state.
    /// </summary>
    public void Reset() {
      VectorOperations.Fill(ref _min, Double.MaxValue);
      VectorOperations.Fill(ref _max, Double.MinValue);
    }
    
    /// <value>
    /// Lower corner of AABB.
    /// Changes to the returned vector will affect the internal state of this ABBB.
    /// </value>
    public IVector Lower {
      get {
        return _min;
      }
    }

    /// <summary>
    /// Limit the lower corner to the given limit.
    /// </summary>
    public void LimitLower(IVector limit) {
      for (int i = 0; i < this.Dimensions; ++i) {
        this.LimitLower(i, limit[i]);
      }
    }

    /// <summary>
    /// Limit the lower corner to the given limit.
    /// </summary>
    public void LimitLower(int dimension, double limit) {
      double lo = Lower[dimension];
      if (lo == Double.MaxValue || limit > lo) {
        Lower[dimension] = limit;
      }
    }

    /// <summary>
    /// Limit the upper corner to the given limit.
    /// </summary>
    public void LimitUpper(IVector limit) {
      for (int i = 0; i < this.Dimensions; ++i) {
        this.LimitUpper(i, limit[i]);
      }
    }

    /// <summary>
    /// Limit the lower corner to the given limit.
    /// </summary>
    public void LimitUpper(int dimension, double limit) {
      double up = Upper[dimension];
      if (up == Double.MinValue || limit < up) {
        Upper[dimension] = limit;
      }
    }


    /// <value>
    /// Upper corner of AABB.
    /// Changes to the returned vector will affect the internal state of this ABBB.
    /// </value>
    public IVector Upper {
      get {
        return _max;
      }
    }
    
    /// <value>
    /// Diagonal of AABB as vector from lower corner to upper corner.
    /// This vector is calculated and changes to it will not modify the internal state of this AABB.
    /// </value>
    public IVector Diagonal {
      get {
        return (_max - _min);
      }
    }
    
    /// <value>
    /// Access the center of the AABB.
    /// This vector is calculated and changes to it will not modify the internal state of this AABB.
    /// </value>
    public IVector Center {
      get {
        return (_min + (_max - _min)*0.5);
      }
    }
    
    /// <summary>
    /// Return the AABBs extension in the given dimension
    /// </summary>
    public double Extension(int dimension) {
      return _max[dimension] - _min[dimension];
    }
    
    /// <value>
    /// Access the number of dimensions 
    /// </value>
    public int Dimensions {
      get {
        return _min.Dimensions;
      }
    }

    /// <summary>
    /// Test for equality with zero tolerance
    /// </summary>
    public bool Equals(AABB other) {
      return _min.Equals(other._min) && _max.Equals(other._max);
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
    /// Test if AABB overlaps given AABB
    /// </summary>
    public bool Intersect(AABB aabb) {
      // Perform a per-dimension overlapping interval test and early exit if
      // a single interval is none overlapping
      for (int i = 0; i < this.Dimensions; ++i) {
        if (!OverlapInterval(this.Lower[i], this.Upper[i], aabb.Lower[i], aabb.Upper[i]))
          return false;
      }
      return true;
    }
    
    /// <summary>
    /// Determine the location of the given axis aligned plane relative to the position of the
    /// bounding volume.
    /// </summary>
    public EPlanePosition ClassifyPlane(int dimension, double position) {
      double li = Lower[dimension];
      double ui = Upper[dimension];
      
      if (position < li)
        return EPlanePosition.LeftOfBV;
      else if (position > ui)
        return EPlanePosition.RightOfBV;
      else
        return EPlanePosition.IntersectingBV;
    }

    /// <summary>
    /// Find the point on/in the AABB that is closest to the query.
    /// </summary>
    public void Closest(IVector x, ref IVector closest) {
      for (int i = 0; i < this.Dimensions; ++i) {
        // Closest is given by: lower[i] if x[i] < lower[i], upper[i] if x[i] > upper[i], else
        // it is x[i]
        double xi = x[i];
        if (xi < this.Lower[i])
          closest[i] = this.Lower[i];
        else if (xi > this.Upper[i])
          closest[i] = this.Upper[i];
        else
          closest[i] = xi;
      }
    }
    
    /// <summary>
    /// Calculate the closest point on/inside this AABB to the given query point.
    /// </summary>
    public IVector Closest(IVector x) {
      IVector closest = new Vector(this.Dimensions);
      this.Closest(x, ref closest);
      return closest;
    }

    /// <summary>
    /// Create the union of the left and right bounds.
    /// </summary>
    public static void Union(AABB left, AABB right, ref AABB dest) {
      VectorOperations.Copy(left.Lower, ref dest._min);
      VectorOperations.Copy(left.Upper, ref dest._max);
      // Selective enlargement to cope with possibly unbounded regions in right.
      dest.EnlargeLower(right.Lower); 
      dest.EnlargeUpper(right.Upper);
    }
    
    /// <summary>
    /// Convert to string.
    /// </summary>
    public override string ToString ()
    {
      return string.Format("[AABB: Lower={0}, Upper={1}]", Lower, Upper);
    }

    
    /// <summary>
    /// Test if the given axis aligned plane crosses the AABB
    /// </summary>
    private bool Inside(int dimension, double position) {
      return OverlapInterval(this.Lower[dimension], this.Upper[dimension], position, position);
    }
    
    /// <summary>
    /// Test if two intervals overlap
    /// </summary>
    private bool OverlapInterval(double a_lower, double a_upper, double b_lower, double b_upper) {
      return a_lower <= b_upper && b_lower <= a_upper;
    }
      
    
    private Vector _min;
    private Vector _max;
  }
}
