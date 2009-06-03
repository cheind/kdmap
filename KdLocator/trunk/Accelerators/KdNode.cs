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

	public class KdNode<T> : BinaryNode<KdNode<T>> where T : IVector
	{
		/// <value>
		/// Access the split dimension.
		/// </value>
		public int SplitDimensions {
			get {
				return _k;
			}
			set {
				_k = value;
			}
		}
		
		/// <value>
		/// Access the location of the split. 
		/// </value>
		public float SplitLocation {
			get {
				return _p;
			}
			set {
				_p = value;
			}
		}
		
		/// <value>
		/// Axis aligned bounding box of this node.
		/// </value>
		public AABB Bounds {
			get {
				return _aabb;
			}
			set {
				_aabb = value;
			}
		}
		
		/// <value>
		/// Vectors in within the bounds of this node.
		/// </value>
		public IList<T> Vectors {
			get {
				return _vectors;
			}
			set {
				_vectors = value;
			}
		}
		
		private int _k;
		private float _p;
		private AABB _aabb;
		private IList<T> _vectors;
	}
}