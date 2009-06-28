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

namespace Accelerators
{
    
  /// <summary>
  /// A kd-Tree implementation that supports dynamic insertion and removal.
  /// </summary>
  /// <remarks>
  /// Elements are stored only in leaf nodes.
  /// </remarks>
  public partial class KdTree<T> : ICollection<T> where T : IVector
  {
    
    /// <summary>
    /// Create empty kd-tree
    /// </summary>
    /// <param name="dimensions"></param>
    /// <param name="policy"></param>
    public KdTree(int dimensions, Subdivision.ISubdivisionPolicy policy) {
      _subdiv_policy = policy;
      _root = new KdNode<T>();
      this.InitializeNode(_root, dimensions);
      _cls = new Searches.ClosestLeafSearch<T>(_root);
      _count = 0;
    }

    /// <summary>
    /// Instance a new kd-tree with the given collection of points and a subdivision policy.
    /// </summary>
    public KdTree(IEnumerable<T> vecs, Subdivision.ISubdivisionPolicy policy)
    {
      _subdiv_policy = policy;
      _root = new KdNode<T>();
      this.InitializeNode(_root, vecs);
      _cls = new Searches.ClosestLeafSearch<T>(_root);
      _count = _root.Vectors.Count;
      this.TrySplit(_root);
    }

    /// <summary>
    /// Instance a new kd-tree with the given collection of points and a subdivision policy.
    /// </summary>
    /// <remarks>
    /// Helps scripting languages that do not support constructor overloading.
    /// </remarks>
    public static KdTree<T> FromEnumerable(IEnumerable<T> vecs, Subdivision.ISubdivisionPolicy policy) {
      return new KdTree<T>(vecs, policy);
    }
    
    /// <summary>
    /// Initialize a node based on elements.
    /// </summary>
    private void InitializeNode(KdNode<T> n, IEnumerable<T> vecs) {
      T first = Numbered.First(vecs);
      
      if (n.Vectors == null)
        n.Vectors = new List<T>();
      n.Vectors.Clear();
      n.Vectors.AddRange(vecs);

      if (n.InternalBounds == null)
        n.InternalBounds = new AABB(first.Dimensions);
      n.InternalBounds.Reset();
      n.InternalBounds.Enlarge(vecs);
    }

    /// <summary>
    /// Initialize a node base on dimensional info
    /// </summary>
    private void InitializeNode(KdNode<T> n, int dimensions) {
      n.Vectors = new List<T>();
      n.InternalBounds = new AABB(dimensions);
    }
    
    /// <summary>
    /// Perform recursive splitting as long as possible
    /// </summary>
    private void TrySplit(KdNode<T> target) {
      try {
        _subdiv_policy.Split(target);
        target.Vectors = null;
        TrySplit(target.Left);
        TrySplit(target.Right);
      } catch (Subdivision.SubdivisionException) {}
    }

    /// <summary>
    /// Perform a single collapse on the target node
    /// </summary>
    private KdNode<T> TryCollapse(KdNode<T> target) {
      try {
        KdNode<T> parent = target.Parent;
        _subdiv_policy.Collapse(target);
        return parent;
      } catch (Subdivision.SubdivisionException) {
        return target;
      }
    }
    
    /// <value>
    /// Access the root node of the tree.
    /// </value>
    public KdNode<T> Root {
      get {
        return _root;
      }
    }

    /// <summary>
    /// Optimize tree using the given subdivision policy.
    /// </summary>
    public void Optimize(Subdivision.ISubdivisionPolicy policy) {
      _subdiv_policy = policy;
      if (this.Count > 0) {
        // Save all elements
        T[] elements = new T[this.Count];
        this.CopyTo(elements, 0);
        // Clean-Up
        this.Clear(); // note: this sets number of elements to be zero.
        // Reconstruct
        this.InitializeNode(_root, elements);
        this.TrySplit(_root);
        // Update missing info
        _count = elements.Length;
      }
    }

    /// <summary>
    /// Optimize tree.
    /// </summary>
    public void Optimize() {
      this.Optimize(_subdiv_policy);
    }

    private Subdivision.ISubdivisionPolicy _subdiv_policy;
    private int _count; // number of elements in tree
    private KdNode<T> _root;
    private Searches.ClosestLeafSearch<T> _cls;
  }
}
