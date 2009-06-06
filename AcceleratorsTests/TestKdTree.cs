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
  public class TestKdTree
  {
    /// <summary>
    /// Demo class that implements IVector and is to be stored in KdTree
    /// </summary>
    class Flag : IVector {
      public Flag(float x, string name) {
        _coords = new float[1] {x};
        _name = name;
      }
      
      public Flag(float x, float y, string name) {
        _coords = new float[2] {x,y};
        _name = name;
      }
      
      public float this[int index] {
        get {
          return _coords[index];
        }
        set {
          _coords[index] = value;
        }
      }
      
      public int Dimensions {
        get {
          return _coords.Length;
        }
      }
      
      public string Name {
        get {
          return _name;
        }
      }
      
      
      private float[] _coords;
      private string _name;
    }
    
    [Test()]
    public void TestFind()
    {
      Flag[] flags = new Flag[] {new Flag(-1.0f, "a"), new Flag(1.0f, "b"), new Flag(1.4f, "c"), new Flag(3.0f, "d")};
      KdTree<Flag> tree = new KdTree<Flag>(flags, new MedianSubdivisionPolicy(1));
      
      Flag x = tree.Find(new Vector(1.0f));
      Assert.IsNotNull(x);
      Assert.AreEqual("b", x.Name);
      
      x = tree.Find(new Vector(1.4f));
      Assert.IsNotNull(x);
      Assert.AreEqual("c", x.Name);
      
      x = tree.Find(new Vector(1.3f));
      Assert.IsNull(x);
    }
  }
}
