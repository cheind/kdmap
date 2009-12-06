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
using NUnit.Framework;
using Accelerators;
using Accelerators.Subdivision;

namespace AcceleratorsTests
{
  /// <summary>
  /// Encapsulates tests to verify that kd-tree invariants are true.
  /// </summary>
	public class KdNodeInvariants
	{

    /// <summary>
    /// Test if node adheres kd-tree invariants.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="root"></param>
    public static void AreMetBy<T>(KdNode<T> root) where T : IVector {
      foreach (KdNode<T> n in root.PreOrder) {
        if (n.Intermediate) {
          AreMetByIntermediate(n);
        } else {
          AreMetByLeaf(n);
        }
      }
    }

    /// <summary>
    /// Test invariants on leaf
    /// </summary>
    private static void AreMetByLeaf<T>(KdNode<T> leaf) where T : IVector {
      // No child nodes are present
      Assert.IsNull(leaf.Left);
      Assert.IsNull(leaf.Right);
      // Empty vector of elements present
      Assert.IsNotNull(leaf.Vectors);
      // Each element must be contained in the inner bounds and split bounds up to root
      // Additionally the element has to lie to the left of each ancestor node if the element is behind or
      // on the split plane. It has to lie to the right of each ancestor node if the element is in front of 
      // the split plane.
      foreach (T t in leaf.Vectors) {
        KdNode<T> previous = leaf; // Used to track left/right while walking upwards.
        foreach (KdNode<T> n in leaf.Ancestors) {
          Assert.IsTrue(n.InternalBounds.Inside(t));
          Assert.IsTrue(n.InternalBounds.Inside(t));
          if (n.Intermediate) {
            if (previous == n.Left)
              Assert.LessOrEqual(t[n.SplitDimension], n.SplitLocation);
            else if (previous == n.Right)
              Assert.Greater(t[n.SplitDimension], n.SplitLocation);
            else
              Assert.Fail();
          }
          previous = n;
        }
      }
    }

    /// <summary>
    /// Test invariants on children
    /// </summary>
    private static void AreMetByIntermediate<T>(KdNode<T> n) where T : IVector {
      // Each intermediate node has two children
      Assert.IsNotNull(n.Left);
      Assert.IsNotNull(n.Right);
      // If not root, each node has a parent which itself references the node in either left or righ
      // child property
      if (!n.Root) {
        Assert.IsNotNull(n.Parent);
        Assert.IsTrue(n.Parent.Left == n || n.Parent.Right == n);
      }
      // Split dimension must be within bounds
      Assert.Less(n.SplitDimension, n.InternalBounds.Dimensions);
      // Split location must be within bounds
      Assert.LessOrEqual(n.SplitBounds.Lower[n.SplitDimension], n.SplitLocation);
      Assert.GreaterOrEqual(n.SplitBounds.Upper[n.SplitDimension], n.SplitLocation);
      // The split bounding volume of the children must represent the split plane of the node
      Assert.AreEqual(n.Left.SplitBounds.Upper[n.SplitDimension], n.SplitLocation, FloatComparison.DefaultEps);
      Assert.AreEqual(n.Right.SplitBounds.Lower[n.SplitDimension], n.SplitLocation, FloatComparison.DefaultEps);
      // The inner bounding volume must be contained in the split bounding volume
      for (int i = 0; i < n.InternalBounds.Dimensions; ++i) {
        Assert.LessOrEqual(n.SplitBounds.Lower[i], n.InternalBounds.Lower[i]);
        Assert.GreaterOrEqual(n.SplitBounds.Upper[i], n.InternalBounds.Upper[i]);
      }
    }
	}
}
