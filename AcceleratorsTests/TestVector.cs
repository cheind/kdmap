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
