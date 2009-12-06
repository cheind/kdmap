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
using NUnit.Framework;
using Accelerators;
using Accelerators.Subdivision;
using System.Collections.Generic;

namespace AcceleratorsTests
{
  
  
  [TestFixture()]
  public class TestAxisOfMaximumSpreadSelector
  {
    
    [Test()]
    [ExpectedException(typeof(DegenerateDatasetException))]
    public void TestAllCoordinatesSame()
    {
      KdNode<IVector> n = new KdNode<IVector>();
      n.Vectors = new List<IVector>(new IVector[]{Vector.Create(1.0f), Vector.Create(1.0f), Vector.Create(1.0f), Vector.Create(1.0f)});
      n.InternalBounds = new AABB(1);
      n.InternalBounds.Enlarge<IVector>(n.Vectors);
      n.InternalBounds = new AABB(n.InternalBounds);
      AxisOfMaximumSpreadSelector s = new AxisOfMaximumSpreadSelector();
      s.Select(n);
    }
    
    [Test]
    public void TestSecondAxisIsAxisOfMaximumSpread() {
      KdNode<IVector> n = new KdNode<IVector>();
      n.Vectors = new List<IVector>(new IVector[]{Vector.Create(0.5, 0.5), Vector.Create(1.0f, -1.0), Vector.Create(1.0f, 2.0), Vector.Create(1.0f, 1.5)});
      n.InternalBounds = new AABB(2);
      n.InternalBounds.Enlarge<IVector>(n.Vectors);
      n.InternalBounds = new AABB(n.InternalBounds);
      AxisOfMaximumSpreadSelector s = new AxisOfMaximumSpreadSelector();
      Assert.AreEqual(1, s.Select(n));
    }

    
    [Test]
    public void TestFirstAxisIsAxisOfMaximumSpread() {
      KdNode<IVector> n = new KdNode<IVector>();
      n.Vectors = new List<IVector>(new IVector[]{Vector.Create(-10.0f, 0.5), Vector.Create(1.0f, -1.0), Vector.Create(1.0f, 2.0), Vector.Create(1.0f, 1.5)});
      n.InternalBounds = new AABB(2);
      n.InternalBounds.Enlarge<IVector>(n.Vectors);
      n.InternalBounds = new AABB(n.InternalBounds);
      AxisOfMaximumSpreadSelector s = new AxisOfMaximumSpreadSelector();
      Assert.AreEqual(0, s.Select(n));
    }
  }
}
