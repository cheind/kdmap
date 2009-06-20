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

namespace AcceleratorsTests
{
  [TestFixture()]
	public class IssueTesting
	{
    [Test]
    public void Issue3() {
      // Test when split bounds is non-empty, but all contained points are degenerate
      KdNode<IVector> n = new KdNode<IVector>();
      n.Vectors = new List<IVector>(new IVector[] { Vector.Create(-1, -5), Vector.Create(-1, -5) });
      n.InternalBounds = new AABB(Vector.Create(-1.25, -5), Vector.Create(-1, -5));
      n.InternalBounds = new AABB(2);
      n.InternalBounds.Enlarge<IVector>(n.Vectors);
      AxisOfMaximumSpreadSelector aom = new AxisOfMaximumSpreadSelector();
      PeriodicAxisSelector pas = new PeriodicAxisSelector();
      Assert.Throws(typeof(DegenerateDatasetException), delegate { aom.Select(n); });
      Assert.Throws(typeof(DegenerateDatasetException), delegate { pas.Select(n); });
    }
	}
}
