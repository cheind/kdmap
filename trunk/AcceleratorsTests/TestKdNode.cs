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
using NUnit.Framework;
using Accelerators;
using System.Collections.Generic;

namespace TestBinaryNode
{
  [TestFixture()]
	public class TestKdNode
	{
    [Test]
    public void TestSplitBounds() {
      
      // +-------------+
      // |      |      |
      // |      |      |
      // +--+---|      |
      // |  | x |      |
      // +--+---+------+
      
      KdNode<Vector> root = new KdNode<Vector>();
      root.InternalBounds = new AABB(Vector.Create(-5, -5), Vector.Create(5, 5));
      root.SplitDimension = 0;
      root.SplitLocation = 0;
      KdNode<Vector> left = root.SetLeftChild(new KdNode<Vector>());
      left.SplitDimension = 1;
      left.SplitLocation = 0;
      left = left.SetLeftChild(new KdNode<Vector>());
      left.SplitDimension = 0;
      left.SplitLocation = -2.5;
      KdNode<Vector> right = left.SetRightChild(new KdNode<Vector>());
      right.InternalBounds = new AABB(2);

      AABB bounds = right.SplitBounds;
      Assert.AreEqual(bounds.Lower[0], -2.5, FloatComparison.DefaultEps);
      Assert.AreEqual(bounds.Lower[1], -5, FloatComparison.DefaultEps);
      Assert.AreEqual(bounds.Upper[0], 0, FloatComparison.DefaultEps);
      Assert.AreEqual(bounds.Upper[1], 0, FloatComparison.DefaultEps);
    }
	}
}
