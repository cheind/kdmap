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
  public class TestKdTree
  {
    [Test]
    public void TestStaticConstruction() {
      KdTree<IVector> tree = new KdTree<IVector>(VectorSampling.InAABB(5000, 2, -100.0, 100.0, 10), new SubdivisionPolicyConnector(1));
      KdNodeInvariants.AreMetBy(tree.Root);
      int count = 0;
      foreach (KdNode<IVector> n in tree.Root.Leaves) {
        count += n.Vectors.Count;
      }
      Assert.AreEqual(5000, count);
    }
    
  }
}
