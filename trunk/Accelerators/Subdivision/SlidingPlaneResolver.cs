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
