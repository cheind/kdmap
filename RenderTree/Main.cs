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
        Console.Write(message + " ..");
      }

      public static void Immediate(string message) {
        Console.WriteLine(message + " .. done (took: 0.0s)");
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

      IList<IVector> vecs = null;
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
        Rendering.RenderTreeCairo render = new Rendering.RenderTreeCairo();
        render.Render(tree.Root, 
                    props.ProjectionDimensions,
                    props.OutFile, 
                    props.Width, props.Height, 
                    props.LineWidth, props.PointSize);

      }
		}
	}
}