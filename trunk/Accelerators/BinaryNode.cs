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
  /// Represents a binary tree.
  /// </summary>
  /// 
  /// <remarks>
  /// Implemented using the CRTP pattern (http://en.wikipedia.org/wiki/Curiously_Recurring_Template_Pattern)
  /// </remarks>
  public class BinaryNode<InheritedType> where InheritedType : BinaryNode<InheritedType>
  {
    
    public BinaryNode()
    {
      _left = null;
      _right = null;
      _parent = null;
    }
    
    /// <value>
    /// Access the left child
    /// </value>
    public InheritedType Left {
      get {
        return _left;
      }
      set {
        if (_left != null)
          _left.Parent = null;
        _left = value;
        if (_left != null)
        _left.Parent = (InheritedType)this;
      }
    }
    
    /// <value>
    /// Access the right child
    /// </value>
    public InheritedType Right {
      get {
        return _right;
      }
      set {
        if (_right != null)
          _right.Parent = null;
        _right = value;
        if (_right != null)
        _right.Parent = (InheritedType)this;
      }
    }
    
    /// <value>
    /// Access the parent
    /// </value>
    public InheritedType Parent {
      get {
        return _parent;
      }
      set {
        _parent = value;
      }
    }
    
    /// <value>
    /// Test if node is leaf 
    /// </value>
    public bool Leaf {
      get {
        return _left == null && _right == null;
      }
    }
    
    /// <value>
    /// Test if node is root 
    /// </value>
    public bool Root {
      get {
        return _parent == null;
      }
    }
    
    /// <value>
    /// Test if node is not a leaf 
    /// </value>
    public bool Intermediate {
      get {
        return !this.Leaf;
      }
    }
    
    /// <value>
    /// Access the depth of the node starting with zero at the root level 
    /// </value>
    public int Depth {
      get {
        int count = 0;
        InheritedType n = (InheritedType)this;
        while (!n.Root) {
          count += 1;
          n = n.Parent;
        }
        return count;
      }
    }
    
    /// <summary>
    /// Utility method taking a node and setting it as the left child
    /// and returning the node.
    /// </summary>
    public InheritedType SetLeftChild(InheritedType n) {
      this.Left = n;
      return n;
    }

    /// <summary>
    /// Utility method taking a node and setting it as the right child
    /// and returning the node.
    /// </summary>
    public InheritedType SetRightChild(InheritedType n) {
      this.Right = n;
      return n;
    }
    
    /// <summary>
    /// Utility method unlinking the left child and returning the node.
    /// </summary>
    public InheritedType UnSetLeftChild() {
      InheritedType n = this.Left;
      this.Left = null;
      return n;
    }

    /// <summary>
    /// Utility method unlinking the left child and returning the node.
    /// </summary>
    public InheritedType UnSetRightChild() {
      InheritedType n = this.Right;
      this.Right = null;
      return n;
    }

    /// <summary>
    /// Test if given node is in left subtree of the current node.
    /// </summary>
    public bool ContainsInLeftSubTree(InheritedType node) {
      if (node == this)
        throw new InvalidOperationException("Queried node and query node cannot be the same.");
      InheritedType previous = node;
      foreach(InheritedType n in node.Ancestors) {
        if (n == this) {
          return this.Left == previous;
        }
        previous = n;
      }
      // Walked up to root but did not encounter self.
      throw new InvalidOperationException(String.Format("Node {0} is not an ancestor of Node {1}", this, node));
    }

    /// <summary>
    /// Test if given node is in left subtree of the current node.
    /// </summary>s
    public bool ContainsInRightSubTree(InheritedType node) {
      return !this.ContainsInLeftSubTree(node);
    }
    
    /// <value>
    /// Pre-order iteration.
    /// </value>
    public IEnumerable<InheritedType> PreOrder {
      get {
        Stack<InheritedType> s = new Stack<InheritedType>();
        s.Push(this as InheritedType);
        while (s.Count > 0) {
          InheritedType n = s.Pop();
          yield return n;
          if (n.Right != null)
            s.Push(n.Right);
          if (n.Left != null)
            s.Push(n.Left);
        }
      }
    }

    /// <value>
    /// Post-order iteration.
    /// </value>
    public IEnumerable<InheritedType> PostOrder
    {
      get
      {
        // Post order iteration using a stack makes use of a marked attribute at nodes.
        // Initially all intermediate nodes are unmarked. During traversal intermediate nodes
        // encountered are marked and re-thrown onto the stack before their children are pushed
        // onto the stack. When a marked node or a leaf is encountered the node is yield.
        Stack<Pair<InheritedType, bool>> s = new Stack<Pair<InheritedType, bool>>();
        s.Push(new Pair<InheritedType, bool>(this as InheritedType, false));
        while (s.Count > 0)
        {
          // Note: cannot use s.Peek() as Pair is a structure that is returned by value.
          Pair<InheritedType, bool> p = s.Pop();
          if (p.First.Leaf || p.Second) {
            yield return p.First;
          } else {
            s.Push(new Pair<InheritedType, bool>(p.First, true));
            if (p.First.Right != null)
              s.Push(new Pair<InheritedType, bool>(p.First.Right, false));
            if (p.First.Left != null)
              s.Push(new Pair<InheritedType, bool>(p.First.Left, false));
          }
        }
      }
    }
    
    /// <value>
    /// Leaf-iteration from left to right.
    /// </value>
    public IEnumerable<InheritedType> Leaves {
      get {
        // Non-optimized version as all nodes are traversed and filtered accordingly.
        // The data-structure used in BinaryNode is not capable of an optimized iteration.
        foreach (InheritedType n in this.PreOrder) {
          if (n.Leaf)
            yield return n;
        }
      }
    }
    
    /// <value>
    /// Ancestor-iteration including the current node.
    /// </value>
    public IEnumerable<InheritedType> Ancestors {
      get {
        InheritedType n = (InheritedType)this;
        do {
          yield return n;
          n = n.Parent;
        } while (n != null);
      }
    }
    
    private InheritedType _left;
    private InheritedType _right;
    private InheritedType _parent;
  }
  
}