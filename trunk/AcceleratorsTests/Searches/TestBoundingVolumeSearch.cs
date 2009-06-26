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
  public class TestBoundingVolumeSearch
  {
    
    private KdTree<Vector> _tree;
    private List<Vector> _vecs;
  
    public TestBoundingVolumeSearch() {
      _vecs = new List<Vector>();
      _vecs.Add(Vector.Create(-1.0, 1.0));
      _vecs.Add(Vector.Create(-1.0, 1.0));
      _vecs.Add(Vector.Create(-2.0, 3.0));
      _vecs.Add(Vector.Create(-4.0, 5.0));
      _vecs.Add(Vector.Create(1.0, 5.0));
      _vecs.Add(Vector.Create(1.0, 5.0));
      _vecs.Add(Vector.Create(1.0, 5.0));
      _tree = new KdTree<Vector>(_vecs, new Accelerators.Subdivision.SubdivisionPolicyConnector(1)); 
    }
  
    [Test()]
    public void TestBallContainsAll()
    {
      // Ball encapsulating entire region
      IVector diag = _tree.Root.InternalBounds.Diagonal;
      IVector center = _tree.Root.InternalBounds.Center;
      double len_half = VectorReductions.L2Norm(diag) * 0.5;
      Ball b = new Ball(center, len_half);
      
      BoundingVolumeSearch<Vector> s = new BoundingVolumeSearch<Vector>(_tree.Root);
      List<Vector> result = new List<Vector>(s.FindInsideBoundingVolume(b));
      
      Assert.AreEqual(7, result.Count);
    }
    
    [Test()]
    public void TestBallContainsNone()
    {
      Ball b = new Ball(Vector.Create(-0.5, -0.5), 0.1);
      
      BoundingVolumeSearch<Vector> s = new BoundingVolumeSearch<Vector>(_tree.Root);
      List<Vector> result = new List<Vector>(s.FindInsideBoundingVolume(b));
      Assert.AreEqual(0, result.Count);
    }
    
    [Test()]
    public void TestBallContainsSome()
    {
      Ball b = new Ball(Vector.Create(1.0, 4.0), 1.0);
      
      BoundingVolumeSearch<Vector> s = new BoundingVolumeSearch<Vector>(_tree.Root);
      List<Vector> result = new List<Vector>(s.FindInsideBoundingVolume(b));
      Assert.AreEqual(3, result.Count);
      Assert.IsTrue(VectorComparison.Equal(result[0], _vecs[4]));
      Assert.IsTrue(VectorComparison.Equal(result[1], _vecs[5]));
      Assert.IsTrue(VectorComparison.Equal(result[2], _vecs[6]));
    }
  }
}
