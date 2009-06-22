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
using System.Collections.Generic;
using System.Text;
using CommandLine;
using CommandLine.OptParse;
using System.Reflection;
using System.ComponentModel;


namespace RenderTree {

  /// <summary>
  /// Option class that handles valid arguments for command line parsing using
  /// http://csharpoptparse.sourceforge.net/
  /// </summary>
  class OptProperties {

    public OptProperties() {
      _bucket_size = 25;
      _width = 500.0;
      _height = 500.0;
      _point_size = 1.0;
      _line_width = 1.0;
      _accelerators_assembly = Assembly.LoadFile(Environment.CurrentDirectory + @"\Accelerators.dll");
      _split_dim_selector = new Accelerators.Subdivision.PeriodicAxisSelector();
      _split_loc_selector = new Accelerators.Subdivision.MidpointSelector();
      _split_trivial_resolver = new Accelerators.Subdivision.NoOperationResolver();
      _help = false;
      _out_file = null;
      _csv_file = null;
      _projection_dimensions = new Accelerators.Pair<int, int>(0, 1);
    }

    private int _bucket_size;
    private string _csv_file, _out_file;
    private double _width, _height;
    private double _line_width, _point_size;
    private Accelerators.Subdivision.ISplitDimensionSelector _split_dim_selector;
    private Accelerators.Subdivision.ISplitLocationSelector _split_loc_selector;
    private Accelerators.Subdivision.ITrivialSplitResolver _split_trivial_resolver;
    private Assembly _accelerators_assembly;
    private bool _help;
    private Accelerators.Pair<int, int> _projection_dimensions;

    public Accelerators.Pair<int, int> ProjectionDimensions {
      get { return _projection_dimensions; }
      set { _projection_dimensions = value; }
    }

    [OptDef(OptValType.ValueReq)]
    [LongOptionName("proj-dims")]
    [Description("Specify the projection dimensions.")]
    [DefaultValue("0,1")]
    public string ProjectionDimensionsString {
      get {
        return _projection_dimensions.ToString();
      }
      set {
        string[] values = value.Split(new char[]{','});
        if (values.Length != 2)
          throw new ParseException("Cannot parse projection dimensions");

        _projection_dimensions.First = Int32.Parse(values[0]);
        _projection_dimensions.Second = Int32.Parse(values[1]);
      }
    }

    [OptDef(OptValType.Flag)]
    [LongOptionName("help")]
    [Description("Print this help.")]
    [DefaultValue(false)]
    public bool Help {
      get { return _help; }
      set { _help = value; }
    }

    [OptDef(OptValType.ValueReq)]
    [LongOptionName("point-size")]
    [Description("Size of points")]
    [DefaultValue(1.0)]
    public double PointSize {
      get { return _point_size; }
      set { _point_size = value; }
    }

    [OptDef(OptValType.ValueReq)]
    [LongOptionName("line-width")]
    [Description("Width of lines")]
    [DefaultValue(1.0)]
    public double LineWidth {
      get { return _line_width; }
      set { _line_width = value; }
    }

    [OptDef(OptValType.ValueReq)]
    [LongOptionName("height")]
    [Description("Height of drawing")]
    [DefaultValue(500.0)]
    public double Height {
      get { return _height; }
      set { _height = value; }
    }

    [OptDef(OptValType.ValueReq)]
    [LongOptionName("width")]
    [Description("Width of drawing")]
    [DefaultValue(500.0)]
    public double Width {
      get { return _width; }
      set { _width = value; }
    }

    [OptDef(OptValType.ValueReq)]
    [LongOptionName("csv-file")]
    [Description("Pointcloud input")]
    public string CSVFile {
      get { return _csv_file; }
      set { _csv_file = value; }
    }

    [OptDef(OptValType.ValueReq)]
    [LongOptionName("out-file")]
    [Description("Output drawing. Valid extension .pdf, .svg, .ps, .png")]
    public string OutFile {
      get { return _out_file; }
      set { _out_file = value; }
    }

    [OptDef(OptValType.ValueReq)]
    [LongOptionName("bucket-size")]
    [Description("Bucket size")]
    [DefaultValue(25)]
    public int BucketSize {
      get { return _bucket_size; }
      set { _bucket_size = value; }
    }

    [OptDef(OptValType.ValueReq)]
    [LongOptionName("dim-selector")]
    [Description("The ISplitDimensionSelector to be used in kd-tree construction")]
    public string ISplitDimensionSelector {
      get {
        return _split_dim_selector.ToString();
      }
      set {
        _split_dim_selector = _accelerators_assembly.CreateInstance(value) as Accelerators.Subdivision.ISplitDimensionSelector;
      }
    }

    [OptDef(OptValType.ValueReq)]
    [LongOptionName("loc-selector")]
    [Description("The ISplitLocationSelector to be used in kd-tree construction")]
    public string ISplitLocationSelector {
      get {
        return _split_loc_selector.ToString();
      }
      set {
        _split_loc_selector = _accelerators_assembly.CreateInstance(value) as Accelerators.Subdivision.ISplitLocationSelector;
      }
    }

    [OptDef(OptValType.ValueReq)]
    [LongOptionName("triv-resolver")]
    [Description("The ITrivialSplitResolver to be used in kd-tree construction")]
    public string ITrivialSplitResolver {
      get {
        return _split_trivial_resolver.ToString();
      }
      set {
        _split_trivial_resolver = _accelerators_assembly.CreateInstance(value) as Accelerators.Subdivision.ITrivialSplitResolver;
      }
    }

    public Accelerators.Subdivision.ISplitDimensionSelector ISplitDimensionSelectorInstance {
      get {
        return _split_dim_selector;
      }
    }

    public Accelerators.Subdivision.ISplitLocationSelector ISplitLocationSelectorInstance {
      get {
        return _split_loc_selector;
      }
    }

    public Accelerators.Subdivision.ITrivialSplitResolver ITrivialSplitResolverInstance {
      get {
        return _split_trivial_resolver;
      }
    }
  }
}
