// 
//  Copyright (c) 2009, Christoph Heindl
//  All rights reserved.
// 
//  Redistribution and use in source and binary forms, with or without modification, are 
//  permitted provided that the following conditions are met:
// 
//  Redistributions of source code must retain the above copyright notice, this list of 
//  conditions and the following disclaimer. 
//  Redistributions in binary form must reproduce the above copyright notice, this list 
//  of conditions and the following disclaimer in the documentation and/or other materials 
//  provided with the distribution. 
//  Neither the name Christoph Heindl nor the names of its contributors may be used to endorse 
//  or promote products derived from this software without specific prior written permission.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS 
//  OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
//  AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
//  DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
//  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
//  IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
//  OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 

using System;
using NUnit.Framework;
using Accelerators;
using Accelerators.Searches;
using System.Collections.Generic;

namespace AcceleratorsTests
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
