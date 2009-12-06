// 
//  Copyright (c) 2009, Christoph Heindl
//  All rights reserved.
// 
//  Redistribution and use in source and binary forms, with or without modification, are 
//  permitted provided that the following conditions are met:
// 
//  Redistributions of source code must retain the above copyright notice, this list of 
//  conditions and the following disclaimer. 
//  Redistributions in binary form must reproduce the above copyright notice, this list 
//  of conditions and the following disclaimer in the documentation and/or other materials 
//  provided with the distribution. 
//  Neither the name Christoph Heindl nor the names of its contributors may be used to endorse 
//  or promote products derived from this software without specific prior written permission.
// 
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS 
//  OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY 
//  AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR 
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
//  DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, 
//  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
//  IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT 
//  OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// 
using System;

namespace Accelerators.Subdivision
{
  
  /// <summary>
  /// Exception thrown when splitting/collapsing of node is not possible.
  /// </summary>
  public class SubdivisionException : InvalidOperationException {}
  
  /// <summary>
  /// Defines the split/collapse policies for kd-tree nodes.
  /// </summary>
  /// 
  /// <remarks>
  /// The subdivision policy defines the procedure of splitting and collapsing nodes. In case this is impossible,
  /// an InvalidOperationException is thrown. The splitting part of the subdivisision policy is assumed todo the following:
  /// Determine the best possible splitting plane, put all points lying on or behind the splitting plane into the left child of
  /// the target node and all others to the right child of the target node. The child nodes need to be instantiated by the split 
  /// method.
  /// </remarks>
  public interface ISubdivisionPolicy
  {
    /// <summary>
    /// Split the given node and return true when successful.
    /// Vectors to the left or on the split-plane are assumed to go into the left child. All others are placed in
    /// the right child.
    /// </summary>
    void Split<T>(KdNode<T> target) where T : IVector;


    /// <summary>
    /// Collapse the target leaf node.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="target">Leaf node to collapse</param>
    /// <returns>Next valid ancestor (including target)</returns>
    void Collapse<T>(KdNode<T> target) where T : IVector;
  }
}
