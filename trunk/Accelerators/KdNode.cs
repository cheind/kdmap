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

namespace Accelerators
{

  /// <summary>
  /// A node in the kd-tree.
  /// </summary>
  public class KdNode<T> : BinaryNode<KdNode<T>> where T : IVector
  {
    /// <value>
    /// Access the split dimension.
    /// </value>
    public int SplitDimension {
      get {
        return _k;
      }
      set {
        _k = value;
      }
    }
    
    /// <value>
    /// Access the location of the split. 
    /// </value>
    public double SplitLocation {
      get {
        return _p;
      }
      set {
        _p = value;
      }
    }

    /// <value>
    /// Axis aligned bounding box of the points contained in this node. This AABB is
    /// the most tightened AABB around the internal points.
    /// </value>
    public AABB InternalBounds {
      get {
        return _aabb_internal;
      }
      set {
        _aabb_internal = value;
      }
    }

    /// <summary>
    /// Axis aligned bounding box of entire node area. The volume of SplitBounds is always
    /// bigger than or equal to the InternalBounds. This property is calculated on the fly.
    /// </summary>
    public AABB SplitBounds {
      get {
        // We start with an empty aabb and walk our way up to root and setting the coordinates
        // of lower and upper accordingly.
        AABB aabb = new AABB(this.InternalBounds.Dimensions);
        foreach (KdNode<T> n in this.Ancestors) {
          if (n.Root) {
            // When the root node is reached the aabb might be unbound on one or
            // more sides. The InternalBounds of root are equal its split bounds.
            // So we limit the unbound sides to those given by the root.
            aabb.LimitLower(n.InternalBounds.Lower);
            aabb.LimitUpper(n.InternalBounds.Upper);
          } else {
            if (n.Parent.ContainsInLeftSubTree(n)) {
              aabb.LimitUpper(n.Parent.SplitDimension, n.Parent.SplitLocation);
            } else { // In right subtree
              aabb.LimitLower(n.Parent.SplitDimension, n.Parent.SplitLocation);
            }
          }
        }
        return aabb;
      }
    }
    
    /// <value>
    /// Vectors in within the bounds of this node.
    /// </value>
    public List<T> Vectors {
      get {
        return _vectors;
      }
      set {
        _vectors = value;
      }
    }
    
    /// <summary>
    /// Convert to string
    /// </summary>
    public override string ToString ()
    {
      return string.Format("[KdNode: SplitDimension={0}, SplitLocation={1}, Leaf={2}, Bounds={3}]", SplitDimension, SplitLocation, Leaf, InternalBounds);
    }

    
    private int _k;
    private double _p;
    private AABB _aabb_internal;
    private List<T> _vectors;
  }
}