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
  public class TestMedianSelector
  {
    
    [Test()]
    public void TestSplitOneDimensional()
    {
      MedianSelector s = new MedianSelector();
      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[]{Vector.Create(1.0), Vector.Create(-1.0), Vector.Create(3.0), Vector.Create(2.0)});
      n.InternalBounds = new AABB(1);
      n.InternalBounds.Enlarge<Vector>(n.Vectors);
           
      Assert.AreEqual(2.0, s.Select(n, 0), FloatComparison.DefaultEps);
    }
    
    [Test]
    public void TestSplitMultiDimensional() {
      MedianSelector s = new MedianSelector();
      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[]{Vector.Create(1.0, 1.0), Vector.Create(1.0, -1.0), Vector.Create(1.0, 3.0), Vector.Create(1.0, 2.0)});
      n.InternalBounds = new AABB(2);
      n.InternalBounds.Enlarge<Vector>(n.Vectors);
      
      Assert.AreEqual(2.0, s.Select(n,1), FloatComparison.DefaultEps);
    }
    
    [Test]
    [ExpectedException(typeof(DegenerateDatasetException))]
    public void TestDegenerated1()
    {
     
      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[]{Vector.Create(1.0, 1.0), Vector.Create(1.0, 1.0), Vector.Create(1.0, 1.0), Vector.Create(1.0, 1.0)});
      n.InternalBounds = new AABB(2);
      n.InternalBounds.Enlarge<Vector>(n.Vectors);
      new MedianSelector().Select(n, 0);
    }

    
    [Test]
    [ExpectedException(typeof(DegenerateDatasetException))]
    public void TestDegenerated2()
    {    
      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[]{Vector.Create(1.0, 1.0), Vector.Create(2.0, 2.0), Vector.Create(2.0, 2.0), Vector.Create(2.0, 2.0)});
      n.InternalBounds = new AABB(2);
      n.InternalBounds.Enlarge<Vector>(n.Vectors);
      new MedianSelector().Select(n, 0);
    }
  }
}
