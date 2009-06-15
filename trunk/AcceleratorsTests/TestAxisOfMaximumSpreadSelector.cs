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
  public class TestAxisOfMaximumSpreadSelector
  {
    
    [Test()]
    [ExpectedException(typeof(DegenerateDatasetException))]
    public void TestAllCoordinatesSame()
    {
      KdNode<IVector> n = new KdNode<IVector>();
      n.Vectors = new List<IVector>(new IVector[]{new Vector(1.0f), new Vector(1.0f), new Vector(1.0f), new Vector(1.0f)});
      n.SplitBounds = new AABB(1);
      n.SplitBounds.Enlarge<IVector>(n.Vectors);
      n.InternalBounds = new AABB(n.SplitBounds);
      AxisOfMaximumSpreadSelector s = new AxisOfMaximumSpreadSelector();
      s.Select(n);
    }
    
    [Test]
    public void TestSecondAxisIsAxisOfMaximumSpread() {
      KdNode<IVector> n = new KdNode<IVector>();
      n.Vectors = new List<IVector>(new IVector[]{new Vector(0.5, 0.5), new Vector(1.0f, -1.0), new Vector(1.0f, 2.0), new Vector(1.0f, 1.5)});
      n.SplitBounds = new AABB(2);
      n.SplitBounds.Enlarge<IVector>(n.Vectors);
      n.InternalBounds = new AABB(n.SplitBounds);
      AxisOfMaximumSpreadSelector s = new AxisOfMaximumSpreadSelector();
      Assert.AreEqual(1, s.Select(n));
    }

    
    [Test]
    public void TestFirstAxisIsAxisOfMaximumSpread() {
      KdNode<IVector> n = new KdNode<IVector>();
      n.Vectors = new List<IVector>(new IVector[]{new Vector(-10.0f, 0.5), new Vector(1.0f, -1.0), new Vector(1.0f, 2.0), new Vector(1.0f, 1.5)});
      n.SplitBounds = new AABB(2);
      n.SplitBounds.Enlarge<IVector>(n.Vectors);
      n.InternalBounds = new AABB(n.SplitBounds);
      AxisOfMaximumSpreadSelector s = new AxisOfMaximumSpreadSelector();
      Assert.AreEqual(0, s.Select(n));
    }
  }
}
