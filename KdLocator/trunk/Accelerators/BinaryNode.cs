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
	/// Single node in a binary tree. Implemented using the CRTP pattern (http://en.wikipedia.org/wiki/Curiously_Recurring_Template_Pattern)
	/// </summary>
	public class BinaryNode<InheritedType> where InheritedType : BinaryNode<InheritedType>
	{
		
		public BinaryNode()
		{
			_left = null;
			_right = null;
		}
		
		/// <value>
		/// Access the left child
		/// </value>
		public InheritedType Left {
			get {
				return _left;
			}
			set {
				_left = value;
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
				_right = value;
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
		/// Test if node is not a leaf 
		/// </value>
		public bool Intermediate {
			get {
				return !this.Leaf;
			}
		}
		
		/// <summary>
		/// Helper method
		/// </summary>
		public InheritedType SetLeftChild(InheritedType n) {
			this.Left = n;
			return n;
		}

		/// <summary>
		/// Helper method
		/// </summary>
		public InheritedType SetRightChild(InheritedType n) {
			this.Right = n;
			return n;
		}
		
		/// <value>
		/// Pre-order iteration 
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
		
		private InheritedType _left;
		private InheritedType _right;
	}
	
}
