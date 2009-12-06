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
      // If query node equals queried node, throw an exception
      if (node == (InheritedType)this)
        throw new InvalidOperationException("Queried node and query node cannot be the same.");
      InheritedType previous = node;
      foreach(InheritedType n in node.Ancestors) {
        if (n == (InheritedType)this) {
          return this.Left != null && this.Left == previous;
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
