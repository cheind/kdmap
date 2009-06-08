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
using Cairo;
using Accelerators;
using System.Collections.Generic;

namespace RenderTree
{
	class MainClass
	{
		
		public static void Main(string[] args)
		{
      CSVReader r = new CSVReader(' ');
      ICollection<IVector> vecs = r.Parse(@"etc/testdata/wrenches.csv");
			KdTree<IVector> tree = new KdTree<IVector>(vecs, new MedianSubdivisionPolicy(25));

			RenderToImage rc = new RenderToImage();
			rc.FirstDimension = 0;
			rc.SecondDimension = 1;
			rc.ImageSize = new Vector(500f, 500.0f); // Maintain aspect ratio of pointcloud
			rc.Render(tree.Root, "kdtree.svg");	
		}
	}
}