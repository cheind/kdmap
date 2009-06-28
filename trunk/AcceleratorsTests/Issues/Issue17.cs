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
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Accelerators;
using Accelerators.Subdivision;

namespace AcceleratorsTests.Issues
{
  /// <summary>
  /// Issue #17: calling KdTree<T>.Clear will render all previously created searches invalid.
  /// http://code.google.com/p/kdmap/issues/detail?id=17
  /// </summary>
  [TestFixture()]
	public class Issue17
	{
    [Test]
    public void TestCallingClearAndSearch() {
      // Problem is that Clear destroys the root node.
      KdTree<Vector> tree = new KdTree<Vector>(2, new Accelerators.Subdivision.SubdivisionPolicyConnector(1));
      tree.Add(Vector.Create(1.0, 2.0));
      tree.Add(Vector.Create(1.0, 3.0));
      tree.Add(Vector.Create(1.0, 4.0));

      Accelerators.Searches.ExactSearch<Vector> es = new Accelerators.Searches.ExactSearch<Vector>(tree.Root);
      tree.Clear();

      Assert.IsTrue(Numbered.Empty(es.FindExact(Vector.Create(1.0, 3.0))));
      Assert.AreEqual(0, tree.Root.Vectors.Count);
      Assert.IsTrue(tree.Root.InternalBounds.Empty);
    }
	}
}
