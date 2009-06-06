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
      _radius = 0.0f;
    }
    
    /// <summary>
    /// Instance a hypersphere with center and radius.
    /// </summary>
    public Ball(IVector center, float radius) 
    {
      _center = new Vector(center);
      this.Radius = radius;
    }
    
    /// <value>
    /// Access the radius of the ball. 
    /// </value>
    public float Radius {
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
    public float SquaredRadius {
      get {
        return _radius2;
      }
      set {
        _radius2 = value;
        _radius = (float)Math.Sqrt(value);
      }
    }
    
    /// <value>
    /// Access the center of the ball. 
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
    public EPlanePosition ClassifyPlane(int dimension, float position) {
      float li = Center[dimension] - Radius;
      float ui = Center[dimension] + Radius;
      
      if (position < li)
        return EPlanePosition.LeftOfBV;
      else if (position > ui)
        return EPlanePosition.RightOfBV;
      else
        return EPlanePosition.IntersectingBV;
    }
    
    
    private Vector _center;
    private float _radius;
    private float _radius2;
  }
}
