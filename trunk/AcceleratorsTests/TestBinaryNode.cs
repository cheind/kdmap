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
  public class TestBinaryNode
  {
    /// <summary>
    /// Node that carries a single character
    /// </summary>
    protected class CharNode : BinaryNode<CharNode>
    {
      public CharNode(char ch) {
        _ch = ch;
      }
      
      public char Character {
        get {
          return _ch;
        }
      }
      
      private char _ch;
    }
    
    /// <summary>
    /// Concatenate the characters in the sequence of the given enumeration
    /// </summary>
    protected string MakeStringFromIteration(IEnumerable<CharNode> ie) {
      System.Text.StringBuilder sb = new System.Text.StringBuilder();
      foreach(CharNode cn in ie) {
        sb.Append(cn.Character);
      }
      return sb.ToString();
    }
    
    /// <summary>
    /// Access a copy of the example tree which is
    ///         a
    ///       /  \ 
    ///      b    e  
    ///     /    / \  
    ///    c    f   g
    ///   /          \
    ///  d            h
    /// </summary>
    protected CharNode ExampleTree {
      get {
        CharNode root = new CharNode('a');
        CharNode left_branch = root.SetLeftChild(new CharNode('b'));
        left_branch = left_branch.SetLeftChild(new CharNode('c'));
        left_branch = left_branch.SetLeftChild(new CharNode('d'));
        CharNode right_branch = root.SetRightChild(new CharNode('e'));
        right_branch.SetLeftChild(new CharNode('f'));
        right_branch = right_branch.SetRightChild(new CharNode('g'));
        right_branch.SetRightChild(new CharNode('h'));
        
        return root;
      }
    }
    
    [Test]
    public void TestPreOrderTraversal()
    {
      Assert.AreEqual("abcdefgh", MakeStringFromIteration(ExampleTree.PreOrder));
    }

    [Test]
    public void TestPostOrderTraversal()
    {
      Assert.AreEqual("dcbfhgea", MakeStringFromIteration(ExampleTree.PostOrder));
    }
    
    [Test]
    public void TestLeafVsIntermediateState()
    {
      CharNode root = ExampleTree;
      Assert.IsTrue(root.Intermediate);
      Assert.IsFalse(root.Leaf);
      Assert.AreEqual("dfh", MakeStringFromIteration(root.Leaves));
    }
    
    [Test]
    public void TestLeafsTraversal()
    {
      Assert.AreEqual("dfh", MakeStringFromIteration(ExampleTree.Leaves));
    }
    
    [Test]
    public void TestAncestorTraversal()
    {
      CharNode root = ExampleTree;
      Assert.AreEqual("a", MakeStringFromIteration(root.Ancestors));
      
      List<CharNode> leaves = new List<CharNode>(root.Leaves);
      Assert.AreEqual("dcba", MakeStringFromIteration(leaves[0].Ancestors));
      Assert.AreEqual("fea", MakeStringFromIteration(leaves[1].Ancestors));
      Assert.AreEqual("hgea", MakeStringFromIteration(leaves[2].Ancestors));
    }

    [Test]
    public void TestContainsInLeftRightSubTree() {
      CharNode root = ExampleTree;
      List<CharNode> leaves = new List<CharNode>(root.Leaves);
      List<CharNode> leaf_parents = new List<CharNode>();
      foreach (CharNode n in leaves) {
        leaf_parents.Add(n.Parent);
      }
      Assert.IsTrue(root.ContainsInLeftSubTree(leaves[0])); // 'd'
      Assert.IsFalse(root.ContainsInLeftSubTree(leaves[1])); // 'f'
      Assert.IsFalse(root.ContainsInLeftSubTree(leaves[2])); // 'h'

      Assert.IsTrue(leaf_parents[0].ContainsInLeftSubTree(leaves[0]));
      Assert.IsTrue(leaf_parents[1].ContainsInLeftSubTree(leaves[1]));
      Assert.IsFalse(leaf_parents[1].ContainsInRightSubTree(leaves[1]));
      Assert.IsTrue(leaf_parents[1].ContainsInRightSubTree(leaves[2]));

      // When query node is not part of subtree the method throws an exception
      Assert.Throws(typeof(InvalidOperationException),
        delegate { leaf_parents[1].ContainsInRightSubTree(leaves[0]); });

      // When the query node is the same node as the node the query is executed on
      // an exception is thrown
      Assert.Throws(typeof(InvalidOperationException),
        delegate { leaf_parents[1].ContainsInRightSubTree(leaf_parents[1]); });
      Assert.Throws(typeof(InvalidOperationException),
        delegate { leaf_parents[1].ContainsInLeftSubTree(leaf_parents[1]); });
    }

    
    

  }
}
