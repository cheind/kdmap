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
		///    			a
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
		public void TestLeafVsIntermediateState()
		{
			CharNode root = ExampleTree;
			Assert.IsTrue(root.Intermediate);
			Assert.IsFalse(root.Leaf);
			Assert.AreEqual("dfh", MakeStringFromIteration(root.Leafs));
		}
		
		[Test]
		public void TestLeafsTraversal()
		{
			Assert.AreEqual("dfh", MakeStringFromIteration(ExampleTree.Leafs));
		}
		

	}
}
