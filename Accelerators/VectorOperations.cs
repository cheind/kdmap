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

namespace Accelerators
{
  
  
  /// <summary>
  /// Defines unary and binary operations on vectors
  /// </summary>
  public class VectorOperations
  {
    /// <summary>
    /// Calculate component-wise subtraction of a and b. 
    /// Equal dimensionality assumed and dimensions of resulting vector must be pre-allocated.
    /// </summary>
    public static void Sub(IVector a, IVector b, IVector dest) {
      for (int i = 0; i < a.Dimensions; ++i) {
        dest[i] = a[i] - b[i];
      }
    }
    
    /// <summary>
    /// Calculate component-wise addition of a and b.
    /// Equal dimensionality assumed and dimensions of resulting vector must be pre-allocated.
    /// </summary>
    public static void Add(IVector a, IVector b, IVector dest) {
      for (int i = 0; i < a.Dimensions; ++i) {
        dest[i] = a[i] + b[i];
      }
    }
    
    /// <summary>
    /// Calculate the inner product of two vectors.
    /// </summary>
    public static float Inner(IVector a, IVector b) {
      float ip = 0.0f;
      for (int i = 0; i < a.Dimensions; ++i) {
        ip += a[i]*b[i];
      }
      return ip;
    }
    
    /// <summary>
    /// Calculate the component-wise multiplication with a scalar
    /// </summary>
    public static void ScalarMul(IVector a, float s, IVector dest) {
      for (int i = 0; i < a.Dimensions; ++i) {
        dest[i] = a[i] * s;
      }
    }
    

    
    /// <summary>
    /// Normalize the given vector using the L2 norm.
    /// </summary>
    public static float Normalize(IVector a, IVector dest) {
      float len = VectorReductions.L2Norm(a);
      if (FloatComparison.CloseZero(len, FloatComparison.DefaultEps))
        throw new DivideByZeroException();
      float inv_len = 1.0f/len;
      for (int i = 0; i < a.Dimensions; ++i) {
        dest[i] = a[i] * inv_len;
      }
      return len;
    }
    
    /// <summary>
    /// Copy components of one vector to destination vector
    /// </summary>
    public static void Copy(IVector a, IVector dest) {
      for (int i = 0; i < a.Dimensions; ++i) {
        dest[i] = a[i];
      }
    }
    
    /// <summary>
    /// Set each coordinate to the given value
    /// </summary>
    public static void Fill(IVector a, float val) { 
      for (int i = 0; i < a.Dimensions; ++i) {
        a[i] = val;
      }
    }
  }
}
