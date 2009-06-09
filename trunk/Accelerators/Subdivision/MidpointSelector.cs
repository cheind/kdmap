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
  /// Selects the split location based on the midpoint of the chosen split axis.
  /// This might generate leaf nodes that are empty.
  /// </summary>
  public class MidpointSelector : ISplitLocationSelector
  {

    /// <summary>
    /// Selects the split location based on the midpoint of the chosen split axis.
    /// This might generate leaf nodes that are empty. Assumes split dimension has a positive spread
    /// </summary>
    public double Select<T>(KdNode<T> target, int split_dimension) where T : IVector
    {
      double split = target.Bounds.Lower[split_dimension] + target.Bounds.Extension(split_dimension) * 0.5;
      return split;
    }
     
  }
}