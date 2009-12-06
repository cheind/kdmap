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

namespace Accelerators.Subdivision
{
  
  /// <summary>
  /// In case of a trivial split this method slides the plane towards an existing element
  /// so that at least one element is assigned to the empty side.
  /// </summary>
  public class SlidingPlaneResolver : ITrivialSplitResolver
  {

    public double Resolve<T> (KdNode<T> target, int split_dimension, double split_location, ETrivialSplitType tst) where T : IVector   
    {
      if (tst == ETrivialSplitType.EmptyLeft)
        return ResolveLeftEmpty(target, split_dimension, split_location);
      else if (tst == ETrivialSplitType.EmptyRight)
        return ResolveRightEmpty(target, split_dimension, split_location);
      else
        throw new SubdivisionException();
    }
    
    /// <summary>
    /// Resolves a split that caused the left partition to be empty
    /// </summary>
    private double ResolveLeftEmpty<T> (KdNode<T> target, int split_dimension, double split_location) where T : IVector {
      // Because of the property that all elements <= split location are put in the left cell, we search for the closest
      // element to the chosen split location. We can safely assume that there is no element that equals the split location.
      return this.FindClosest(target, split_dimension, split_location, false);
    }
    
    /// <summary>
    /// Resolves a split that caused the right partion to be empty.
    /// </summary>
    private double ResolveRightEmpty<T> (KdNode<T> target, int split_dimension, double split_location) where T : IVector {
      // Because of the property that all elements > split_location are put in the right partition, we need to search for the
      // closest element to the split location with the restriction that the distance is greater than zero.
      
      // First pass, find the closest element
      double best = this.FindClosest(target, split_dimension, split_location, false);
      return this.FindClosest(target, split_dimension, best, true); 
    }
    
    private double FindClosest<T> (KdNode<T> target, int split_dimension, double split_location, bool differ) where T : IVector {
      List<T> vecs = target.Vectors;
      Pair<double, double> best = new Pair<double, double>(Double.MaxValue, split_location);
      for (int i = 0; i < vecs.Count; ++i) {
        double e = vecs[i][split_dimension];
        double dist = Math.Abs(e - split_location);
        if (dist < best.First && (!differ || dist > 0)) {
          best.First = dist;
          best.Second = e;
        }
      }
      return best.Second; 
    }
  }
}
