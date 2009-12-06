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
  public class TestAABB
  {
    [Test]
    public void TestConstructor() {
      AABB a = new AABB(2);
      Assert.AreEqual(a.Lower.Dimensions, 2);
      Assert.AreEqual(a.Upper.Dimensions, 2);
      Assert.AreEqual(Double.MaxValue, a.Lower[0]);
      Assert.AreEqual(Double.MaxValue, a.Lower[1]);
      Assert.AreEqual(Double.MinValue, a.Upper[0]);
      Assert.AreEqual(Double.MinValue, a.Upper[1]);
      Assert.IsTrue(a.Empty);
    }
    
    [Test]
    public void TestEmpty() {
      AABB a = new AABB(20);
      Assert.IsTrue(a.Empty);
      a.Lower[10] = 1.0;
      Assert.IsFalse(a.Empty);
    }
    
    [Test]
    public void TestReset() {
      AABB a = new AABB(20);
      a.Lower[10] = 1.0;
      a.Upper[10] = 1.0;
      Assert.IsFalse(a.Empty);
      a.Reset();
      Assert.IsTrue(a.Empty);
    }
    
    [Test]
    public void TestEnlargeSingle() {
      AABB box = new AABB(2);
      Vector a = Vector.Create(1.0, 2.0);
      box.Enlarge(a);
      Assert.IsFalse(box.Empty);
      Assert.IsTrue(VectorComparison.Close(a, box.Lower, FloatComparison.DefaultEps));
      Assert.IsTrue(VectorComparison.Close(a, box.Upper, FloatComparison.DefaultEps));
      
      Vector b = Vector.Create(-1.0, 4.0);
      box.Enlarge(b);
      Assert.AreEqual(box.Lower[0], -1.0, FloatComparison.DefaultEps);
      Assert.AreEqual(box.Lower[1], 2.0, FloatComparison.DefaultEps);
      Assert.AreEqual(box.Upper[0], 1.0, FloatComparison.DefaultEps);
      Assert.AreEqual(box.Upper[1], 4.0, FloatComparison.DefaultEps);
    }
    
    [Test]
    public void TestEnlargeCollection() {
      AABB box = new AABB(2);
      box.Enlarge(VectorSampling.InAABB(1000, 2, -1.0, 1.0, 10));
      Assert.LessOrEqual(-1.0 , box.Lower[0]);
      Assert.LessOrEqual(-1.0 , box.Lower[1]);
      Assert.GreaterOrEqual(1.0 , box.Upper[0]);
      Assert.GreaterOrEqual(1.0 , box.Upper[1]);
    }
    
    [Test]
    public void TestDiagonal() {
      AABB box = new AABB(2);
      box.Enlarge(Vector.Create(1.0, -2.0));
      box.Enlarge(Vector.Create(-1.0, 5.0));
      Vector diag = (Vector)box.Diagonal;
      Assert.AreEqual(diag[0], 2.0, FloatComparison.DefaultEps);
      Assert.AreEqual(diag[1], 7.0, FloatComparison.DefaultEps);
    }
    
    [Test]
    public void TestExtension() {
      AABB box = new AABB(2);
      box.Enlarge(Vector.Create(1.0, -2.0));
      box.Enlarge(Vector.Create(-1.0, 5.0));
      Assert.AreEqual(box.Extension(0), 2.0, FloatComparison.DefaultEps);
      Assert.AreEqual(box.Extension(1), 7.0, FloatComparison.DefaultEps);
    }
    
    [Test]
    public void TestSplit() {
      AABB box = new AABB(Vector.Create(-1.0, -1.0), Vector.Create(1.0, 1.0));
      AABB left, right;
      box.Split(1, 0.5, out left, out right);
      
      Assert.AreEqual(-1.0, left.Lower[0], FloatComparison.DefaultEps);
      Assert.AreEqual(-1.0, left.Lower[1], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, left.Upper[0], FloatComparison.DefaultEps);
      Assert.AreEqual(0.5, left.Upper[1], FloatComparison.DefaultEps);
      
      Assert.AreEqual(-1.0, right.Lower[0], FloatComparison.DefaultEps);
      Assert.AreEqual(0.5, right.Lower[1], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, right.Upper[0], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, right.Upper[1], FloatComparison.DefaultEps);
    }
    
    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void TestSplitOutsidePlane() {
      AABB box = new AABB(Vector.Create(-1.0, -1.0), Vector.Create(1.0, 1.0));
      AABB left, right;
      box.Split(1, -1.1, out left, out right);
    }
    
    [Test]
    public void TestClosestOnSurface() {
      AABB box = new AABB(Vector.Create(-1.0, -1.0), Vector.Create(1.0, 1.0));
      IVector closest = box.Closest(Vector.Create(-2.0, -0.5));
      Assert.AreEqual(-1.0, closest[0], FloatComparison.DefaultEps);
      Assert.AreEqual(-0.5, closest[1], FloatComparison.DefaultEps);
      
      closest = box.Closest(Vector.Create(3.0, 3.0));
      Assert.AreEqual(1.0, closest[0], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, closest[1], FloatComparison.DefaultEps);
      
      closest = box.Closest(Vector.Create(0.5, 0.5));
      Assert.AreEqual(0.5, closest[0], FloatComparison.DefaultEps);
      Assert.AreEqual(0.5, closest[1], FloatComparison.DefaultEps);
    }
    
    [Test]
    public void TestInside() {
      AABB box = new AABB(Vector.Create(-1.0, -1.0), Vector.Create(1.0, 1.0));
      
      // Perform a monte-carlo integration test
      const int count = 100000;
      List<IVector> vecs  = new List<IVector>(VectorSampling.InAABB(count, 2, -2.0, 2.0, 10));
      
      int inside = 0;
      foreach (IVector v in vecs) {
        if (box.Inside(v))
          inside += 1;
      }
      
      double ratio = (double)inside / count;
      double volume_outer_box = 4.0 * 4.0;
      double volume_inner_box = volume_outer_box * ratio;
      
      Assert.AreEqual(4.0, volume_inner_box, 0.1);
    }
    
    [Test]
    public void TestIntersect() {
      AABB a = new AABB(Vector.Create(-1.0, -1.0), Vector.Create(1.0, 1.0));
      AABB b = new AABB(Vector.Create(0.5, 0.5), Vector.Create(0.6, 0.6)); // completely inside of a
      AABB c = new AABB(Vector.Create(-2.5, -2.5), Vector.Create(-2.4, -2.4)); // completely outside of a
      AABB d = new AABB(Vector.Create(2.5, 2.5), Vector.Create(2.6, 2.6)); // completely outside of a
      AABB e = new AABB(Vector.Create(0.5, 0.5), Vector.Create(2.6, 2.6)); // partially inside of a
      AABB f = new AABB(Vector.Create(1.0, 1.0), Vector.Create(2.6, 2.6)); // partially inside of a (touching)
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
      AABB a = new AABB(Vector.Create(-1.0, -1.0), Vector.Create(1.0, 1.0));
      Assert.AreEqual(EPlanePosition.LeftOfBV, a.ClassifyPlane(1, -2.0));
      Assert.AreEqual(EPlanePosition.RightOfBV, a.ClassifyPlane(1, 2.0));
      Assert.AreEqual(EPlanePosition.IntersectingBV, a.ClassifyPlane(1, 0.5));
    }

    [Test]
    public void TestLimitLower() {
      AABB a = new AABB(Vector.Create(-1.0, -1.0), Vector.Create(1.0, 1.0));
      a.LimitLower(Vector.Create(-0.5, -1.5));
      Assert.AreEqual(a.Lower[0], -0.5, FloatComparison.DefaultEps);
      Assert.AreEqual(a.Lower[1], -1.0, FloatComparison.DefaultEps);

      a = new AABB(2);
      a.Lower[0] = 1.0;
      a.LimitLower(Vector.Create(-0.5, -1.5));
      Assert.AreEqual(a.Lower[0], 1.0, FloatComparison.DefaultEps);
      Assert.AreEqual(a.Lower[1], -1.5, FloatComparison.DefaultEps);
    }

    [Test]
    public void TestLimitUpper() {
      AABB a = new AABB(Vector.Create(-5.0, -5.0), Vector.Create(1.0, 1.0));
      a.LimitUpper(Vector.Create(-0.5, -1.5));
      Assert.AreEqual(a.Upper[0], -0.5, FloatComparison.DefaultEps);
      Assert.AreEqual(a.Upper[1], -1.5, FloatComparison.DefaultEps);

      a = new AABB(2);
      a.Upper[0] = 1.0;
      a.LimitUpper(Vector.Create(-0.5, -1.5));
      Assert.AreEqual(a.Upper[0], -0.5, FloatComparison.DefaultEps);
      Assert.AreEqual(a.Upper[1], -1.5, FloatComparison.DefaultEps);
    }

    [Test]
    public void TestUnion() {
      AABB a = new AABB(Vector.Create(-5.0, -5.0), Vector.Create(1.0, 1.0));
      AABB b = new AABB(Vector.Create(-3.0, -6.0), Vector.Create(0.0, 4.0));
      AABB merged = new AABB(2);
      AABB.Union(a, b, ref merged);

      Assert.AreEqual(merged.Lower[0], -5.0, FloatComparison.DefaultEps);
      Assert.AreEqual(merged.Lower[1], -6.0, FloatComparison.DefaultEps);
      Assert.AreEqual(merged.Upper[0], 1.0, FloatComparison.DefaultEps);
      Assert.AreEqual(merged.Upper[1], 4.0, FloatComparison.DefaultEps);
    }

    [Test]
    public void TestUnionEmpty() {
      AABB a = new AABB(Vector.Create(-5.0, -5.0), Vector.Create(1.0, 1.0));
      AABB b = new AABB(2);
      AABB merged = new AABB(2);
      AABB.Union(a, b, ref merged);

      Assert.AreEqual(merged.Lower[0], -5.0, FloatComparison.DefaultEps);
      Assert.AreEqual(merged.Lower[1], -5.0, FloatComparison.DefaultEps);
      Assert.AreEqual(merged.Upper[0], 1.0, FloatComparison.DefaultEps);
      Assert.AreEqual(merged.Upper[1], 1.0, FloatComparison.DefaultEps);

      AABB.Union(b, a, ref merged);

      Assert.AreEqual(merged.Lower[0], -5.0, FloatComparison.DefaultEps);
      Assert.AreEqual(merged.Lower[1], -5.0, FloatComparison.DefaultEps);
      Assert.AreEqual(merged.Upper[0], 1.0, FloatComparison.DefaultEps);
      Assert.AreEqual(merged.Upper[1], 1.0, FloatComparison.DefaultEps);
    }
    
    
  }
}
