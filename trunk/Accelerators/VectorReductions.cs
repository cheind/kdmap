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

namespace Accelerators
{
	
	
	/// <summary>
	/// Reduction on vectors
	/// </summary>
	public class VectorReductions
	{
  	/// <summary>
		/// Calculate the square-length (L2 norm) of the given vector.
		/// </summary>
		public static float SquaredL2Norm(IVector a) {
			return VectorOperations.Inner(a, a);
		}
		
		/// <summary>
		/// Calculate the length of the vector
		/// </summary>
		public static float L2Norm(IVector a) {
			return (float)Math.Sqrt(SquaredL2Norm(a));
		}
		
		/// <summary>
		/// Calculate the index of the element having the maximum absolut value.
		/// Assumes at least a one-dimensional vector.
		/// </summary>
		public static int IndexNormInf(IVector a) {
			float max_val = Math.Abs(a[0]);
			int max_index = 0;
			
			for (int i = 1; i < a.Dimensions; ++i) {
				float val = Math.Abs(a[i]);
				if (val > max_val) {
					max_val = val;
					max_index = i;
				}
			}
			return max_index;
		}
	}
}