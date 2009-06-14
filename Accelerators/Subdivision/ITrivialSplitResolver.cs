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

namespace Accelerators.Subdivision
{
  /// <summary>
  /// Declares possible trivial splits
  /// </summary>
  public enum ETrivialSplitType {
    EmptyLeft, // trivial split, no points on left side (<= split location)
    EmptyRight, // trivial split, no points on right side (> split location)
    NoTrivial // marks a none trivial split
  }

  /// <summary>
  /// Interface to be implemented by classes that resolve trivial splits as produces by ISplitLocationSelector.
  /// </summary>
  public interface ITrivialSplitResolver
  {
    /// <summary>
    /// Possible resolve a trivial split.
    /// </summary>
    double Resolve<T>(KdNode<T> target, int split_dimension, double split_location, ETrivialSplitType tst) where T : IVector;
  }
}
