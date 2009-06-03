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
	}
}