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
    
    [Test]
    public void TestEnumerationOfEmptyTree() {
      KdTree<IVector> tree = new KdTree<IVector>(2, new SubdivisionPolicyConnector(1));
      using (IEnumerator<IVector> e = tree.GetEnumerator()) {
        Assert.IsFalse(e.MoveNext());
      }
    }

    [Test]
    public void TestEnumeration() {
      Vector[] vecs = new Vector[] { Vector.Create(-1.0, -1.0), Vector.Create(0.0, 0.0), Vector.Create(1.0, 1.0), Vector.Create(2.0, 2.0) };
      KdTree<Vector> tree = new KdTree<Vector>(vecs, new Accelerators.Subdivision.SubdivisionPolicyConnector(1));
      int index = 0;
      using (IEnumerator<Vector> e = tree.GetEnumerator()) {
        while (e.MoveNext()) {
          // iterator traverses nodes from left to right
          Assert.IsTrue(vecs[index].Equals(e.Current));
          index += 1;
        }
        Assert.AreEqual(4, index);
      }
    }
    
  }
}
