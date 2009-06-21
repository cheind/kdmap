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
using Accelerators.Subdivision;
using System.Collections.Generic;

namespace AcceleratorsTests
{
  
  
  [TestFixture()]
  public class TestKdTree
  {
    /// <summary>
    /// Demo class that implements IVector and is to be stored in KdTree
    /// </summary>
    class Flag : IVector {
      public Flag(double x, string name) {
        _coords = new double[1] {x};
        _name = name;
      }
      
      public Flag(double x, double y, string name) {
        _coords = new double[2] {x,y};
        _name = name;
      }
      
      public double this[int index] {
        get {
          return _coords[index];
        }
        set {
          _coords[index] = value;
        }
      }
      
      public int Dimensions {
        get {
          return _coords.Length;
        }
      }
      
      public string Name {
        get {
          return _name;
        }
      }
      
      
      private double[] _coords;
      private string _name;
    }
    
    [Test()]
    public void TestSearchExact()
    {
      /*
      Flag[] flags = new Flag[] {new Flag(-1.0, "a"), new Flag(1.0, "b"), new Flag(1.4, "c"), new Flag(3.0, "d")};
      KdTree<Flag> tree = new KdTree<Flag>(flags, new SubdivisionPolicyConnector(1));

      Accelerators.Searches.ExactSearch<Flag> es = new Accelerators.Searches.ExactSearch<Flag>(tree.Root);
      es.FindExact(Vector.Create(1.0));


      Flag x = tree.Find(Vector.Create(1.0));
      Assert.IsNotNull(x);
      Assert.AreEqual("b", x.Name);
      
      x = tree.Find(Vector.Create(1.4));
      Assert.IsNotNull(x);
      Assert.AreEqual("c", x.Name);
      
      x = tree.Find(Vector.Create(1.3));
      Assert.IsNull(x);
       * */
    }

    private void FindInsideVolumeNumerically(ISubdivisionPolicy policy) {
      Vector[] vecs = new Vector[] { Vector.Create(-1.0, -1.0), Vector.Create(1.0, 1.0), Vector.Create(2.0, 2.0), Vector.Create(3.0, 3.0) };
      KdTree<Vector> tree = new KdTree<Vector>(vecs, policy);

      List<Vector> found = new List<Vector>(
        tree.FindInsideVolume(
          new AABB(Vector.Create(0.5, 0.5), Vector.Create(2.5, 2.5))
        )
      );

      Assert.AreEqual(found.Count, 2);
      Assert.IsTrue(VectorComparison.Equal(vecs[1], found[0]));
      Assert.IsTrue(VectorComparison.Equal(vecs[2], found[1]));

      found = new List<Vector>(
        tree.FindInsideVolume(
          new AABB(Vector.Create(0.0, 0.0), Vector.Create(0.5, 0.5))
        )
      );

      Assert.IsEmpty(found);

      found = new List<Vector>(
        tree.FindInsideVolume(
          new AABB(Vector.Create(-2.0, -2.0), Vector.Create(4.5, 4.5))
        )
      );

      Assert.AreEqual(4, found.Count);
      Assert.IsTrue(VectorComparison.Equal(vecs[0], found[0]));
      Assert.IsTrue(VectorComparison.Equal(vecs[1], found[1]));
      Assert.IsTrue(VectorComparison.Equal(vecs[2], found[2]));
      Assert.IsTrue(VectorComparison.Equal(vecs[3], found[3]));
    }
    
    [Test]
    public void FindInsideVolumeNumerically()
    {
      this.FindInsideVolumeNumerically(new SubdivisionPolicyConnector(1));
      this.FindInsideVolumeNumerically(SubdivisionPolicyConnector.CreatePolicy<PeriodicAxisSelector, MedianSelector, SlidingPlaneResolver>(1));
    }

    private void FindInSortedOrder(ISubdivisionPolicy policy) {
      Vector[] vecs = new Vector[] { Vector.Create(-1.0, -1.0), Vector.Create(0.0, 0.0), Vector.Create(1.0, 1.0), Vector.Create(2.0, 2.0) };
      KdTree<Vector> tree = new KdTree<Vector>(vecs, policy);
      List<Vector> order_min = new List<Vector>(tree.FindInSortedOrder(Vector.Create(-1.0, -1.0), 10.0));

      Assert.AreEqual(order_min.Count, 4);
      Assert.IsTrue(VectorComparison.Equal(order_min[0], vecs[0]));
      Assert.IsTrue(VectorComparison.Equal(order_min[1], vecs[1]));
      Assert.IsTrue(VectorComparison.Equal(order_min[2], vecs[2]));
      Assert.IsTrue(VectorComparison.Equal(order_min[3], vecs[3]));

      order_min = new List<Vector>(tree.FindInSortedOrder(Vector.Create(-1.0, -1.0), 1.5));
      Assert.AreEqual(order_min.Count, 2);
      Assert.IsTrue(VectorComparison.Equal(order_min[0], vecs[0]));
      Assert.IsTrue(VectorComparison.Equal(order_min[1], vecs[1]));
    }

    [Test]
    public void FindInSortedOrder() {
      this.FindInSortedOrder(new SubdivisionPolicyConnector(1));
      this.FindInSortedOrder(SubdivisionPolicyConnector.CreatePolicy<PeriodicAxisSelector, MedianSelector, SlidingPlaneResolver>(1));
    }

    [Test]
    public void TestStaticConstruction() {
      KdTree<IVector> tree = new KdTree<IVector>(VectorSampling.InAABB(5000, 2, -100.0, 100.0, 10), new SubdivisionPolicyConnector(1));
      KdNodeInvariants.AreMetBy(tree.Root);
      int count = 0;
      foreach (KdNode<IVector> n in tree.Root.Leaves) {
        count += n.Vectors.Count;
      }
      Assert.AreEqual(5000, count);
    }

    [Test]
    public void TestDynamicConstruction() {
      KdTree<IVector> tree = new KdTree<IVector>(2, new SubdivisionPolicyConnector(1));
      foreach (IVector iv in VectorSampling.InAABB(5000, 2, -100.0, 100.0, 10)) {
        tree.Add(iv);
      }
      KdNodeInvariants.AreMetBy(tree.Root);
      int count = 0;
      foreach (KdNode<IVector> n in tree.Root.Leaves) {
        count += n.Vectors.Count;
      }
      Assert.AreEqual(5000, count);
    }

    [Test]
    public void TestDynamicRemoval() {
      List<IVector> vecs = new List<IVector>(VectorSampling.InAABB(5000, 2, -100.0, 100.0, 10));
      KdTree<IVector> tree = new KdTree<IVector>(vecs, new SubdivisionPolicyConnector(1));

      for (int i = 0; i < 500; ++i) {
        tree.Remove(vecs[i]);
      }
      KdNodeInvariants.AreMetBy(tree.Root);
      Assert.AreEqual(5000 -500, tree.Count);
      int count = 0;
      foreach (KdNode<IVector> n in tree.Root.Leaves) {
        count += n.Vectors.Count;
      }
      Assert.AreEqual(5000 - 500, count);
    }
  }
}
