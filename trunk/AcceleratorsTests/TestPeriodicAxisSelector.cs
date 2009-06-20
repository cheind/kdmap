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
  public class TestPeriodicAxisSelector
  {
    
    [Test()]
    [ExpectedException(typeof(DegenerateDatasetException))]
    public void TestAllCoordinatesSame()
    {
      KdNode<IVector> n = new KdNode<IVector>();
      n.Vectors = new List<IVector>(new IVector[]{Vector.Create(1.0f, 0.0f), Vector.Create(1.0f, 0.0f), Vector.Create(1.0f, 0.0f), Vector.Create(1.0f, 0.0f)});
      n.InternalBounds = new AABB(2);
      n.InternalBounds.Enlarge<IVector>(n.Vectors);
      n.InternalBounds = new AABB(n.InternalBounds);
      PeriodicAxisSelector s = new PeriodicAxisSelector();
      s.Select(n);
    }
    
    [Test]
    public void TestRoot() {
      KdNode<IVector> n = new KdNode<IVector>();
      n.Vectors = new List<IVector>(new IVector[]{Vector.Create(1.0f, 0.5), Vector.Create(1.0f, 0.0), Vector.Create(1.0f, 0.0), Vector.Create(1.0f, 0.0)});
      n.InternalBounds = new AABB(2);
      n.InternalBounds.Enlarge<IVector>(n.Vectors);
      n.InternalBounds = new AABB(n.InternalBounds);
      PeriodicAxisSelector s = new PeriodicAxisSelector();
      Assert.AreEqual(1, s.Select(n));
      
      n.Vectors[0][0] = 0.5;
      n.InternalBounds.Reset();
      n.InternalBounds.Enlarge<IVector>(n.Vectors);
      n.InternalBounds = new AABB(n.InternalBounds);
      Assert.AreEqual(0, s.Select(n));
    }
    
    [Test]
    public void TestNonRoot() {
      KdNode<IVector> root = new KdNode<IVector>();
      KdNode<IVector> first = root.SetLeftChild(new KdNode<IVector>());
      KdNode<IVector> second = first.SetLeftChild(new KdNode<IVector>());
      KdNode<IVector> third = second.SetLeftChild(new KdNode<IVector>());
      
      third.Vectors = new List<IVector>(new IVector[]{Vector.Create(0.0, 0.5), Vector.Create(1.0f, 0.0), Vector.Create(1.0f, 0.0), Vector.Create(1.0f, 0.0)});
      third.InternalBounds = new AABB(2);
      third.InternalBounds.Enlarge<IVector>(third.Vectors);
      third.InternalBounds = new AABB(third.InternalBounds);
      PeriodicAxisSelector s = new PeriodicAxisSelector();
      Assert.AreEqual(1, s.Select(third));
      
      second.Vectors = third.Vectors;
      second.InternalBounds = third.InternalBounds;
      second.InternalBounds = new AABB(third.InternalBounds);
      Assert.AreEqual(0, s.Select(second));
    }
    
  }
}
