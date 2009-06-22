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
