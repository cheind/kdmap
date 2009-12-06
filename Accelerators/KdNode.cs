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