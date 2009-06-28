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
using System.Text;

namespace Accelerators {

  /// <summary>
  /// Represents a key/value pair insertable into a kd-tree
  /// </summary>
  public class LocatablePair<T, U> : Pair<T,U>, IVector where T : IVector {

    /// <summary>
    /// Construct from first and second parameter.
    /// </summary>
    public LocatablePair(T t, U u) : base(t, u) {}
    
    /// <summary>
    /// Construct from key/value pair 
    /// </summary>
    public LocatablePair(KeyValuePair<T,U> pair) : base (pair.Key, pair.Value) {}

    /// <summary>
    /// Access dimensionality of pair.
    /// </summary>
    public int Dimensions {
      // forward to first
      get { return this.First.Dimensions; }
    }

    /// <summary>
    /// Access individual coordinates by index.
    /// </summary>
    public double this[int index] {
      // forward to first
      get {
        return this.First[index];
      }
      set {
        this.First[index] = value;
      }
    }
  }
}
