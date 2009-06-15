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
    /// <summary>
    /// Display progress and elapsed times on console
    /// </summary>
    class Progress : IDisposable {

      public Progress(string message) {
        _start = DateTime.Now;
        Console.Write(message + " ...");
      }

      public static void Immediate(string message) {
        Console.WriteLine(message + " ... done (took: 0.0s)");
      }
      
      public void Dispose() {
        TimeSpan s = DateTime.Now - _start;
        Console.WriteLine(string.Format(" done (took: {0}s)", s.TotalSeconds));
      }

      private DateTime _start;
    }

    public static void PrintUsageAndExit(Parser p) {
      UsageBuilder usage = new UsageBuilder();
      usage.GroupOptionsByCategory = true;
      usage.BeginSection("Name");
      usage.AddParagraph("RenderTree.exe - Render the content of a kdtree");
      usage.EndSection();

      usage.BeginSection("Arguments");
      usage.AddOptions(p.GetOptions());
      usage.EndSection();

      usage.ToText(System.Console.Out, OptStyle.Unix, true);
      System.Environment.Exit(1);
    }

		public static void Main(string[] args)
		{
      OptProperties props = new OptProperties();
      Parser parser = ParserFactory.BuildParser(props);
      parser.Parse(args);

      if (props.Help || props.CSVFile == null || props.OutFile == null || !(new FileInfo(props.CSVFile).Exists)) {
        PrintUsageAndExit(parser);
      }

      Progress.Immediate(string.Format("Setting projection dimensions to {0}", props.ProjectionDimensions));
      Progress.Immediate(string.Format("Setting drawing width to {0}", props.Width));
      Progress.Immediate(string.Format("Setting drawing height to {0}", props.Width));
      Progress.Immediate(string.Format("Setting drawing line width to {0}", props.LineWidth));
      Progress.Immediate(string.Format("Setting drawing point size to {0}", props.PointSize));
      Progress.Immediate(string.Format("Setting bucket size to {0}", props.BucketSize));
      Progress.Immediate(string.Format("Setting ISplitDimensionSelector to '{0}'", props.ISplitDimensionSelector));
      Progress.Immediate(string.Format("Setting ISplitLocationSelector to '{0}'", props.ISplitLocationSelector));
      Progress.Immediate(string.Format("Setting ITrivialSplitResolver to '{0}'", props.ITrivialSplitResolver));

      ICollection<IVector> vecs = null;
      using (Progress p = new Progress(string.Format("Reading data from '{0}'", props.CSVFile))) {
        CSVReader r = new CSVReader(' ');
        vecs = r.Parse(props.CSVFile);
      }

      ISubdivisionPolicy policy = 
        new SubdivisionPolicyConnector(props.BucketSize, 
                                       props.ISplitDimensionSelectorInstance, 
                                       props.ISplitLocationSelectorInstance, 
                                       props.ITrivialSplitResolverInstance);

      KdTree<IVector> tree = null;
      using (Progress p = new Progress("Constructing kd-tree")) {
        tree = new KdTree<IVector>(vecs, policy);
      }
      
      using (Progress p = new Progress(string.Format("Rendering kd-tree to '{0}'", props.OutFile))) {
        RenderTreeCairo render = new RenderTreeCairo();
        render.Render(tree.Root, 
                    props.ProjectionDimensions,
                    props.OutFile, 
                    props.Width, props.Height, 
                    props.LineWidth, props.PointSize);

      }
		}
	}
}