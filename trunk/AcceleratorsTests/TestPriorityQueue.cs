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

namespace AcceleratorsTests
{
  
  
  [TestFixture()]
  public class TestPriorityQueue
  {
    
    [Test()]
    public void TestPush()
    {
      Random r = new Random(10);
      PriorityQueue<double, bool> q = new PriorityQueue<double, bool>();
      
      const int count = 10000;
      for (int i = 0; i < count; ++i) {
        q.Push(r.NextDouble(), false);
      }
      
      Assert.AreEqual(count, q.Count);
      double max = q.PeekPriority();
      while (q.Count > 0) {
        double p = q.PeekPriority(); q.Pop();
        Assert.GreaterOrEqual(p, max);
        max = p;
      }
    }
    
    [Test]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestPopEmpty() {
      PriorityQueue<double, bool> q = new PriorityQueue<double, bool>();
      q.Pop();
    }
    
    [Test]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestPeekEmpty() {
      PriorityQueue<double, bool> q = new PriorityQueue<double, bool>();
      q.Peek();
    }

  
    [Test]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestPeekPriorityEmpty() {
      PriorityQueue<double, bool> q = new PriorityQueue<double, bool>();
      q.PeekPriority();
    }
  }
}
