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
    public static void Sub<T>(IVector a, IVector b, ref T dest) where T : IVector {
      for (int i = 0; i < a.Dimensions; ++i) {
        dest[i] = a[i] - b[i];
      }
    }
    
    /// <summary>
    /// Calculate component-wise addition of a and b.
    /// Equal dimensionality assumed and dimensions of resulting vector must be pre-allocated.
    /// </summary>
    public static void Add<T>(IVector a, IVector b, ref T dest) where T : IVector {
      for (int i = 0; i < a.Dimensions; ++i) {
        dest[i] = a[i] + b[i];
      }
    }
    
    /// <summary>
    /// Calculate the inner product of two vectors.
    /// </summary>
    public static double Inner(IVector a, IVector b) {
      double ip = 0.0;
      for (int i = 0; i < a.Dimensions; ++i) {
        ip += a[i]*b[i];
      }
      return ip;
    }
    
    /// <summary>
    /// Calculate the component-wise multiplication with a scalar
    /// </summary>
    public static void ScalarMul<T>(IVector a, double s, ref T dest) where T : IVector {
      for (int i = 0; i < a.Dimensions; ++i) {
        dest[i] = a[i] * s;
      }
    }
    

    
    /// <summary>
    /// Normalize the given vector using the L2 norm.
    /// </summary>
    public static double Normalize<T>(IVector a, ref T dest) where T : IVector {
      double len = VectorReductions.L2Norm(a);
      if (FloatComparison.CloseZero(len, FloatComparison.DefaultEps))
        throw new DivideByZeroException();
      double inv_len = 1.0/len;
      for (int i = 0; i < a.Dimensions; ++i) {
        dest[i] = a[i] * inv_len;
      }
      return len;
    }
    
    /// <summary>
    /// Copy components of one vector to destination vector
    /// </summary>
    public static void Copy<T>(IVector a, ref T dest) where T : IVector {
      for (int i = 0; i < a.Dimensions; ++i) {
        dest[i] = a[i];
      }
    }
    
    /// <summary>
    /// Set each coordinate to the given value
    /// </summary>
    public static void Fill<T>(ref T a, double val) where T : IVector { 
      for (int i = 0; i < a.Dimensions; ++i) {
        a[i] = val;
      }
    }
  }
}
