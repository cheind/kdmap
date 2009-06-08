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
  public class TestBall
  {
    
    [Test()]
    public void TestInside()
    {
      Ball b = new Ball(new Vector(0.0, 0.0, 0.0), 1.0);
      
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
      Ball a = new Ball(new Vector(0.0, 0.0), 1.0);
      AABB b = new AABB(new Vector(0.5, 0.5), new Vector(0.6, 0.6)); // completely inside of a
      AABB c = new AABB(new Vector(-2.5, -2.5), new Vector(-2.4, -2.4)); // completely outside of a
      AABB d = new AABB(new Vector(2.5, 2.5), new Vector(2.6, 2.6)); // completely outside of a
      AABB e = new AABB(new Vector(0.5, 0.5), new Vector(2.6, 2.6)); // partially inside of a
      AABB f = new AABB(new Vector(1.0, 0.0), new Vector(2.6, 2.6)); // partially inside of a (touching)
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
      Ball a = new Ball(new Vector(0.0, 1.0), 1.0);
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
