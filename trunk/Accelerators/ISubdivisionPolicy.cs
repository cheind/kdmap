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
  /// Exception thrown when splitting of node is not possible.
  /// </summary>
  public class SplitException : ArgumentException {}
  
  /// <summary>
  /// Exception thrown when collapsing of node is not possible.
  /// </summary>
  public class CollapseException : ArgumentException {}
  
  /// <summary>
  /// Defines the split/collapse policies for kd-tree nodes
  /// </summary>
  public interface ISubdivisionPolicy
  {
    /// <summary>
    /// Split the given node and return true when successful.
    /// Vectors to the left or on the split-plane are assumed to go into the left child. All others are placed in
    /// the right child.
    /// </summary>
    void Split<T>(KdNode<T> target) where T : IVector;
    
    /// <summary>
    /// Collapse the leaf-children of the given parental node and return true when successful.
    /// </summary>
    void Collapse<T>(KdNode<T> parent) where T : IVector;
  }
}
