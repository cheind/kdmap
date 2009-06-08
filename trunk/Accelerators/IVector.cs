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
  /// Interface for n-dimensional vectors.
  /// </summary>
  /// 
  /// <remarks>
  /// This is the only interface to be implemented by custom classes that are to 
  /// be used in conjunction with classes found in the Accelerators namespace.
  /// </remarks>
  public interface IVector
  {
    /// <summary>
    /// Access the number of coordinates.
    /// </summary>
    int Dimensions {
      get;
    }
    
    /// <value>
    /// Access a single coordinate. 
    /// </value>
    double this[int index] {
      get;
      set;
    }
  }
}
