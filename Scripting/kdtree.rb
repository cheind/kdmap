=begin
  Copyright (C) 2009 Christoph Heindl
 
  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.
 
  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.
 
  You should have received a copy of the GNU General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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