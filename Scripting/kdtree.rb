=begin
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
=end

# Requires IronRuby (http://www.ironruby.net) to run.

# Make sure we can reference the release assemblies
$LOAD_PATH << "../Accelerators/bin/Release"
$LOAD_PATH << "../Rendering/bin/Release"

require 'Accelerators'
require 'Rendering'

# Create a new policy: default policy with bucket size of 1.
p = Accelerators::Subdivision::SubdivisionPolicyConnector.new(1)
# Create a new kd-tree in two-dimensional space
t = Accelerators::KdTree[Accelerators::IVector].new(2, p)

# Add some elements
t.Add(Accelerators::Vector.Create(2.0, 2.0));
t.Add(Accelerators::Vector.Create(4.0, 4.0));

# Render the tree
r = Rendering::RenderTreeCairo.new
r.Render(
  t.Root, # Starting at root node
  Accelerators::Pair[System::Int32,System::Int32].new(0,1),  # Projecting onto xy-plane
  "kdtree.pdf", # Output filename 
  100, 100, # Size
  0.2, 1 # Linewidth and pointsize
)