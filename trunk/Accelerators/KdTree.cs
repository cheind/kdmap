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
