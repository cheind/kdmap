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
  public class TestBall
  {
    
    [Test()]
    public void TestInside()
    {
      Ball b = new Ball(Vector.Create(0.0, 0.0, 0.0), 1.0);
      
      // Perform a monte-carlo integration test
      const int count = 10000;
      List<IVector> vecs  = new List<IVector>(VectorSampling.InAABB(count, 3, -1.0, 1.0, 10));
      
      int inside = 0;
      foreach (IVector v in vecs) {
        if (b.Inside(v))
          inside += 1;
      }
      
      double ratio = (double)inside / count;
      double volume_outer_box = 8.0;
      double volume_sphere = volume_outer_box * ratio;
      
      Assert.AreEqual((4.0/3.0)*Math.PI, volume_sphere, 0.1);
    }
    
    [Test]
    public void TestIntersect() {
      Ball a = new Ball(Vector.Create(0.0, 0.0), 1.0);
      AABB b = new AABB(Vector.Create(0.5, 0.5), Vector.Create(0.6, 0.6)); // completely inside of a
      AABB c = new AABB(Vector.Create(-2.5, -2.5), Vector.Create(-2.4, -2.4)); // completely outside of a
      AABB d = new AABB(Vector.Create(2.5, 2.5), Vector.Create(2.6, 2.6)); // completely outside of a
      AABB e = new AABB(Vector.Create(0.5, 0.5), Vector.Create(2.6, 2.6)); // partially inside of a
      AABB f = new AABB(Vector.Create(1.0, 0.0), Vector.Create(2.6, 2.6)); // partially inside of a (touching)
      AABB g = new AABB(Vector.Create(-2.0, -2.0), Vector.Create(2.6, 2.6)); // completely containing a
      
      Assert.IsTrue(a.Intersect(b));
      Assert.IsFalse(a.Intersect(c));
      Assert.IsFalse(a.Intersect(d));
      Assert.IsTrue(a.Intersect(e));
      Assert.IsTrue(a.Intersect(f));
      Assert.IsTrue(a.Intersect(g));
    }
    
    [Test]
    public void TestClassifyPlane() {
      Ball a = new Ball(Vector.Create(0.0, 1.0), 1.0);
      Assert.AreEqual(EPlanePosition.IntersectingBV, a.ClassifyPlane(0, -1.0));
      Assert.AreEqual(EPlanePosition.LeftOfBV, a.ClassifyPlane(0, -2.0));
      Assert.AreEqual(EPlanePosition.RightOfBV, a.ClassifyPlane(0, 2.0));
      Assert.AreEqual(EPlanePosition.IntersectingBV, a.ClassifyPlane(1, 0.0));
      Assert.AreEqual(EPlanePosition.IntersectingBV, a.ClassifyPlane(1, 2.0));
      Assert.AreEqual(EPlanePosition.LeftOfBV, a.ClassifyPlane(1, -1.0));
      Assert.AreEqual(EPlanePosition.RightOfBV, a.ClassifyPlane(1, 3.0));
    }
  }
}
