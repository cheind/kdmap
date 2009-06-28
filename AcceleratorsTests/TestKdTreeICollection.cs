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
  public class TestKdTreeICollection
  {
    
    [Test]
    public void TestAdd() {
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
    public void TestRemove() {
      List<IVector> vecs = new List<IVector>(VectorSampling.InAABB(5000, 2, -100.0, 100.0, 10));
      KdTree<IVector> tree = new KdTree<IVector>(vecs, new SubdivisionPolicyConnector(1));

      for (int i = 0; i < 500; ++i) {
        Assert.IsTrue(tree.Remove(vecs[i]));
      }
      KdNodeInvariants.AreMetBy(tree.Root);
      Assert.AreEqual(5000 -500, tree.Count);
      int count = 0;
      foreach (KdNode<IVector> n in tree.Root.Leaves) {
        count += n.Vectors.Count;
      }
      Assert.AreEqual(5000 - 500, count);
      
      // Removing of non-existant elements
      Assert.IsFalse(tree.Remove(Vector.Create(200.0, 200.0)));
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
    
    [Test]
    public void TestCount() {
      List<IVector> vecs = new List<IVector>(VectorSampling.InAABB(10, 2, -100.0, 100.0, 10));
      KdTree<IVector> tree = new KdTree<IVector>(2, new Accelerators.Subdivision.SubdivisionPolicyConnector(1));
     
      
      Assert.AreEqual(0, tree.Count);
      tree.Add(vecs[0]);
      Assert.AreEqual(1, tree.Count);
      tree.Add(vecs[1]);
      Assert.AreEqual(2, tree.Count);
      tree.Add(vecs[2]);
      Assert.AreEqual(3, tree.Count);
      tree.Remove(vecs[2]);
      Assert.AreEqual(2, tree.Count);
      tree.Remove(vecs[5]);
      Assert.AreEqual(2, tree.Count);
      tree.Remove(vecs[1]);
      Assert.AreEqual(1, tree.Count);
      tree.Remove(vecs[0]);
      Assert.AreEqual(0, tree.Count);
      tree.Add(vecs[0]);
      tree.Clear();
      Assert.AreEqual(0, tree.Count);
    }
    
    [Test]
    public void TestContains() {
      KdTree<IVector> tree = new KdTree<IVector>(2, new Accelerators.Subdivision.SubdivisionPolicyConnector(1));
      
      Assert.IsFalse(tree.Contains(Vector.Create(2.0, 1.0)));
      tree.Add(Vector.Create(3.0, 2.0));
      Assert.IsTrue(tree.Contains(Vector.Create(3.0, 2.0)));
      tree.Add(Vector.Create(3.0, 2.0)); // duplicate elements
      Assert.IsTrue(tree.Contains(Vector.Create(3.0, 2.0)));
      
      tree.Add(Vector.Create(2.0, 1.0));
      Assert.IsTrue(tree.Contains(Vector.Create(2.0, 1.0)));
      Assert.IsTrue(tree.Remove(Vector.Create(2.0, 1.0)));
      Assert.IsFalse(tree.Contains(Vector.Create(2.0, 1.0)));
    }
    
    [Test]
    public void TestCopyTo() {
      IVector[] dest = new IVector[10];
      Vector[] vecs = new Vector[] { Vector.Create(-1.0, -1.0), Vector.Create(0.0, 0.0), Vector.Create(1.0, 1.0), Vector.Create(2.0, 2.0) };
      KdTree<IVector> tree = new KdTree<IVector>(2, new Accelerators.Subdivision.SubdivisionPolicyConnector(1));
      
      tree.CopyTo(dest, 0);
      Assert.IsNull(dest[0]); // nothing copied
      
      tree.Add(vecs[0]);
      tree.Add(vecs[1]);
      tree.Add(vecs[2]);
      tree.CopyTo(dest, 0);
      for (int i =  0; i < 3; ++i) { Assert.IsTrue(VectorComparison.Equal(vecs[i], dest[i]));}
      
      tree.CopyTo(dest, 3);
      for (int i =  0; i < 3; ++i) { Assert.IsTrue(VectorComparison.Equal(vecs[i], dest[i+3]));}
      
      tree = new KdTree<IVector>(2, new Accelerators.Subdivision.SubdivisionPolicyConnector(2));
      tree.Add(vecs[0]);
      tree.Add(vecs[1]);
      tree.Add(vecs[2]);
      tree.CopyTo(dest, 6);
      for (int i =  0; i < 3; ++i) { Assert.IsTrue(VectorComparison.Equal(vecs[i], dest[i+6]));}
    }
  }
}
