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
  /// A kd-Tree implementation
  /// </summary>
  public partial class KdTree<T> where T : IVector
  {
    
    /// <summary>
    /// Instance a new kd-tree with the given collection of points
    /// </summary>
    public KdTree(IEnumerable<T> vecs, ISubdivisionPolicy policy)
    {
      _subdiv_policy = policy;
      _root = this.CreateRootNode(vecs);
      this.RecursiveSplit(_root);
    }
    
    /// <summary>
    /// Creates the root kd-tree node that contains the entire scene
    /// </summary>
    private KdNode<T> CreateRootNode(IEnumerable<T> vecs) {
      T first;
      if (!this.FirstFromEnumerable(vecs, out first))
        throw new ArgumentOutOfRangeException("Cannot work on empty collection");
      
      _dimensions = first.Dimensions;
      
      KdNode<T> n = new KdNode<T>();
      n.Bounds = new AABB(_dimensions);
      n.Bounds.Enlarge(vecs);
      n.Vectors = new List<T>(vecs);
      return n;
    }
    
    /// <summary>
    /// Find the first element in the Enumerable
    /// </summary>
    private bool FirstFromEnumerable(IEnumerable<T> vecs, out T first) {
      bool non_empty = false;
      using (IEnumerator<T> e = vecs.GetEnumerator()) {
        if (e.MoveNext()) {
          first = e.Current;
          non_empty = true;
        }
      }
      return non_empty;
    }
    
    /// <summary>
    /// Perform splitting as long as possible
    /// </summary>
    private void RecursiveSplit(KdNode<T> target) {
      Stack<KdNode<T>> s = new Stack<KdNode<T>>();
      s.Push(target);
      while (s.Count > 0) {
        KdNode<T> n = s.Pop();
        try {
          _subdiv_policy.Split(n);
          s.Push(n.Left);
          s.Push(n.Right);
          n.Vectors = null; // Vectors are only stored in leaf nodes
        } catch (SplitException) {}
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
    
    
    private ISubdivisionPolicy _subdiv_policy;
    private int _dimensions;
    private KdNode<T> _root;
  }
}
