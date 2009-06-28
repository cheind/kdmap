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

  public partial class KdMap<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IVector {

    public void Add(KeyValuePair<TKey, TValue> item) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void Clear() {
      throw new Exception("The method or operation is not implemented.");
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) {
      throw new Exception("The method or operation is not implemented.");
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
      throw new Exception("The method or operation is not implemented.");
    }

    public int Count {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public bool IsReadOnly {
      get { throw new Exception("The method or operation is not implemented."); }
    }

    public bool Remove(KeyValuePair<TKey, TValue> item) {
      throw new Exception("The method or operation is not implemented.");
    }
  }
}
