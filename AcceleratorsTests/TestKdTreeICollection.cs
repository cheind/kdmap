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
