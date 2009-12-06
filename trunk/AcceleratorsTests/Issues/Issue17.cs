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
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Accelerators;
using Accelerators.Subdivision;

namespace AcceleratorsTests.Issues
{
  /// <summary>
  /// Issue #17: calling KdTree<T>.Clear will render all previously created searches invalid.
  /// http://code.google.com/p/kdmap/issues/detail?id=17
  /// </summary>
  [TestFixture()]
	public class Issue17
	{
    [Test]
    public void TestCallingClearAndSearch() {
      // Problem is that Clear destroys the root node.
      KdTree<Vector> tree = new KdTree<Vector>(2, new Accelerators.Subdivision.SubdivisionPolicyConnector(1));
      tree.Add(Vector.Create(1.0, 2.0));
      tree.Add(Vector.Create(1.0, 3.0));
      tree.Add(Vector.Create(1.0, 4.0));

      Accelerators.Searches.ExactSearch<Vector> es = new Accelerators.Searches.ExactSearch<Vector>(tree.Root);
      tree.Clear();

      Assert.IsTrue(Numbered.Empty(es.FindExact(Vector.Create(1.0, 3.0))));
      Assert.AreEqual(0, tree.Root.Vectors.Count);
      Assert.IsTrue(tree.Root.InternalBounds.Empty);
    }
	}
}
