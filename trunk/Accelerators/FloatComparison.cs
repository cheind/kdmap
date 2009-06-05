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
  /// Helpers in floating point comparison
  /// </summary>
  public class FloatComparison
  {
    private static float default_eps = 1e-5f;
    
    /// <value>
    /// Default epsilon interval. 
    /// </value>
    public static float DefaultEps {
      get {
        return default_eps;
      }
    }
    
    /// <summary>
    /// Test if a is close to b given a symmetric interval of eps.
    /// </summary>
    /// <param name="a">
    public static bool Close(float a, float b, float eps) {
      return Math.Abs(a-b) <= eps;
    }
    
    /// <summary>
    /// Test if a is close to zero given the symmetric interval of eps.
    /// </summary>
    public static bool CloseZero(float a, float eps) {
      return Close(a, 0.0f, eps);
    }
  }
}
