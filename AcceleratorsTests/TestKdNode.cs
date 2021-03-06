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
using NUnit.Framework;
using Accelerators;
using System.Collections.Generic;

namespace AcceleratorsTests
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
