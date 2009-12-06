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
