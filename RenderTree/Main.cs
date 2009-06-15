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
using Accelerators.Subdivision;
using System.Collections.Generic;
using CommandLine.OptParse;
using System.ComponentModel;
using System.Reflection;
using System.IO;

namespace RenderTree
{
	class MainClass
	{	


		public static void Main(string[] args)
		{
      OptProperties props = new OptProperties();
      Parser p = ParserFactory.BuildParser(props);
      p.Parse(args);

      if (props.Help) {
        UsageBuilder usage = new UsageBuilder();
        usage.GroupOptionsByCategory = true;
        usage.BeginSection("Name");
        usage.AddParagraph("RenderTree.exe - Render the content of a kdtree");
        usage.EndSection();

        usage.BeginSection("Arguments");
        usage.AddOptions(p.GetOptions());
        usage.EndSection();
        usage.ToText(System.Console.Out, OptStyle.Unix, true);
      } else {
        FileInfo fi = new FileInfo(props.CSVFile);
        if (!fi.Exists)
          throw new ArgumentException("Provided csv file does not exist");

        CSVReader r = new CSVReader(' ');
        ICollection<IVector> vecs = r.Parse(props.CSVFile);
        ISubdivisionPolicy policy = 
          new SubdivisionPolicyConnector(props.BucketSize, 
                                         props.ISplitDimensionSelectorInstance, 
                                         props.ISplitLocationSelectorInstance, 
                                         props.ITrivialSplitResolverInstance);

        KdTree<IVector> tree = new KdTree<IVector>(vecs, policy);
        RenderTreeCairo render = new RenderTreeCairo();
        render.Render(tree.Root, 
                      new Pair<int, int>(0, 1), 
                      props.OutFile, 
                      props.Width, props.Height, 
                      props.LineWidth, props.PointSize);
      }
		}
	}
}