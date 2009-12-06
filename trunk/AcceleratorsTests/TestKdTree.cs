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
    public void TestOptimize() {
      List<IVector> vecs = new List<IVector>(VectorSampling.InAABB(500, 2, -100.0, 100.0, 10));
      KdTree<IVector> tree = new KdTree<IVector>(2, new SubdivisionPolicyConnector(1));
      foreach(IVector iv in vecs) {
        tree.Add(iv);
      }
      
      SubdivisionPolicyConnector spc = new SubdivisionPolicyConnector(3, new AxisOfMaximumSpreadSelector(), new MedianSelector(), new NoOperationResolver());
      tree.Optimize(spc);
      
      Assert.AreEqual(500, tree.Count);
      KdNodeInvariants.AreMetBy(tree.Root);
    }
  }
}
