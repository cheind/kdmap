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
using Accelerators.Subdivision;
using System.Collections.Generic;

namespace AcceleratorsTests
{
  
  
  [TestFixture()]
  public class TestSubdivisionPolicyConnector
  {
    
    [Test]
    [ExpectedException(typeof(BucketSizeException))]
    public void TestBucketSize() {
      SubdivisionPolicyConnector c = new SubdivisionPolicyConnector(10);

      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[] { Vector.Create(1.0, 1.0), Vector.Create(2.0, 3.0), Vector.Create(3.0, 1.0), Vector.Create(4.0, 1.0) });
      n.InternalBounds = new AABB(2);
      n.InternalBounds.Enlarge<Vector>(n.Vectors);
      n.InternalBounds = new AABB(n.InternalBounds);
      c.Split(n);
    }
    
    [Test]
    [ExpectedException(typeof(IntermediateNodeException))]
    public void TestIntermediateNode() {
      SubdivisionPolicyConnector c = new SubdivisionPolicyConnector(1);


      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[] { Vector.Create(1.0, 1.0), Vector.Create(2.0, 3.0), Vector.Create(3.0, 1.0), Vector.Create(4.0, 1.0) });
      n.InternalBounds = new AABB(2);
      n.InternalBounds.Enlarge<Vector>(n.Vectors);
      n.InternalBounds = new AABB(n.InternalBounds);
      c.Split(n);
      c.Split(n); // split again
    }
    
    [Test]
    public void TestSplitOneDimensional() {
      ISubdivisionPolicy p = SubdivisionPolicyConnector.CreatePolicy<AxisOfMaximumSpreadSelector, MidpointSelector, SlidingPlaneResolver>(1);
      
      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[] { Vector.Create(-1.0), Vector.Create(1.0), Vector.Create(3.0), Vector.Create(2.0) });
      n.InternalBounds = new AABB(1);
      n.InternalBounds.Enlarge<Vector>(n.Vectors);
      p.Split(n);

      Assert.AreEqual(1.0, n.SplitLocation, FloatComparison.DefaultEps);
      Assert.AreEqual(0, n.SplitDimension);

      KdNode<Vector> left = n.Left;
      KdNode<Vector> right = n.Right;

      Assert.AreEqual(2, left.Vectors.Count);
      Assert.AreEqual(-1.0, left.Vectors[0][0], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, left.Vectors[1][0], FloatComparison.DefaultEps);
      
      Assert.AreEqual(2, right.Vectors.Count);
      Assert.AreEqual(3.0, right.Vectors[0][0], FloatComparison.DefaultEps);
      Assert.AreEqual(2.0, right.Vectors[1][0], FloatComparison.DefaultEps);

      Assert.AreEqual(-1.0, left.SplitBounds.Lower[0], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, left.SplitBounds.Upper[0], FloatComparison.DefaultEps);

      Assert.AreEqual(1.0, right.SplitBounds.Lower[0], FloatComparison.DefaultEps);
      Assert.AreEqual(3.0, right.SplitBounds.Upper[0], FloatComparison.DefaultEps);
    }
    
    [Test]
    public void TestSplitMultiDimensional() {
      ISubdivisionPolicy p = SubdivisionPolicyConnector.CreatePolicy<AxisOfMaximumSpreadSelector, MidpointSelector, SlidingPlaneResolver>(1);

      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[] { Vector.Create(1.0, 1.0), Vector.Create(1.0, -1.0), Vector.Create(1.0, 3.0), Vector.Create(1.0, 2.0) });
      n.InternalBounds = new AABB(2);
      n.InternalBounds.Enlarge<Vector>(n.Vectors);
      p.Split(n);

      Assert.AreEqual(1.0, n.SplitLocation, FloatComparison.DefaultEps);
      Assert.AreEqual(1, n.SplitDimension);

      KdNode<Vector> left = n.Left;
      KdNode<Vector> right = n.Right;

      Assert.AreEqual(2, left.Vectors.Count);
      Assert.AreEqual(1.0, left.Vectors[0][0], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, left.Vectors[0][1], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, left.Vectors[1][0], FloatComparison.DefaultEps);
      Assert.AreEqual(-1.0, left.Vectors[1][1], FloatComparison.DefaultEps);

      Assert.AreEqual(2, right.Vectors.Count);
      Assert.AreEqual(1.0, right.Vectors[0][0], FloatComparison.DefaultEps);
      Assert.AreEqual(3.0, right.Vectors[0][1], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, right.Vectors[1][0], FloatComparison.DefaultEps);
      Assert.AreEqual(2.0, right.Vectors[1][1], FloatComparison.DefaultEps);

      
      Assert.AreEqual(1.0, left.SplitBounds.Lower[0], FloatComparison.DefaultEps);
      Assert.AreEqual(-1.0, left.SplitBounds.Lower[1], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, left.SplitBounds.Upper[0], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, left.SplitBounds.Upper[1], FloatComparison.DefaultEps);

      Assert.AreEqual(1.0, right.SplitBounds.Lower[0], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, right.SplitBounds.Lower[1], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, right.SplitBounds.Upper[0], FloatComparison.DefaultEps);
      Assert.AreEqual(3.0, right.SplitBounds.Upper[1], FloatComparison.DefaultEps);
    }
  }
}
