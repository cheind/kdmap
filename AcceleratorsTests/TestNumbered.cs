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

namespace AcceleratorsTests 
{
  [TestFixture()]
	public class TestNumbered
	{
    [Test]
    public void TestFirstSecondThird() {
      int[] numbers = new int[] { 3, 6, 8, 10, 12, 14 };
      Assert.AreEqual(3, Accelerators.Numbered.First(numbers));
      Assert.AreEqual(6, Accelerators.Numbered.Second(numbers));
      Assert.AreEqual(8, Accelerators.Numbered.Third(numbers));

      numbers = new int[] {};
      NUnitExtensions.Assert.Throws(typeof(InvalidOperationException), delegate { Numbered.First(numbers); });
      NUnitExtensions.Assert.Throws(typeof(InvalidOperationException), delegate { Numbered.Second(numbers); });
      NUnitExtensions.Assert.Throws(typeof(InvalidOperationException), delegate { Numbered.Third(numbers); });
    }

    [Test]
    public void TestFirstSecondThirdOrDefault() {
      int[] numbers = new int[] { 3, 6, 8, 10, 12, 14 };
      Assert.AreEqual(3, Accelerators.Numbered.FirstOrDefault(numbers));
      Assert.AreEqual(6, Accelerators.Numbered.SecondOrDefault(numbers));
      Assert.AreEqual(8, Accelerators.Numbered.ThirdOrDefault(numbers));

      numbers = new int[] { };
      Assert.AreEqual(0, Accelerators.Numbered.FirstOrDefault(numbers));
      Assert.AreEqual(0, Accelerators.Numbered.SecondOrDefault(numbers));
      Assert.AreEqual(0, Accelerators.Numbered.ThirdOrDefault(numbers));
    }
    
    [Test]
    public void TestTryFirstSecondThird() {
      int[] numbers = new int[] { 3, 6, 8, 10, 12, 14 };
      int val;
      Assert.IsTrue(Accelerators.Numbered.TryFirst(numbers, out val));
      Assert.AreEqual(3, val);
      Assert.IsTrue(Accelerators.Numbered.TrySecond(numbers, out val));
      Assert.AreEqual(6, val);
      Assert.IsTrue(Accelerators.Numbered.TryThird(numbers, out val));
      Assert.AreEqual(8, val);
      Assert.IsFalse(Accelerators.Numbered.TryNth(numbers, 10, out val));

      numbers = new int[] { };
      Assert.IsFalse(Accelerators.Numbered.TryFirst(numbers, out val));
      Assert.IsFalse(Accelerators.Numbered.TrySecond(numbers, out val));
      Assert.IsFalse(Accelerators.Numbered.TryThird(numbers, out val));
    }
	}
}
