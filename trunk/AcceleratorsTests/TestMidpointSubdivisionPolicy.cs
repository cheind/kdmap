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

namespace AcceleratorsTests
{
	[TestFixture()]
  public class TestMidpointSubdivisionPolicy
	{
    [Test]
    [ExpectedException(typeof(DegenerateDatasetException))]
    public void TestDegeneratedAllSame() {
      MidpointSubdivisionPolicy msp = new MidpointSubdivisionPolicy(1);

      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[] { new Vector(1.0, 1.0), new Vector(1.0, 1.0), new Vector(1.0, 1.0), new Vector(1.0, 1.0) });
      n.Bounds = new AABB(2);
      n.Bounds.Enlarge<Vector>(n.Vectors);
      msp.Split(n);
    }

    [Test]
    [ExpectedException(typeof(BucketSizeException))]
    public void TestBucketSize() {
      MidpointSubdivisionPolicy msp = new MidpointSubdivisionPolicy();
      msp.MaximumBucketSize = 10;

      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[] { new Vector(1.0, 1.0), new Vector(2.0, 3.0), new Vector(3.0, 1.0), new Vector(4.0, 1.0) });
      n.Bounds = new AABB(2);
      n.Bounds.Enlarge<Vector>(n.Vectors);
      msp.Split(n);
    }

    [Test]
    [ExpectedException(typeof(IntermediateNodeException))]
    public void TestIntermediateNode() {
      MidpointSubdivisionPolicy msp = new MidpointSubdivisionPolicy();
      msp.MaximumBucketSize = 1;

      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[] { new Vector(1.0, 1.0), new Vector(2.0, 3.0), new Vector(3.0, 1.0), new Vector(4.0, 1.0) });
      n.Bounds = new AABB(2);
      n.Bounds.Enlarge<Vector>(n.Vectors);
      msp.Split(n);
      msp.Split(n); // split again
    }

    [Test]
    public void TestSplitOneDimensional() {
      MidpointSubdivisionPolicy msp = new MidpointSubdivisionPolicy(1);

      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[] { new Vector(-1.0), new Vector(1.0), new Vector(3.0), new Vector(2.0) });
      n.Bounds = new AABB(1);
      n.Bounds.Enlarge<Vector>(n.Vectors);
      msp.Split(n);

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

      Assert.AreEqual(-1.0, left.Bounds.Lower[0], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, left.Bounds.Upper[0], FloatComparison.DefaultEps);

      Assert.AreEqual(1.0, right.Bounds.Lower[0], FloatComparison.DefaultEps);
      Assert.AreEqual(3.0, right.Bounds.Upper[0], FloatComparison.DefaultEps);
    }

    [Test]
    public void TestSplitMultiDimensional() {
      MidpointSubdivisionPolicy msp = new MidpointSubdivisionPolicy(1);

      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[] { new Vector(1.0, 1.0), new Vector(1.0, -1.0), new Vector(1.0, 3.0), new Vector(1.0, 2.0) });
      n.Bounds = new AABB(2);
      n.Bounds.Enlarge<Vector>(n.Vectors);
      msp.Split(n);

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

      
      Assert.AreEqual(1.0, left.Bounds.Lower[0], FloatComparison.DefaultEps);
      Assert.AreEqual(-1.0, left.Bounds.Lower[1], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, left.Bounds.Upper[0], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, left.Bounds.Upper[1], FloatComparison.DefaultEps);

      Assert.AreEqual(1.0, right.Bounds.Lower[0], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, right.Bounds.Lower[1], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, right.Bounds.Upper[0], FloatComparison.DefaultEps);
      Assert.AreEqual(3.0, right.Bounds.Upper[1], FloatComparison.DefaultEps);
    }
	}
}
