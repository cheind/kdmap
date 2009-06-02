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
	public class TestAABB
	{
		[Test]
		public void TestConstructor() {
			AABB a = new AABB(2);
			Assert.AreEqual(a.Lower.Dimensions, 2);
			Assert.AreEqual(a.Upper.Dimensions, 2);
			Assert.AreEqual(Single.MaxValue, a.Lower[0]);
			Assert.AreEqual(Single.MaxValue, a.Lower[1]);
			Assert.AreEqual(-Single.MaxValue, a.Upper[0]);
			Assert.AreEqual(-Single.MaxValue, a.Upper[1]);
			Assert.IsTrue(a.Empty);
		}
		
		[Test]
		public void TestEmpty() {
			AABB a = new AABB(20);
			Assert.IsTrue(a.Empty);
			a.Lower[10] = 1.0f;
			Assert.IsFalse(a.Empty);
		}
		
		[Test]
		public void TestReset() {
			AABB a = new AABB(20);
			a.Lower[10] = 1.0f;
			a.Upper[10] = 1.0f;
			Assert.IsFalse(a.Empty);
			a.Reset();
			Assert.IsTrue(a.Empty);
		}
		
		[Test]
		public void TestEnlarge() {
		}
	}
}
