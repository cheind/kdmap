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
      n.Vectors = new List<IVector>(new IVector[]{new Vector(1.0f, 0.0f), new Vector(1.0f, 0.0f), new Vector(1.0f, 0.0f), new Vector(1.0f, 0.0f)});
      n.Bounds = new AABB(2);
      n.Bounds.Enlarge<IVector>(n.Vectors);
      PeriodicAxisSelector s = new PeriodicAxisSelector();
      s.Select(n);
    }
    
    [Test]
    public void TestRoot() {
      KdNode<IVector> n = new KdNode<IVector>();
      n.Vectors = new List<IVector>(new IVector[]{new Vector(1.0f, 0.5), new Vector(1.0f, 0.0), new Vector(1.0f, 0.0), new Vector(1.0f, 0.0)});
      n.Bounds = new AABB(2);
      n.Bounds.Enlarge<IVector>(n.Vectors);
      PeriodicAxisSelector s = new PeriodicAxisSelector();
      Assert.AreEqual(1, s.Select(n));
      
      n.Vectors[0][0] = 0.5;
      n.Bounds.Reset();
      n.Bounds.Enlarge<IVector>(n.Vectors);
      Assert.AreEqual(0, s.Select(n));
    }
    
    [Test]
    public void TestNonRoot() {
      KdNode<IVector> root = new KdNode<IVector>();
      KdNode<IVector> first = root.SetLeftChild(new KdNode<IVector>());
      KdNode<IVector> second = first.SetLeftChild(new KdNode<IVector>());
      KdNode<IVector> third = second.SetLeftChild(new KdNode<IVector>());
      
      third.Vectors = new List<IVector>(new IVector[]{new Vector(0.0, 0.5), new Vector(1.0f, 0.0), new Vector(1.0f, 0.0), new Vector(1.0f, 0.0)});
      third.Bounds = new AABB(2);
      third.Bounds.Enlarge<IVector>(third.Vectors);
      PeriodicAxisSelector s = new PeriodicAxisSelector();
      Assert.AreEqual(1, s.Select(third));
      
      second.Vectors = third.Vectors;
      second.Bounds = third.Bounds;
      Assert.AreEqual(0, s.Select(second));
    }
    
  }
}