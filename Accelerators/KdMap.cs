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

    /// <summary>
    /// Construct default kd-map with n-dimensions.
    /// </summary>
    public KdMap(int dimensions) {
      _kdtree = new KdTree<LocatablePair<TKey, TValue>>(dimensions, new Subdivision.SubdivisionPolicyConnector());
    }
    
    /// <summary>
    /// Access the underlying kd-tree.
    /// </summary>
    public KdTree<LocatablePair<TKey, TValue>> KdTree {
      get { return _kdtree; }
    }

    private KdTree<LocatablePair<TKey, TValue>> _kdtree;
  }
}
