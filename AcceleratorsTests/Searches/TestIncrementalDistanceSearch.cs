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
using Accelerators.Searches;
using System.Collections.Generic;

namespace AcceleratorsTests.Searches
{
  
  [TestFixture()]
  public class TestIncrementalDistanceSearch
  {
      private KdTree<Vector> _tree;
      private Vector[] _vecs;
    
    public TestIncrementalDistanceSearch() {
      _vecs = new Vector[] { Vector.Create(-1.0, -1.0), Vector.Create(0.0, 0.0), Vector.Create(1.0, 1.0), Vector.Create(2.0, 2.0) };
      _tree = new KdTree<Vector>(_vecs, new Accelerators.Subdivision.SubdivisionPolicyConnector(1));
    }
    
    [Test()]
    public void TestNoLimits()
    {
      IncrementalDistanceSearch<Vector> ids = new IncrementalDistanceSearch<Vector>(_tree.Root);
      List<Vector> ordered = new List<Vector>(ids.Find(Vector.Create(-1.0, -1.0)));

      Assert.AreEqual(ordered.Count, 4);
      Assert.IsTrue(VectorComparison.Equal(ordered[0], _vecs[0]));
      Assert.IsTrue(VectorComparison.Equal(ordered[1], _vecs[1]));
      Assert.IsTrue(VectorComparison.Equal(ordered[2], _vecs[2]));
      Assert.IsTrue(VectorComparison.Equal(ordered[3], _vecs[3]));
      
      ordered = new List<Vector>(ids.Find(Vector.Create(5.0, 5.0)));
      Assert.IsTrue(VectorComparison.Equal(ordered[0], _vecs[3]));
      Assert.IsTrue(VectorComparison.Equal(ordered[1], _vecs[2]));
      Assert.IsTrue(VectorComparison.Equal(ordered[2], _vecs[1]));
      Assert.IsTrue(VectorComparison.Equal(ordered[3], _vecs[0]));
    }
    
    [Test()]
    public void TestLimitCount()
    {
      IncrementalDistanceSearch<Vector> ids = new IncrementalDistanceSearch<Vector>(_tree.Root);
      ids.CountLimit = 2;
      List<Vector> ordered = new List<Vector>(ids.Find(Vector.Create(-1.0, -1.0)));

      Assert.AreEqual(2, ordered.Count);
      Assert.IsTrue(VectorComparison.Equal(ordered[0], _vecs[0]));
      Assert.IsTrue(VectorComparison.Equal(ordered[1], _vecs[1]));
      
      ordered = new List<Vector>(ids.Find(Vector.Create(5.0, 5.0)));
      Assert.AreEqual(2, ordered.Count);
      Assert.IsTrue(VectorComparison.Equal(ordered[0], _vecs[3]));
      Assert.IsTrue(VectorComparison.Equal(ordered[1], _vecs[2]));
    }
    
    [Test()]
    public void TestLimitDistance()
    {
      IncrementalDistanceSearch<Vector> ids = new IncrementalDistanceSearch<Vector>(_tree.Root);
      ids.DistanceLimit = 0.1;
      ids.CountLimit = 2;
      List<Vector> ordered = new List<Vector>(ids.Find(Vector.Create(-1.0, -1.0)));

      Assert.AreEqual(1, ordered.Count);
      Assert.IsTrue(VectorComparison.Equal(ordered[0], _vecs[0]));
      
      
      ordered = new List<Vector>(ids.Find(Vector.Create(5.0, 5.0)));
      Assert.AreEqual(0, ordered.Count);
    }
  }
}
