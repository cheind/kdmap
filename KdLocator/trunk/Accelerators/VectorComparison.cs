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
	/// Helpers for comparing two vectors
	/// </summary>
	public class VectorComparison
	{
		/// <summary>
		/// Test if two vectors are component-wise equal.
		/// </summary>
		public static bool Equal(IVector a, IVector b) {
			for (int i = 0; i < a.Dimensions; ++i) {
				if (a[i] != b[i])
					return false;
			}
			return true;
		}
		
		/// <summary>
		/// Test if two vectors are component-wise close using a symmetric tolerance interval.
		/// </summary>
		public static bool Close(IVector a, IVector b, float eps) {
			for (int i = 0; i < a.Dimensions; ++i) {
				if (!FloatComparison.Close(a[i], b[i], eps))
					return false;
			}
			return true;
		}
	}
}
