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
      _root = this.CreateRootNode(dimensions);
      _count = 0;
      _equality_comp = EqualityComparer<T>.Default;
    }

    /// <summary>
    /// Instance a new kd-tree with the given collection of points and a subdivision policy.
    /// </summary>
    public KdTree(IEnumerable<T> vecs, Subdivision.ISubdivisionPolicy policy)
    {
      _subdiv_policy = policy;
      _root = this.CreateRootNode(vecs);
      _count = _root.Vectors.Count;
      _equality_comp = EqualityComparer<T>.Default;
      this.Split(_root);
    }
    
    /// <summary>
    /// Creates the root kd-tree node that contains the entire scene
    /// </summary>
    private KdNode<T> CreateRootNode(IEnumerable<T> vecs) {
      T first;
      if (!this.FirstFromEnumerable(vecs, out first))
        throw new ArgumentOutOfRangeException("Cannot work on empty collection");
      
      KdNode<T> n = new KdNode<T>();
      n.InternalBounds = new AABB(first.Dimensions);
      n.InternalBounds.Enlarge(vecs);
      n.Vectors = new List<T>(vecs);
      return n;
    }

    /// <summary>
    /// Create empty root node
    /// </summary>
    private KdNode<T> CreateRootNode(int dimensions) {
      KdNode<T> n = new KdNode<T>();
      n.InternalBounds = new AABB(dimensions);
      n.Vectors = new List<T>();
      return n;
    }
    
    /// <summary>
    /// Find the first element in the Enumerable.
    /// </summary>
    private bool FirstFromEnumerable(IEnumerable<T> vecs, out T first) {
      bool non_empty = false;
      first = default(T);
      using (IEnumerator<T> e = vecs.GetEnumerator()) {
        if (e.MoveNext()) {
          first = e.Current;
          non_empty = true;
        }
      }
      return non_empty;
    }
    
    /// <summary>
    /// Perform recursive splitting as long as possible
    /// </summary>
    private void Split(KdNode<T> target) {
      try {
        _subdiv_policy.Split(target);
        target.Vectors = null;
        Split(target.Left);
        Split(target.Right);
      } catch (Subdivision.SubdivisionException) {}
    }

    /// <summary>
    /// Perform a single collapse on the target node
    /// </summary>
    private bool Collapse(KdNode<T> target) {
      if (target.Root) {
        return false;
      } else {
        try {
          _subdiv_policy.Collapse(target.Parent);
          return true;
        } catch (Subdivision.SubdivisionException) {
          return false;
        }
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
    /// Add an item to the kd-tree.
    /// </summary>
    /// <remarks>
    /// Duplicate IVectors are allowed to be contained.
    /// </remarks>
    public void Add(T item) {
      KdNode<T> leaf = this.FindClosestLeaf(item);
      bool need_stretching = !leaf.InternalBounds.Inside(item);
      leaf.Vectors.Add(item);
      
      if (need_stretching)
        this.StretchAncestorBounds(leaf, item);
      else
        leaf.InternalBounds.Enlarge(item);

      this.Split(leaf);
      _count += 1;
    }

    /// <summary>
    /// Enlarge the internal bounds of all ancestors of the target node including
    /// the target node itself.
    /// </summary>
    /// <param name="target"></param>
    /// <param name="item"></param>
    private void StretchAncestorBounds(KdNode<T> target, T item) {
      foreach(KdNode<T> n in target.Ancestors) {
        n.InternalBounds.Enlarge(item);
      }
    }

    public bool Remove(T item) {
      KdNode<T> leaf = this.FindClosestLeaf(item);
      int index = leaf.Vectors.FindIndex(delegate(T obj) { return VectorComparison.Equal(item, obj); });
      throw new Exception("The method or operation is not implemented.");
    }

    /// <summary>
    /// Remove all items.
    /// </summary>
    public void Clear() {
      throw new Exception("The method or operation is not implemented.");
    }

    /// <summary>
    /// Determines whether the kd-tree contains a specific value.
    /// </summary>
    public bool Contains(T item) {
      throw new Exception("The method or operation is not implemented.");
    }

    /// <summary>
    /// Copies the elements of the kd-tree to an Array, starting at a particular Array index.
    /// </summary>
    public void CopyTo(T[] array, int arrayIndex) {
      throw new Exception("The method or operation is not implemented.");
    }

    /// <summary>
    /// Gets the number of elements contained.
    /// </summary>
    public int Count {
      get {
        return _count;
      }
    }

    /// <summary>
    /// Gets a value indicating whether the kd-tree is read only.
    /// </summary>
    public bool IsReadOnly {
      get { 
        return false; 
      }
    }

    public IEnumerator<T> GetEnumerator() {
      throw new Exception("The method or operation is not implemented.");
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      throw new Exception("The method or operation is not implemented.");
    }


    private Subdivision.ISubdivisionPolicy _subdiv_policy;
    private int _count; // number of elements in tree
    private KdNode<T> _root;
    private IEqualityComparer<T> _equality_comp;
  }
}
