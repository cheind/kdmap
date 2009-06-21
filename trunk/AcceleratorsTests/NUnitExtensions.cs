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

namespace AcceleratorsTests.NUnitExtensions
{
  
  /// <summary>
  /// Extensions to NUnit not available prior to 2.5
  /// </summary>
  public class Assert
  {
    public delegate void Block();
    
    /// <summary>
    /// Assert that the given block of code throws the presented exception type.
    /// </summary>
    /// <remarks>
    /// NUnit prior to release 2.5 (which is unstable under Mono) does not offer this method.
    /// </remarks>
    public static void Throws(Type exception_type, Block block) {
      try {
        block();
        NUnit.Framework.Assert.Fail(String.Format("Exception of type '{0}' was excepted but nothing was thrown", exception_type.FullName));
      } catch (Exception e) {
        if (e.GetType() != exception_type) {
          NUnit.Framework.Assert.Fail(String.Format("Exception of type '{0}' was excepted but exception of type '{1}' was thrown", exception_type.FullName, e.GetType().FullName));
        }
      }
    }
  }
}
