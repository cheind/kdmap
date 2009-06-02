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
	/// N-Dimensional vector implementation
	/// </summary>
	public class Vector : IVector
	{
		/// <summary>
		/// Create a vector that can hold n-dimensions.
		/// </summary>
		public Vector(int dimensions) {
			_coordinates = new float[dimensions];
		}
		
		/// <summary>
		/// Create vector of n-dimensions with equal coordinates
		/// </summary>
		public Vector(int dimensions, float val) {
			_coordinates = new float[dimensions];
			VectorOps.Fill(this, val);
		}
		
		/// <summary>
		/// Create a two-dimensional vector with coordinates explicitly set.
		/// </summary>
		public Vector(float x, float y) {
			_coordinates = new float[2]{x, y};
		}
		
		/// <summary>
		/// Create a three-dimesional vector with coordinates explicitly set.
		/// </summary>
		public Vector(float x, float y, float z) {
			_coordinates = new float[3]{x, y, z};
		}
		
		/// <summary>
		/// Copy construct from the given vector
		/// </summary>
		public Vector(Vector other) {
			_coordinates = (float[])other._coordinates.Clone();
		}
		
		/// <summary>
		/// Copy construct from a vector implementing the IVector interface
		/// </summary>
		public Vector(IVector other) {
			_coordinates = new float[other.Dimensions];
			for (int i = 0; i < Dimensions ; ++i )
				_coordinates[i] = other[i];
		}

		#region IVector implementation
		public int Dimensions {
			get {
				return _coordinates.Length;
			}
		}
		
		public float this[int index] {
			get {
				return _coordinates[index];
			}
			set {
				_coordinates[index] = value;
			}
		}
		#endregion
		
		public static Vector operator+(Vector lhs, IVector rhs) {
			Vector res = new Vector(lhs.Dimensions);
			VectorOps.Add(lhs, rhs, res);
			return res;
		}
		
		public static Vector operator-(Vector lhs, IVector rhs) {
			Vector res = new Vector(lhs.Dimensions);
			VectorOps.Sub(lhs, rhs, res);
			return res;
		}
		
		public static Vector operator*(Vector lhs, float s) {
			Vector res = new Vector(lhs.Dimensions);
			VectorOps.ScalarMul(lhs, s, res);
			return res;
		}
		
		public float Length {
			get {
				return VectorOps.Length(this);
			}
		}
		
		public static Accelerators.Vector InitR3(float x, float y, float z) {
			Vector v = new Accelerators.Vector(3);
			v[0] = x; v[1] = y; v[2] = z;
			return v;
		}
		
		public static Accelerators.Vector InitR2(float x, float y) {
			Vector v = new Accelerators.Vector(2);
			v[0] = x; v[1] = y;
			return v;
		}
		
		
		private float[] _coordinates;
	}
}
