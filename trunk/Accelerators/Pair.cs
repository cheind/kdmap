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

namespace Accelerators
{
  /// <summary>
  /// Generic pair structure
  /// </summary>
  public class Pair<T,U>
	{
    /// <summary>
    /// Construct from first and second.
    /// </summary>
    public Pair(T t, U u)
    {
      _t = t;
      _u = u;
    }

    /// <value>
    /// Access first
    /// </value>
    public T First
    {
      get { return _t; }
      set { _t = value; }
    }

    /// <value>
    /// Access second 
    /// </value>
    public U Second
    {
      get { return _u; }
      set { _u = value; }
    }

    /// <summary>
    /// Convert pair to string
    /// </summary>
    public override string ToString() {
      return string.Format("<{0},{1}>", _t, _u);
    }

    /// <summary>
    /// Convert pair to KeyValuePair.
    /// </summary>
    public KeyValuePair<T,U> ToKeyValuePair() {
      return new KeyValuePair<T, U>(_t, _u);
    }

    private T _t;
    private U _u;
	}
}
