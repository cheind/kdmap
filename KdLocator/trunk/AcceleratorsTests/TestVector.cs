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
	public class TestVector
	{
		[Test()]
		public void TestConstructorSet()
		{
			// Construct from dimensionality
			Vector v1 = new Vector(3);
			Assert.AreEqual(3, v1.Dimensions);
			v1[0] = 1.0f; v1[1] = 2.0f;
			
			// Construct from other vector
			Vector reference = new Vector(1.0f, 2.0f, 3.0f);
			Vector v2 = new Accelerators.Vector(reference);
			Assert.IsTrue(VectorComparison.Equal(reference, v2, FloatComparison.DefaultEps));
			Vector v3 = new Accelerators.Vector((IVector)reference);
			Assert.IsTrue(VectorComparison.Equal(reference, v3, FloatComparison.DefaultEps));
			
			// Construct from dimension and value
			Vector v4 = new Vector(2, 1.0f);
			Assert.AreEqual(1.0f, v4[0], FloatComparison.DefaultEps);
			Assert.AreEqual(1.0f, v4[1], FloatComparison.DefaultEps);
			
			Vector v5 = new Vector(1.0f, 2.0f);
			Assert.AreEqual(1.0f, v5[0], FloatComparison.DefaultEps);
			Assert.AreEqual(2.0f, v5[1], FloatComparison.DefaultEps);
			
			Vector v6 = new Vector(1.0f, 2.0f, 3.0f);
			Assert.AreEqual(1.0f, v6[0], FloatComparison.DefaultEps);
			Assert.AreEqual(2.0f, v6[1], FloatComparison.DefaultEps);
			Assert.AreEqual(3.0f, v6[2], FloatComparison.DefaultEps);
		}
	}
}
