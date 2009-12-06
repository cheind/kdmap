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
  /// Defines the position of an axis aligned plane relative to a bounding volume.
  /// </summary>
  public enum EPlanePosition{
    LeftOfBV,
    RightOfBV,
    IntersectingBV
  }
  
  /// <summary>
  /// Interface that defines a bounding volume. Implementing this interface allowes efficient
  /// bounding volume queries using kd-tree searches.
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
    /// Touching is considered to be intersecting.
    /// </summary>
    bool Intersect(AABB aabb);
    
    /// <summary>
    /// Determine the location of the given axis aligned plane relative to the position of the
    /// bounding volume.
    /// </summary>
    EPlanePosition ClassifyPlane(int dimension, double position);
    
  }
}
