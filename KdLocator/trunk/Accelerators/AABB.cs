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
	/// Axis-aligned bounding box
	/// </summary>
	public class AABB
	{
		
		/// <summary>
		/// Create an empty AABB in n-dimensions (min > max).
		/// </summary>
		public AABB(int dimensions)
		{
			_min = new Vector(dimensions, Single.MaxValue);
			_max = new Vector(dimensions, -Single.MaxValue);
		}
		
		/// <summary>
		/// Enlarge AABB to contain the given vectors.
		/// </summary>
		public void Enlarge<T>(ICollection<T> values) where T : IVector {
			foreach(IVector v in values) {
				for(int i = 0; i < _min.Dimensions; ++i) {
					float vi = v[i];
					if (vi < _min[i]) _min[i] = vi;
					if (vi > _max[i]) _max[i] = vi;
				}
			}
		}
		
		/// <value>
		/// Test if AABB is empty. 
		/// </value>
		public bool Empty {
			get {
				return false;
			}
		}
		
		/// <summary>
		/// Reset to empty state
		/// </summary>
		public void Reset() {
		}
		
		
		
		
		private Vector _min;
		private Vector _max;
	}
}
