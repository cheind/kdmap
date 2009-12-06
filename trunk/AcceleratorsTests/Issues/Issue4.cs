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

namespace AcceleratorsTests.Issues
{
  /// <summary>
  /// Issue #4: VectorOperations should use 'ref'
  /// </summary>
  [TestFixture]
	public class Issue4
	{
    /// <summary>
    /// Entity that implements IVector and is a struct
    /// </summary>
    struct VectorByValue : IVector {
      
      public VectorByValue(double x, double y) {
        _x = x;
        _y = y;
      }

      public int Dimensions {
        get {return 2;}
      }

      public double this[int index] {
        get { return index == 0 ? _x : _y;}
        set {
          if (index == 0)
            _x = value;
          else
            _y = value;
        }
      }

      private double _x, _y;
    }

    private VectorByValue _a, _b, _dest;

    public Issue4() {
      _a = new VectorByValue(1.0, 2.0);
      _b = new VectorByValue(3.0, 4.0);
      _dest = new VectorByValue();
    }

    [Test]
    public void TestVectorOperations() {
      VectorOperations.Sub(_a, _b, ref _dest);
      Assert.AreEqual(-2.0, _dest[0], FloatComparison.DefaultEps);
      Assert.AreEqual(-2.0, _dest[1], FloatComparison.DefaultEps);

      VectorOperations.Add(_a, _b, ref _dest);
      Assert.AreEqual(4.0, _dest[0], FloatComparison.DefaultEps);
      Assert.AreEqual(6.0, _dest[1], FloatComparison.DefaultEps);

      VectorOperations.ScalarMul(_a, 2, ref _dest);
      Assert.AreEqual(2.0, _dest[0], FloatComparison.DefaultEps);
      Assert.AreEqual(4.0, _dest[1], FloatComparison.DefaultEps);

      VectorOperations.Copy(_a, ref _dest);
      Assert.AreEqual(1.0, _dest[0], FloatComparison.DefaultEps);
      Assert.AreEqual(2.0, _dest[1], FloatComparison.DefaultEps);

      VectorOperations.Normalize(_a, ref _dest);
      Assert.AreEqual(0.44721359549995793, _dest[0], FloatComparison.DefaultEps);
      Assert.AreEqual(0.89442719099991586, _dest[1], FloatComparison.DefaultEps);

      VectorOperations.Fill(ref _dest, 5.0);
      Assert.AreEqual(5.0, _dest[0], FloatComparison.DefaultEps);
      Assert.AreEqual(5.0, _dest[1], FloatComparison.DefaultEps);

    }

	}
}
