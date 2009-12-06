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
  /// Chooses a split dimension based on the depth of the node. At each depth level, d, 
  /// the d % dimensions axis is chosen. If the choses axis has no extension a single loop over
  /// all dimensions starting at the chosen one determines the next axis that has a positive extension.
  /// If no such axis can be found the dataset is degenerate.
  /// </summary>
  public class PeriodicAxisSelector : ISplitDimensionSelector
  {
    /// <summary>
    /// Select axis by cycling through dimensions at each depth level of tree
    /// </summary>
    public int Select<T> (KdNode<T> target) where T : IVector {
        
      IVector diagonal = target.InternalBounds.Diagonal;
      int dims = target.InternalBounds.Dimensions;
      
      // Setup initial axis
      // On root level this is zero on each other level it the depth of the node module its dimensions
      int axis = target.Depth % dims;
      double spread = diagonal[axis];
      int i = 0;
      while (FloatComparison.CloseZero(spread, FloatComparison.DefaultEps) && i < dims) {
        axis = (axis + 1) % dims;
        spread = diagonal[axis];
        i += 1;
      }
      
      if (i == dims) {
        // Cycle completed without finding an axis that has a spread greater than zero
        throw new DegenerateDatasetException();
      }
      
      return axis;
    }
  }
}
