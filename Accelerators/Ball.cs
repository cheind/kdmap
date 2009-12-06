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
  /// Represents an n-dimensional sphere
  /// </summary>
  public class Ball : IBoundingVolume
  {
    
    /// <summary>
    /// Instance a hypersphere of n-dimensions.
    /// </summary>
    public Ball(int dimensions)
    {
      _center = new Vector(dimensions);
      _radius = 0.0;
    }
    
    /// <summary>
    /// Create a hypersphere given center and radius.
    /// </summary>
    public Ball(IVector center, double radius) 
    {
      _center = new Vector(center);
      this.Radius = radius;
    }
    
    /// <value>
    /// Access the radius of the ball. 
    /// </value>
    public double Radius {
      get {
        return _radius;
      }
      set {
        _radius = value;
        _radius2 = _radius * _radius;
      }
    }
    
    /// <value>
    /// Access the squared radius
    /// </value>
    public double SquaredRadius {
      get {
        return _radius2;
      }
      set {
        _radius2 = value;
        _radius = (double)Math.Sqrt(value);
      }
    }
    
    /// <value>
    /// Access the center of the ball. 
    /// Changes to the returned vector will affect the internal state of this ball.
    /// </value>
    public IVector Center {
      get {
        return _center;
      }
    }
    
    /// <value>
    /// Access the dimensionality of the ball. 
    /// </value>
    public int Dimensions {
      get {
        return _center.Dimensions;
      }
    }
    
    /// <summary>
    /// Test if vector is inside of ball.
    /// </summary>
    public bool Inside(IVector x) {
      Vector v = _center - x;
      return VectorReductions.SquaredL2Norm(v) <= this.SquaredRadius;
    }
    
    /// <summary>
    /// Test if AABB intersects with this ball.
    /// </summary>
    public bool Intersect(AABB aabb) {
      IVector closest = aabb.Closest(this.Center);
      return Inside(closest);
    }
    
    /// <summary>
    /// Determine the location of the given axis aligned plane relative to the position of the
    /// bounding volume.
    /// </summary>
    public EPlanePosition ClassifyPlane(int dimension, double position) {
      double li = Center[dimension] - Radius;
      double ui = Center[dimension] + Radius;
      
      if (position < li)
        return EPlanePosition.LeftOfBV;
      else if (position > ui)
        return EPlanePosition.RightOfBV;
      else
        return EPlanePosition.IntersectingBV;
    }
    
    
    private Vector _center;
    private double _radius;
    private double _radius2;
  }
}
