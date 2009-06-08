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
