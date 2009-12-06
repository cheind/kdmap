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
  public class TestSlidingPlaneResolver
  {
    
    [Test()]
    public void TestEmptySplitsRegular()
    {
      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[] { Vector.Create(-1.0), Vector.Create(1.0), Vector.Create(3.0), Vector.Create(2.0) });
     
      SlidingPlaneResolver spr = new SlidingPlaneResolver();
      Assert.AreEqual(2.0, spr.Resolve(n, 0, 3.0, ETrivialSplitType.EmptyRight), FloatComparison.DefaultEps);
      Assert.AreEqual(2.0, spr.Resolve(n, 0, 3.5, ETrivialSplitType.EmptyRight), FloatComparison.DefaultEps);
      Assert.AreEqual(-1.0, spr.Resolve(n, 0, -1.1, ETrivialSplitType.EmptyLeft), FloatComparison.DefaultEps);
      Assert.AreEqual(-1.0, spr.Resolve(n, 0, -15.0, ETrivialSplitType.EmptyLeft), FloatComparison.DefaultEps);
    }

    [Test()]
    public void TestEmptySplitsRegularSmall()
    {
      KdNode<Vector> n = new KdNode<Vector>();
      n.Vectors = new List<Vector>(new Vector[] { Vector.Create(-1.0), Vector.Create(1.0)});
     
      SlidingPlaneResolver spr = new SlidingPlaneResolver();
      Assert.AreEqual(-1.0, spr.Resolve(n, 0, 1.0, ETrivialSplitType.EmptyRight), FloatComparison.DefaultEps);
      Assert.AreEqual(-1.0, spr.Resolve(n, 0, 3.5, ETrivialSplitType.EmptyRight), FloatComparison.DefaultEps);
      Assert.AreEqual(-1.0, spr.Resolve(n, 0, -1.1, ETrivialSplitType.EmptyLeft), FloatComparison.DefaultEps);
      Assert.AreEqual(-1.0, spr.Resolve(n, 0, -15.0, ETrivialSplitType.EmptyLeft), FloatComparison.DefaultEps);
    }
  
  }
}
