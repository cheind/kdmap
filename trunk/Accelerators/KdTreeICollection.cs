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

namespace Accelerators {
  
  public partial class KdTree<T> : ICollection<T> where T : IVector {
    
    /// <summary>
    /// Add an item to the kd-tree.
    /// </summary>
    /// <remarks>
    /// Duplicate IVectors are allowed to be contained.
    /// </remarks>
    public void Add(T item) {
      KdNode<T> leaf = _cls.FindClosestLeaf(item);
      bool need_stretching = !leaf.InternalBounds.Inside(item);
      leaf.Vectors.Add(item);

      if (need_stretching)
        this.StretchAncestorBounds(leaf, item);
      else
        leaf.InternalBounds.Enlarge(item);

      this.TrySplit(leaf);
      _count += 1;
    }

    /// <summary>
    /// Remove item from kd-tree.
    /// </summary>
    /// <remarks>
    /// Removes the first item with equal coordinates.
    /// </remarks>
    public bool Remove(T item) {
      // Determine leaf
      KdNode<T> leaf = _cls.FindClosestLeaf(item);
      // Find first index in collection with equal coordinates
      int index = leaf.Vectors.FindIndex(delegate(T obj) { return VectorComparison.Equal(item, obj); });
      if (index < 0) { // Item not found
        return false;
      }
      leaf.Vectors.RemoveAt(index);
      if (leaf.Vectors.Count > 0) {
        // Still items to process, test if removal of item changed internal bounds
        AABB aabb = new AABB(leaf.InternalBounds.Dimensions);
        aabb.Enlarge<T>(leaf.Vectors);
        if (!aabb.Equals(leaf.InternalBounds)) {
          // It did change internal bounds; broadcast change up to root
          leaf.InternalBounds = aabb;
          this.ShrinkAncestorBounds(leaf);
        }
      } else {
        // No more items contained
        leaf.InternalBounds.Reset();
        this.ShrinkAncestorBounds(leaf); // Need to shrink here: else space encapsulated by 
        // empty leaf is not freed up to root
        this.TryCollapse(leaf);
      }
      _count -= 1;
      return true;
    }

    /// <summary>
    /// Recalculates the internal bounds of the parents of the given leaf up to root
    /// by joining the bounds of its children.
    /// </summary>
    private void ShrinkAncestorBounds(KdNode<T> leaf) {
      if (!leaf.Root) {
        foreach (KdNode<T> n in leaf.Parent.Ancestors) {
          AABB aabb = n.InternalBounds;
          AABB.Union(n.Left.InternalBounds, n.Right.InternalBounds, ref aabb);
        }
      }
    }

    /// <summary>
    /// Enlarge the internal bounds of all ancestors of the target node including
    /// the target node itself.
    /// </summary>
    private void StretchAncestorBounds(KdNode<T> target, T item) {
      foreach (KdNode<T> n in target.Ancestors) {
        n.InternalBounds.Enlarge(item);
      }
    }

    /// <summary>
    /// Remove all items.
    /// </summary>
    public void Clear() {
      int dimensions = this.Root.InternalBounds.Dimensions;
      // Does setting node references explicitely to null help the garabage collector? 
      foreach(KdNode<T> n in this.Root.PostOrder) {
        if (n.Intermediate) {
          n.UnSetLeftChild();
          n.UnSetRightChild();
        } else {
          n.Vectors = null;
        }
      }
      this.InitializeNode(_root, dimensions);
      _count = 0;
    }

    /// <summary>
    /// Determines whether the kd-tree contains a specific element.
    /// </summary>
    public bool Contains(T item) {
      Searches.ExactSearch<T> es = new Accelerators.Searches.ExactSearch<T>(this.Root);
      es.CountLimit = 1;
      return !Numbered.Empty(es.FindExact(item));
    }

    /// <summary>
    /// Copies the elements of the kd-tree to an array, starting at a particular array index.
    /// </summary>
    public void CopyTo(T[] array, int arrayIndex) {
      // Pass over leaves from left to right and copy each element in turn
      foreach (KdNode<T> n in this.Root.Leaves) {
        n.Vectors.CopyTo(array, arrayIndex);
        arrayIndex += n.Vectors.Count;
      }
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

    /// <summary>
    /// Retrieve a new element iterator 
    /// </summary>
    public IEnumerator<T> GetEnumerator() {
      return new KdTreeEnumerator<T>(this);
    }

    /// <summary>
    /// Retrieve a new element iterator 
    /// </summary>
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
      return new KdTreeEnumerator<T>(this);
    }
  }
}
