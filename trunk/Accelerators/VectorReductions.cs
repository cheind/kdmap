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
  /// Reduction on vectors
  /// </summary>
  public class VectorReductions
  {
    /// <summary>
    /// Calculate the square-length (L2 norm) of the given vector.
    /// </summary>
    public static double SquaredL2Norm(IVector a) {
      return VectorOperations.Inner(a, a);
    }
    
    /// <summary>
    /// Calculate the length of the vector
    /// </summary>
    public static double L2Norm(IVector a) {
      return (double)Math.Sqrt(SquaredL2Norm(a));
    }

    /// <summary>
    /// Calculate the squared distance (L2 norm) between two vectors.
    /// </summary>
    public static double SquaredL2NormDistance(IVector a, IVector b) {
      double dist2 = 0.0;
      for (int i = 0; i < a.Dimensions; ++i) {
        double delta = b[i] - a[i];
        dist2 += delta * delta;
      }
      return dist2;
    }
    
    /// <summary>
    /// Calculate the index of the element having the maximum absolut value.
    /// Assumes at least a one-dimensional vector.
    /// </summary>
    public static int IndexNormInf(IVector a) {
      double max_val = Math.Abs(a[0]);
      int max_index = 0;
      
      for (int i = 1; i < a.Dimensions; ++i) {
        double val = Math.Abs(a[i]);
        if (val > max_val) {
          max_val = val;
          max_index = i;
        }
      }
      return max_index;
    }
  }
}