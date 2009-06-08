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
  /// Defines the position of an axis aligned plane relative to a bounding volume.
  /// </summary>
  public enum EPlanePosition{
    LeftOfBV,
    RightOfBV,
    IntersectingBV
  }
  
  /// <summary>
  /// Interface that allows bounding volume queries.
  /// </summary>
  public interface IBoundingVolume
  { 
    /// <value>
    /// Access the dimensionality of the bounding volume.
    /// </value>
    int Dimensions {
      get;
    }
    
    /// <summary>
    /// Test if vector is contained in bounding volume.
    /// A vector lying on the bounding volume's surface is considered to be contained.
    /// </summary>
    bool Inside(IVector x);
    
    /// <summary>
    /// Test if bounding volume intersects with the given axis aligned bounding box.
    /// </summary>
    bool Intersect(AABB aabb);
    
    /// <summary>
    /// Determine the location of the given axis aligned plane relative to the position of the
    /// bounding volume.
    /// </summary>
    EPlanePosition ClassifyPlane(int dimension, double position);
    
  }
}
