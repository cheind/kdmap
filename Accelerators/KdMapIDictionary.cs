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
  /// K-dimensional dictionary
  /// </summary>
  public partial class KdMap<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IVector {

    public void Add(TKey key, TValue value) {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool ContainsKey(TKey key) {
      throw new Exception("The method or operation is not implemented.");
    }

    public ICollection<TKey> Keys {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public bool Remove(TKey key) {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool TryGetValue(TKey key, out TValue value) {
      throw new Exception("The method or operation is not implemented.");
    }

    public ICollection<TValue> Values {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public TValue this[TKey key] {
      get {
        throw new Exception("The method or operation is not implemented.");
      }
      set {
        throw new Exception("The method or operation is not implemented.");
      }
    }
  }
}
