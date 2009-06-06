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
      Ball b = new Ball(new Vector(0.0f, 0.0f, 0.0f), 1.0f);
      
      // Perform a monte-carlo integration test
      const int count = 1000;
      List<IVector> vecs  = new List<IVector>(VectorSampling.InAABB(count, 3, -1.0f, 1.0f, 10));
      
      int inside = 0;
      foreach (IVector v in vecs) {
        if (b.Inside(v))
          inside += 1;
      }
      
      float ratio = (float)inside / count;
      float volume_outer_box = 8.0f;
      float volume_sphere = volume_outer_box * ratio;
      
      Assert.AreEqual((4.0f/3.0f)*Math.PI, volume_sphere, 0.1f);
    }
    
    [Test]
    public void TestIntersect() {
      Ball a = new Ball(new Vector(0.0f, 0.0f), 1.0f);
      AABB b = new AABB(new Vector(0.5f, 0.5f), new Vector(0.6f, 0.6f)); // completely inside of a
      AABB c = new AABB(new Vector(-2.5f, -2.5f), new Vector(-2.4f, -2.4f)); // completely outside of a
      AABB d = new AABB(new Vector(2.5f, 2.5f), new Vector(2.6f, 2.6f)); // completely outside of a
      AABB e = new AABB(new Vector(0.5f, 0.5f), new Vector(2.6f, 2.6f)); // partially inside of a
      AABB f = new AABB(new Vector(1.0f, 0.0f), new Vector(2.6f, 2.6f)); // partially inside of a (touching)
      AABB g = new AABB(new Vector(-2.0f, -2.0f), new Vector(2.6f, 2.6f)); // completely containing a
      
      Assert.IsTrue(a.Intersect(b));
      Assert.IsFalse(a.Intersect(c));
      Assert.IsFalse(a.Intersect(d));
      Assert.IsTrue(a.Intersect(e));
      Assert.IsTrue(a.Intersect(f));
      Assert.IsTrue(a.Intersect(g));
    }
  }
}
