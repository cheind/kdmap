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
  public class TestVectorOperations
  {
    
    [Test()]
    public void TestAdd()
    {
      Vector a = Vector.Create(1.0, 2.0);
      Vector b = Vector.Create(-1.0, 3.0);
      VectorOperations.Add(a, b, a);
      Assert.IsTrue(VectorComparison.Close(a, Vector.Create(0.0, 5.0), FloatComparison.DefaultEps));
    }
    
    [Test()]
    public void TestSub()
    {
      Vector a = Vector.Create(1.0, 2.0);
      Vector b = Vector.Create(-1.0, 3.0);
      VectorOperations.Sub(a, b, a);
      Assert.IsTrue(VectorComparison.Close(a, Vector.Create(2.0, -1.0), FloatComparison.DefaultEps));
    }
    
    [Test()]
    public void TestScalarMul()
    {
      Vector a = Vector.Create(1.0, 2.0);
      VectorOperations.ScalarMul(a, 0.5, a);
      Assert.IsTrue(VectorComparison.Close(a, Vector.Create(0.5, 1.0), FloatComparison.DefaultEps));
    }
    
    [Test()]
    public void TestInner()
    {
      Vector a = Vector.Create(1.0, 2.0);
      Vector b = Vector.Create(-1.0, 3.0);
      Assert.IsTrue(FloatComparison.Close(VectorOperations.Inner(a,b), 5.0, FloatComparison.DefaultEps));
    }
    
    [Test()]
    public void TestNormalize()
    {
      Vector a = Vector.Create(1.0, 2.0);
      Vector na = new Vector(2);
      double old_len = VectorOperations.Normalize(a, na);
      Assert.AreEqual((double)Math.Sqrt(5.0), old_len, FloatComparison.DefaultEps);
      double new_len = VectorReductions.L2Norm(na);
      Assert.AreEqual(1.0, new_len, FloatComparison.DefaultEps);
    }
    
    [Test()]
    [ExpectedException(typeof(DivideByZeroException))]
    public void TestNormalizeZero() {
      Vector a =Vector.Create(0.0, 0.0);
      VectorOperations.Normalize(a, a);
    }
    
    [Test()]
    public void TestCopy() {
      Vector a = Vector.Create(3.0, 4.0);
      Vector b = new Vector(2);
      VectorOperations.Copy(a, b);
      Assert.IsTrue(VectorComparison.Close(a, b, FloatComparison.DefaultEps));      
    }
    
    [Test()]
    public void TestFill() {
      Vector a = new Vector(2);
      VectorOperations.Fill(a, 2.0);
      Assert.AreEqual(2.0, a[0], FloatComparison.DefaultEps);
      Assert.AreEqual(2.0, a[1], FloatComparison.DefaultEps);
    }
  }
}
