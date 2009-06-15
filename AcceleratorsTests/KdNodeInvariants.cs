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


    public static void AreMetBy<T>(KdNode<T> root) where T : IVector {
      foreach (KdNode<T> n in root.PreOrder) {
        if (n.Intermediate) {
          AreMetByIntermediate(n);
        } else {
          AreMetByChild(n);
        }
      }
    }

    private static void AreMetByChild<T>(KdNode<T> leaf) where T : IVector {
      // No child nodes are present
      Assert.IsNull(leaf.Left);
      Assert.IsNull(leaf.Right);
      // Empty vector of elements present
      Assert.NotNull(leaf.Vectors);
      // Each element must be contained in the inner bounds and split bounds up to root
      // Additionally the element has to lie to the left of each ancestor node if the element is behind or
      // on the split plane. It has to lie to the right of each ancestor node if the element is in front of 
      // the split plane.
      foreach (T t in leaf.Vectors) {
        KdNode<T> previous = leaf; // Used to track left/right while walking upwards.
        foreach (KdNode<T> n in leaf.Ancestors) {
          Assert.IsTrue(n.SplitBounds.Inside(t));
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

    private static void AreMetByIntermediate<T>(KdNode<T> n) where T : IVector {
      // Each intermediate node has two children
      Assert.NotNull(n.Left);
      Assert.NotNull(n.Right);
      // If not root, each node has a parent which itself references the node in either left or righ
      // child property
      if (!n.Root) {
        Assert.NotNull(n.Parent);
        Assert.IsTrue(n.Parent.Left == n || n.Parent.Right == n);
      }
      // Split dimension must be within bounds
      Assert.Less(n.SplitDimension, n.SplitBounds.Dimensions);
      // Split location must be within bounds
      Assert.LessOrEqual(n.SplitBounds.Lower[n.SplitDimension], n.SplitLocation);
      Assert.GreaterOrEqual(n.SplitBounds.Upper[n.SplitDimension], n.SplitLocation);
      // The split bounding volume of the children must represent the split plane of the node
      Assert.AreEqual(n.Left.SplitBounds.Upper[n.SplitDimension], n.SplitLocation, FloatComparison.DefaultEps);
      Assert.AreEqual(n.Right.SplitBounds.Lower[n.SplitDimension], n.SplitLocation, FloatComparison.DefaultEps);
      // The inner bounding volume must be contained in the split bounding volume
      for (int i = 0; i < n.SplitBounds.Dimensions; ++i) {
        Assert.LessOrEqual(n.SplitBounds.Lower[i], n.InternalBounds.Lower[i]);
        Assert.GreaterOrEqual(n.SplitBounds.Upper[i], n.InternalBounds.Upper[i]);
      }
    }
	}
}
