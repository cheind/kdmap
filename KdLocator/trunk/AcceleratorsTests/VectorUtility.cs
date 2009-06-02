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

namespace AcceleratorsTests
{
	
	
	public class VectorUtility
	{
		public static Accelerators.Vector InitR3(float x, float y, float z) {
			Accelerators.Vector v = new Accelerators.Vector(3);
			v[0] = x; v[1] = y; v[2] = z;
			return v;
		}
		
		public static Accelerators.Vector InitR2(float x, float y) {
			Accelerators.Vector v = new Accelerators.Vector(2);
			v[0] = x; v[1] = y;
			return v;
		}
		
		public static Accelerators.Vector ZeroR3 {
			get {
				return InitR3(0.0f, 0.0f, 0.0f);
			}
		}
	}
}
