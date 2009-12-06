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
      NUnitExtensions.Assert.Throws(typeof(InvalidOperationException),
        delegate { leaf_parents[1].ContainsInRightSubTree(leaves[0]); });

      // When the query node is the same node as the node the query is executed on
      // an exception is thrown
      NUnitExtensions.Assert.Throws(typeof(InvalidOperationException),
        delegate { leaf_parents[1].ContainsInRightSubTree(leaf_parents[1]); });
      NUnitExtensions.Assert.Throws(typeof(InvalidOperationException),
        delegate { leaf_parents[1].ContainsInLeftSubTree(leaf_parents[1]); });
    }

    
    

  }
}
