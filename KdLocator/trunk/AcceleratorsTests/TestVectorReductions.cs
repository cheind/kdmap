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
	public class TestVectorReductions
	{
				
		[Test()]
		public void TestSquaredL2Norm()
		{
			Vector a = new Vector(1.0f, 2.0f);
			Assert.IsTrue(FloatComparison.Close(VectorReductions.SquaredL2Norm(a), 5.0f, FloatComparison.DefaultEps));
		}
		
		[Test()]
		public void TestL2Norm()
		{
			Vector a = new Vector(1.0f, 2.0f);
			Assert.IsTrue(FloatComparison.Close(VectorReductions.L2Norm(a), (float)Math.Sqrt(5.0f), FloatComparison.DefaultEps));
		}
	}
}
