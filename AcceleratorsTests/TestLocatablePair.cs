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

namespace TestBinaryNode
{
  [TestFixture()]
	public class TestLocatablePair
	{
    [Test]
    public void TestExplicitConversion() {
      List<LocatablePair<Vector, string>> list = new List<LocatablePair<Vector, string>>();
      list.Add(new LocatablePair<Vector, string>(Vector.Create(1.0), "a"));
      list.Add(new LocatablePair<Vector, string>(Vector.Create(2.0), "b"));
      list.Add(new LocatablePair<Vector, string>(Vector.Create(3.0), "c"));

      int count = 0;
      // Explicit conversion LocatablePair -> KeyValuePair
      foreach (KeyValuePair<Vector, string> p in list) {
        Assert.AreEqual(list[count].Second, p.Value);
        count += 1;
      }

      // Explicit conversion KeyValuePair -> LocatablePair
      ICollection<KeyValuePair<Vector, string>> col = new List<KeyValuePair<Vector, string>>();
      col.Add((KeyValuePair<Vector, string>)list[0]);
      col.Add((KeyValuePair<Vector, string>)list[1]);
      col.Add((KeyValuePair<Vector, string>)list[2]);

      count = 0;
      // Explicit conversion LocatablePair -> KeyValuePair
      foreach (KeyValuePair<Vector, string> p in col) {
        Assert.AreEqual(list[count].Second, p.Value);
        count += 1;
      }
    }

    [Test]
    public void TestConversionInSearch() {
      KdMap<Vector, string> map = new KdMap<Vector, string>(1);
      map[Vector.Create(0.0)] = "a";
      map[Vector.Create(1.0)] = "b";
      map[Vector.Create(2.0)] = "c";

      IncrementalDistanceSearch<LocatablePair<Vector, string>> s =
        new IncrementalDistanceSearch<LocatablePair<Vector, string>>(map.KdTree.Root);

      IEnumerable<LocatablePair<Vector, string>> e = (IEnumerable<LocatablePair<Vector, string>>)s.Find(Vector.Create(3.0));

      List<KeyValuePair<Vector, string>> l = new List<KeyValuePair<Vector, string>>((IEnumerable<KeyValuePair<Vector, string>>)e);

      Assert.AreEqual("c", l[0].Value);
      Assert.AreEqual("b", l[0].Value);
      Assert.AreEqual("a", l[0].Value);
    }
	}
}
