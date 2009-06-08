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
      Assert.AreEqual(-Double.MaxValue, a.Upper[0]);
      Assert.AreEqual(-Double.MaxValue, a.Upper[1]);
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
      Vector a = new Vector(1.0, 2.0);
      box.Enlarge(a);
      Assert.IsFalse(box.Empty);
      Assert.IsTrue(VectorComparison.Close(a, box.Lower, FloatComparison.DefaultEps));
      Assert.IsTrue(VectorComparison.Close(a, box.Upper, FloatComparison.DefaultEps));
      
      Vector b = new Vector(-1.0, 4.0);
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
      box.Enlarge(new Vector(1.0, -2.0));
      box.Enlarge(new Vector(-1.0, 5.0));
      Vector diag = (Vector)box.Diagonal;
      Assert.AreEqual(diag[0], 2.0, FloatComparison.DefaultEps);
      Assert.AreEqual(diag[1], 7.0, FloatComparison.DefaultEps);
    }
    
    [Test]
    public void TestExtension() {
      AABB box = new AABB(2);
      box.Enlarge(new Vector(1.0, -2.0));
      box.Enlarge(new Vector(-1.0, 5.0));
      Assert.AreEqual(box.Extension(0), 2.0, FloatComparison.DefaultEps);
      Assert.AreEqual(box.Extension(1), 7.0, FloatComparison.DefaultEps);
    }
    
    [Test]
    public void TestSplit() {
      AABB box = new AABB(new Vector(-1.0, -1.0), new Vector(1.0, 1.0));
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
      AABB box = new AABB(new Vector(-1.0, -1.0), new Vector(1.0, 1.0));
      AABB left, right;
      box.Split(1, -1.1, out left, out right);
    }
    
    [Test]
    public void TestClosestOnSurface() {
      AABB box = new AABB(new Vector(-1.0, -1.0), new Vector(1.0, 1.0));
      IVector closest = box.Closest(new Vector(-2.0, -0.5));
      Assert.AreEqual(-1.0, closest[0], FloatComparison.DefaultEps);
      Assert.AreEqual(-0.5, closest[1], FloatComparison.DefaultEps);
      
      closest = box.Closest(new Vector(3.0, 3.0));
      Assert.AreEqual(1.0, closest[0], FloatComparison.DefaultEps);
      Assert.AreEqual(1.0, closest[1], FloatComparison.DefaultEps);
      
      closest = box.Closest(new Vector(0.5, 0.5));
      Assert.AreEqual(0.5, closest[0], FloatComparison.DefaultEps);
      Assert.AreEqual(0.5, closest[1], FloatComparison.DefaultEps);
    }
    
    [Test]
    public void TestInside() {
      AABB box = new AABB(new Vector(-1.0, -1.0), new Vector(1.0, 1.0));
      
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
      AABB a = new AABB(new Vector(-1.0, -1.0), new Vector(1.0, 1.0));
      AABB b = new AABB(new Vector(0.5, 0.5), new Vector(0.6, 0.6)); // completely inside of a
      AABB c = new AABB(new Vector(-2.5, -2.5), new Vector(-2.4, -2.4)); // completely outside of a
      AABB d = new AABB(new Vector(2.5, 2.5), new Vector(2.6, 2.6)); // completely outside of a
      AABB e = new AABB(new Vector(0.5, 0.5), new Vector(2.6, 2.6)); // partially inside of a
      AABB f = new AABB(new Vector(1.0, 1.0), new Vector(2.6, 2.6)); // partially inside of a (touching)
      AABB g = new AABB(new Vector(-2.0, -2.0), new Vector(2.6, 2.6)); // completely containing a
      
      Assert.IsTrue(a.Intersect(b));
      Assert.IsFalse(a.Intersect(c));
      Assert.IsFalse(a.Intersect(d));
      Assert.IsTrue(a.Intersect(e));
      Assert.IsTrue(a.Intersect(f));
      Assert.IsTrue(a.Intersect(g));
    }
    
    [Test]
    public void TestClassifyPlane() {
      AABB a = new AABB(new Vector(-1.0, -1.0), new Vector(1.0, 1.0));
      Assert.AreEqual(EPlanePosition.LeftOfBV, a.ClassifyPlane(1, -2.0));
      Assert.AreEqual(EPlanePosition.RightOfBV, a.ClassifyPlane(1, 2.0));
      Assert.AreEqual(EPlanePosition.IntersectingBV, a.ClassifyPlane(1, 0.5));
    }
    
    
  }
}
