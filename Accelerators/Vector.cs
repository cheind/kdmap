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
  /// N-Dimensional vector implementation
  /// </summary>
  public class Vector : IVector, IEquatable<IVector>
  {
    /// <summary>
    /// Create a vector that can hold n-dimensions.
    /// </summary>
    public Vector(int dimensions) {
      _coordinates = new double[dimensions];
    }
    
    /// <summary>
    /// Create vector of n-dimensions with equal coordinates
    /// </summary>
    public Vector(int dimensions, double val) {
      _coordinates = new double[dimensions];
      IVector self = this;
      VectorOperations.Fill(ref self, val);
    }

    /// <summary>
    /// Create a two-dimensional vector with coordinates explicitly set.
    /// </summary>
    public static Vector Create(double x, double y) {
      Vector v = new Vector(2);
      v[0] = x;
      v[1] = y;
      return v;
    }

    /// <summary>
    /// Create a three-dimesional vector with coordinates explicitly set.
    /// </summary>
    public static Vector Create(double x, double y, double z) {
      Vector v = new Vector(3);
      v[0] = x;
      v[1] = y;
      v[2] = z;
      return v;
    }

    /// <summary>
    /// Create a one-dimensional vector with coordinates explicitly set.
    /// </summary>
    public static Vector Create(double x) {
      Vector v = new Vector(1);
      v[0] = x;
      return v;
    }
    
    /// <summary>
    /// Copy construct from the given vector
    /// </summary>
    public Vector(Vector other) {
      _coordinates = (double[])other._coordinates.Clone();
    }
    
    /// <summary>
    /// Copy construct from a vector implementing the IVector interface
    /// </summary>
    public Vector(IVector other) {
      _coordinates = new double[other.Dimensions];
      for (int i = 0; i < Dimensions ; ++i )
        _coordinates[i] = other[i];
    }

    #region IVector implementation
    public int Dimensions {
      get {
        return _coordinates.Length;
      }
    }
    
    public double this[int index] {
      get {
        return _coordinates[index];
      }
      set {
        _coordinates[index] = value;
      }
    }
    #endregion
    
    public static Vector operator+(Vector lhs, IVector rhs) {
      Vector res = new Vector(lhs.Dimensions);
      VectorOperations.Add(lhs, rhs, ref res);
      return res;
    }
    
    public static Vector operator-(Vector lhs, IVector rhs) {
      Vector res = new Vector(lhs.Dimensions);
      VectorOperations.Sub(lhs, rhs, ref res);
      return res;
    }
    
    public static Vector operator*(Vector lhs, double s) {
      Vector res = new Vector(lhs.Dimensions);
      VectorOperations.ScalarMul(lhs, s, ref res);
      return res;
    }
    
    public double L2Norm {
      get {
        return VectorReductions.L2Norm(this);
      }
    }
    
    public override string ToString ()
    {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      sb.Append('[');
      for(int i = 0; i < Dimensions; ++i) {
        sb.Append(_coordinates[i]);
        if (i < Dimensions - 1)
          sb.Append(',');
      }
      sb.Append(']');
      return sb.ToString();
    }

    /// <summary>
    /// Component-wise equality with zero tolerance.
    /// </summary>
    public bool Equals(IVector other) {
      return VectorComparison.Equal(this, other);
    }
    
    private double[] _coordinates;
  }
}
