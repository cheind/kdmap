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
using System.Collections.Generic;

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
    
    [Test]
    public void FindInsideVolumeNumerically()
    {
      Vector[] vecs = new Vector[]{new Vector(-1.0f, -1.0f), new Vector(1.0f, 1.0f), new Vector(2.0f, 2.0f), new Vector(3.0f, 3.0f)};
      KdTree<Vector> tree = new KdTree<Vector>(vecs, new MedianSubdivisionPolicy(1));
      
      List<Vector> found = new List<Vector>(
        tree.FindInsideVolume(
          new AABB(new Vector(0.5f, 0.5f), new Vector(2.5f, 2.5f))
        )
      );
      
      Assert.AreEqual(found.Count, 2);
      Assert.IsTrue(VectorComparison.Equal(vecs[1], found[0]));
      Assert.IsTrue(VectorComparison.Equal(vecs[2], found[1]));
      
      found =  new List<Vector>(
        tree.FindInsideVolume(
          new AABB(new Vector(0.0f, 0.0f), new Vector(0.5f, 0.5f))
        )
      );
      
      Assert.IsEmpty(found);

      found =  new List<Vector>(
        tree.FindInsideVolume(
          new AABB(new Vector(-2.0f, -2.0f), new Vector(4.5f, 4.5f))
        )
      );
      
      Assert.AreEqual(4, found.Count);
      Assert.IsTrue(VectorComparison.Equal(vecs[0], found[0]));
      Assert.IsTrue(VectorComparison.Equal(vecs[1], found[1]));
      Assert.IsTrue(VectorComparison.Equal(vecs[2], found[2]));
      Assert.IsTrue(VectorComparison.Equal(vecs[3], found[3]));
    }
    
    class CountingBV : IBoundingVolume {
      
      public CountingBV(AABB aabb) {
        _aabb = aabb;
        _count = 0;
      }
      
      public int Count {
        get {
          return _count;
        }
      }
      
     
      #region IBoundingVolume implementation
      public bool Inside (IVector x)
      {
        _count += 1;
        return _aabb.Inside(x);
      }
      
      public bool Intersect (AABB aabb)
      {
        return _aabb.Intersect(aabb);
      }
      
      public EPlanePosition ClassifyPlane (int dimension, float position)
      {
        return _aabb.ClassifyPlane(dimension, position);
      }
      
      public int Dimensions {
        get {
          return _aabb.Dimensions;
        }
      }
      #endregion 
      
      private AABB _aabb;
      private int _count;
    }
    
    [Test]
    public void FindInsideVolumeAnalytically() {
      Vector[] vecs = new Vector[]{new Vector(-1.0f, -1.0f), new Vector(0.0f, 0.0f), new Vector(1.0f, 1.0f), new Vector(2.0f, 2.0f)};
      KdTree<Vector> tree = new KdTree<Vector>(vecs, new MedianSubdivisionPolicy(1));
     
      CountingBV cbv = new CountingBV(new AABB(new Vector(0.5f, 0.5f), new Vector(2.5f, 2.5f)));
      List<Vector> found = new List<Vector>(tree.FindInsideVolume(cbv));
      Assert.AreEqual(2, cbv.Count);
      Assert.AreEqual(2, found.Count);
    }

    /// <summary>
    /// Invert logic of comparator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class InvertedComparer<T> : IComparer<T> {
      public InvertedComparer(IComparer<T> comp) {
        _comp = comp;
      }

      public int Compare(T x, T y) {
        return _comp.Compare(y, x);
      }


      private IComparer<T> _comp;
    }

    [Test]
    public void FindInSortedOrder() {

      Vector[] vecs = new Vector[] { new Vector(-1.0f, -1.0f), new Vector(0.0f, 0.0f), new Vector(1.0f, 1.0f), new Vector(2.0f, 2.0f) };
      KdTree<Vector> tree = new KdTree<Vector>(vecs, new MedianSubdivisionPolicy(1));
      List<Vector> order_min = new List<Vector>(tree.FindInSortedOrder(new Vector(-1.0f, -1.0f), 10.0f));

      Assert.AreEqual(order_min.Count, 4);
      Assert.IsTrue(VectorComparison.Equal(order_min[0], vecs[0]));
      Assert.IsTrue(VectorComparison.Equal(order_min[1], vecs[1]));
      Assert.IsTrue(VectorComparison.Equal(order_min[2], vecs[2]));
      Assert.IsTrue(VectorComparison.Equal(order_min[3], vecs[3]));
      
      order_min = new List<Vector>(tree.FindInSortedOrder(new Vector(-1.0f, -1.0f), 1.5f));
      Assert.AreEqual(order_min.Count, 2);
      Assert.IsTrue(VectorComparison.Equal(order_min[0], vecs[0]));
      Assert.IsTrue(VectorComparison.Equal(order_min[1], vecs[1]));
      
    }
  }
}
