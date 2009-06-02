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
	public class TestVectorOps
	{
		
		[Test()]
		public void TestAdd()
		{
			Vector a = new Vector(1.0f, 2.0f);
			Vector b = new Vector(-1.0f, 3.0f);
			VectorOps.Add(a, b, a);
			Assert.IsTrue(VectorComparison.Close(a, new Vector(0.0f, 5.0f), FloatComparison.DefaultEps));
		}
		
		[Test()]
		public void TestSub()
		{
			Vector a = new Vector(1.0f, 2.0f);
			Vector b = new Vector(-1.0f, 3.0f);
			VectorOps.Sub(a, b, a);
			Assert.IsTrue(VectorComparison.Close(a, new Vector(2.0f, -1.0f), FloatComparison.DefaultEps));
		}
		
		[Test()]
		public void TestScalarMul()
		{
			Vector a = new Vector(1.0f, 2.0f);
			VectorOps.ScalarMul(a, 0.5f, a);
			Assert.IsTrue(VectorComparison.Close(a, new Vector(0.5f, 1.0f), FloatComparison.DefaultEps));
		}
		
		[Test()]
		public void TestInner()
		{
			Vector a = new Vector(1.0f, 2.0f);
			Vector b = new Vector(-1.0f, 3.0f);
			Assert.IsTrue(FloatComparison.Close(VectorOps.Inner(a,b), 5.0f, FloatComparison.DefaultEps));
		}
		
		[Test()]
		public void TestSquareLength()
		{
			Vector a = new Vector(1.0f, 2.0f);
			Assert.IsTrue(FloatComparison.Close(VectorOps.SquareLength(a), 5.0f, FloatComparison.DefaultEps));
		}
		
		[Test()]
		public void TestLength()
		{
			Vector a = new Vector(1.0f, 2.0f);
			Assert.IsTrue(FloatComparison.Close(VectorOps.Length(a), (float)Math.Sqrt(5.0f), FloatComparison.DefaultEps));
		}
		
		[Test()]
		public void TestNormalize()
		{
			Vector a = new Vector(1.0f, 2.0f);
			Vector na = new Vector(2);
			float old_len = VectorOps.Normalize(a, na);
			Assert.AreEqual((float)Math.Sqrt(5.0f), old_len, FloatComparison.DefaultEps);
			float new_len = VectorOps.Length(na);
			Assert.AreEqual(1.0f, new_len, FloatComparison.DefaultEps);
		}
		
		[Test()]
		[ExpectedException(typeof(DivideByZeroException))]
		public void TestNormalizeZero() {
			Vector a =new Vector(0.0f, 0.0f);
			VectorOps.Normalize(a, a);
		}
		
		[Test()]
		public void TestCopy() {
			Vector a = new Vector(3.0f, 4.0f);
			Vector b = new Vector(2);
			VectorOps.Copy(a, b);
			Assert.IsTrue(VectorComparison.Close(a, b, FloatComparison.DefaultEps));			
		}
		
		[Test()]
		public void TestFill() {
			Vector a = new Vector(2);
			VectorOps.Fill(a, 2.0f);
			Assert.AreEqual(2.0f, a[0], FloatComparison.DefaultEps);
			Assert.AreEqual(2.0f, a[1], FloatComparison.DefaultEps);
		}
	}
}
