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
  public class TestVectorOperations
  {
    
    [Test()]
    public void TestAdd()
    {
      Vector a = Vector.Create(1.0, 2.0);
      Vector b = Vector.Create(-1.0, 3.0);
      VectorOperations.Add(a, b, ref a);
      Assert.IsTrue(VectorComparison.Close(a, Vector.Create(0.0, 5.0), FloatComparison.DefaultEps));
    }
    
    [Test()]
    public void TestSub()
    {
      Vector a = Vector.Create(1.0, 2.0);
      Vector b = Vector.Create(-1.0, 3.0);
      VectorOperations.Sub(a, b, ref a);
      Assert.IsTrue(VectorComparison.Close(a, Vector.Create(2.0, -1.0), FloatComparison.DefaultEps));
    }
    
    [Test()]
    public void TestScalarMul()
    {
      Vector a = Vector.Create(1.0, 2.0);
      VectorOperations.ScalarMul(a, 0.5, ref a);
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
      double old_len = VectorOperations.Normalize(a, ref na);
      Assert.AreEqual((double)Math.Sqrt(5.0), old_len, FloatComparison.DefaultEps);
      double new_len = VectorReductions.L2Norm(na);
      Assert.AreEqual(1.0, new_len, FloatComparison.DefaultEps);
    }
    
    [Test()]
    [ExpectedException(typeof(DivideByZeroException))]
    public void TestNormalizeZero() {
      Vector a =Vector.Create(0.0, 0.0);
      VectorOperations.Normalize(a, ref a);
    }
    
    [Test()]
    public void TestCopy() {
      Vector a = Vector.Create(3.0, 4.0);
      Vector b = new Vector(2);
      VectorOperations.Copy(a, ref b);
      Assert.IsTrue(VectorComparison.Close(a, b, FloatComparison.DefaultEps));      
    }
    
    [Test()]
    public void TestFill() {
      Vector a = new Vector(2);
      VectorOperations.Fill(ref a, 2.0);
      Assert.AreEqual(2.0, a[0], FloatComparison.DefaultEps);
      Assert.AreEqual(2.0, a[1], FloatComparison.DefaultEps);
    }
  }
}
