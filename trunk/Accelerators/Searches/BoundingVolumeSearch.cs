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
using System.Text;

namespace Accelerators.Searches {

  /// <summary>
  /// Search for all elements within bounding volume
  /// </summary>
  public class BoundingVolumeSearch<T> : KdTreeSearch<T> where T : IVector {

     /// <summary>
    /// Initialize with the kd-tree node to start search at.
    /// </summary>
    public BoundingVolumeSearch(KdNode<T> tree) : base(tree) { }

    /// <summary>
    /// Search for elements within the volume defined by the bounding volume.
    /// </summary>
    public IEnumerable<T> FindInsideBoundingVolume(IBoundingVolume bv) {
      if (!bv.Intersect(this.Tree.InternalBounds))
        yield break;

      // Init search
      Stack<KdNode<T>> s = new Stack<KdNode<T>>();
      s.Push(this.Tree);
      int found = 0;
      // Run
      while (s.Count > 0 && found < this.CountLimit) {
        KdNode<T> n = s.Pop();
        if (n.Leaf) {
          // Once we encounter a leaf an exhaustive search is performed for all elements inside
          // the bounding volume up to the adjusted limit.
          List<T> elements = this.ExhaustiveSearchLeaf(n, ref found, delegate(T obj) { return bv.Inside(obj); });
          foreach (T t in elements)
            yield return t;
        } else { // Intermediate node
          // Classify against split plane
          EPlanePosition pos = bv.ClassifyPlane(n.SplitDimension, n.SplitLocation);
          if (pos == EPlanePosition.LeftOfBV) {
            s.Push(n.Right);
          } else if (pos == EPlanePosition.RightOfBV) {
            s.Push(n.Left);
          } else { // Intersecting
            s.Push(n.Right);
            s.Push(n.Left);
          }
        }
      }
    }

  }
}
