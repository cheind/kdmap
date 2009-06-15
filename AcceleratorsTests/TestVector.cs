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
      v1[0] = 1.0; v1[1] = 2.0;
      
      // Construct from other vector
      Vector reference = Vector.Create(1.0, 2.0, 3.0);
      Vector v2 = new Accelerators.Vector(reference);
      Assert.IsTrue(VectorComparison.Close(reference, v2, FloatComparison.DefaultEps));
      Vector v3 = new Accelerators.Vector((IVector)reference);
      Assert.IsTrue(VectorComparison.Close(reference, v3, FloatComparison.DefaultEps));
      
      Vector v5 = Vector.Create(1.0, 2.0);
      Assert.AreEqual(1.0, v5[0], FloatComparison.DefaultEps);
      Assert.AreEqual(2.0, v5[1], FloatComparison.DefaultEps);
      
      Vector v6 = Vector.Create(1.0, 2.0, 3.0);
      Assert.AreEqual(1.0, v6[0], FloatComparison.DefaultEps);
      Assert.AreEqual(2.0, v6[1], FloatComparison.DefaultEps);
      Assert.AreEqual(3.0, v6[2], FloatComparison.DefaultEps);
    }
    
    [Test()]
    public void TestIVectorInterface() {
      Vector v = Vector.Create(1.0, 2.0, 3.0);
      IVector iv = v;
      
      Assert.AreEqual(3, iv.Dimensions);
      Assert.AreEqual(1.0, iv[0], FloatComparison.DefaultEps);
      Assert.AreEqual(2.0, iv[1], FloatComparison.DefaultEps);
      Assert.AreEqual(3.0, iv[2], FloatComparison.DefaultEps);
    }
    
    [Test]
    public void TestOperatorOverloadingPlus() {
      Vector a = Vector.Create(1.0, 2.0, 3.0);
      Vector b = Vector.Create(1.0, 2.0, 3.0);
      Vector c = a + b;
      Assert.IsTrue(VectorComparison.Close(c, Vector.Create(2.0, 4.0, 6.0), FloatComparison.DefaultEps));
      a += b;
      Assert.IsTrue(VectorComparison.Close(a, Vector.Create(2.0, 4.0, 6.0), FloatComparison.DefaultEps));
    }
    
    [Test]
    public void TestOperatorOverloadingSub() {
      Vector a = Vector.Create(4.0, 5.0, 6.0);
      Vector b = Vector.Create(1.0, 2.0, 3.0);
      Vector c = a - b;
      Assert.IsTrue(VectorComparison.Close(c, Vector.Create(3.0, 3.0, 3.0), FloatComparison.DefaultEps));
      a -= b;
      Assert.IsTrue(VectorComparison.Close(a, Vector.Create(3.0, 3.0, 3.0), FloatComparison.DefaultEps));
    }
  }
}
