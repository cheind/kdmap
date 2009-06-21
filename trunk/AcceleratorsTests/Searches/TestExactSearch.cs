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
  public class TestExactSearch
  {
    private KdTree<Vector> _tree;
    private List<Vector> _vecs;
    
    public TestExactSearch() {
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
    public void TestExistingSingle()
    {
      ExactSearch<Vector> es = new ExactSearch<Vector>(_tree.Root);
      Assert.AreSame(_vecs[2], Numbered.First(es.FindExact(Vector.Create(-2.0, 3.0))));
      Assert.AreEqual(_vecs[2], Numbered.First(es.FindExact(Vector.Create(-2.0, 3.0))));
    }
    
    [Test]
    public void TestExistingDuplicates() {
      ExactSearch<Vector> es = new ExactSearch<Vector>(_tree.Root);
      IEnumerable<Vector> en = es.FindExact(Vector.Create(1.0, 5.0));
      Assert.IsTrue(VectorComparison.Equal(Numbered.First(en), Vector.Create(1.0, 5.0)));
      Assert.IsTrue(VectorComparison.Equal(Numbered.Second(en), Vector.Create(1.0, 5.0)));
      Assert.IsTrue(VectorComparison.Equal(Numbered.Third(en), Vector.Create(1.0, 5.0)));
    }
    
    [Test]
    public void TestNonExisting() {
      ExactSearch<Vector> es = new ExactSearch<Vector>(_tree.Root);
      List<Vector> elements = new List<Vector>(es.FindExact(Vector.Create(4.0, 6.0)));
      Assert.AreEqual(0, elements.Count);
    }
    
    [Test]
    public void TestLimitResultset() {
      ExactSearch<Vector> es = new ExactSearch<Vector>(_tree.Root);
      es.Limit = 2;
      List<Vector> elements = new List<Vector>(es.FindExact(Vector.Create(1.0, 5.0)));
      Assert.AreEqual(2, elements.Count);
    }
  }
}
