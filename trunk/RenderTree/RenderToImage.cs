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
using Accelerators;

namespace RenderTree
{
	
	
	/// <summary>
	/// Render Kd-Tree to an image
	/// </summary>
	public class RenderToImage
	{
		public RenderToImage() {
			_point_color = new Cairo.Color(0.0, 0.4, 0.0);
			_line_color = new Cairo.Color(0.0, 0.0, 0.0);
			_dim0 = 0;
			_dim1 = 1;
			this.ImageSize =  new Vector(500.0f, 500.0f);
				
		}
		
		public IVector ImageSize {
			set {
				const double point_size_ratio = 1.5 / 500.0;
				const double line_width_ratio = 0.8 / 500.0;
				_image_size = new Vector(value);
				if (_image_size[0] > _image_size[1]) {
					_line_width = _image_size[0] * line_width_ratio;
					_point_size = _image_size[0] * point_size_ratio;
				} else {
					_line_width = _image_size[1] * line_width_ratio;
					_point_size = _image_size[1] * point_size_ratio;
				}
			}
		}
		
		public int FirstDimension {
			set {
				_dim0 = value;
			}
		}
		
		public int SecondDimension {
			set {
				_dim1 = value;
			}
		}
	 
		
		public void Render<T>(KdNode<T> tree, string filename) where T : IVector {
			// Drawing is done only to 90 percent of surface area
			double width = _image_size[0];
			double height = _image_size[1];
			double inner_x = width * 0.9;
			double inner_y = height * 0.9;
			
			AABB world = tree.Bounds;
			IVector diag = world.Diagonal;
			IVector center = world.Center;
			
			// Setup matrix that transforms world to image coordinates
			_world_to_image = new Cairo.Matrix(1, 0, 0, -1, 0, 0);
			_world_to_image.Translate(width*0.5f, -height*0.5f);
			_world_to_image.Scale(inner_x / diag[_dim0], inner_y / diag[_dim1]);
			_world_to_image.Translate(-center[_dim0], -center[_dim1]);

      using (Cairo.PdfSurface draw = new Cairo.PdfSurface(filename, width, height))
      {
				using (Cairo.Context gr = new Cairo.Context(draw)) {
					gr.LineWidth = _line_width;
					gr.Antialias = Cairo.Antialias.Default;
					
					// Background
					gr.Rectangle(0, 0, width, height);
					gr.Color = new Cairo.Color(1.0, 1.0, 1.0);
					gr.Fill();
					gr.Stroke();
					
					// World bounds
					this.RenderRectangle(world, gr);
					
					// Iterate
					this.RenderLeaves(tree, gr);
          this.RenderIntermediates(tree, gr);

          gr.ShowPage();
				}
			}
		}
		
		private void RenderRectangle(AABB rectangle, Cairo.Context gr) {
			IVector diag = rectangle.Diagonal;
			double lx = rectangle.Lower[_dim0]; double ly = rectangle.Lower[_dim1];
			double w = diag[_dim0]; double h = diag[_dim1];
			
			_world_to_image.TransformPoint(ref lx, ref ly);
			_world_to_image.TransformDistance(ref w, ref h);
			
			gr.LineWidth = _line_width;
			gr.Color = _line_color;
			gr.Rectangle(lx, ly, w, h);
			gr.Stroke();
		}

    private void RenderLeaves<T>(KdNode<T> node, Cairo.Context gr) where T : IVector
    {
      foreach (KdNode<T> n in node.Leaves)
      {
        foreach (IVector iv in n.Vectors)
        {
          this.RenderPoint(iv, gr);
        }
      }
    }

    private void RenderIntermediates<T>(KdNode<T> node, Cairo.Context gr) where T : IVector {
			foreach(KdNode<T> n in node.PreOrder) {
        if (n.Intermediate)
        {
          this.RenderSplitPlane(n, gr);
        }
			}
		}
		
		private void RenderPoint(IVector iv, Cairo.Context gr) {
			double px = iv[_dim0]; double py = iv[_dim1];
			
			_world_to_image.TransformPoint(ref px, ref py);
			
			gr.Arc(px, py, _point_size, 0, 2.0 * Math.PI);
			gr.Color = _point_color;
			gr.Fill();
			gr.Stroke();
		}
		
		private void RenderSplitPlane<T>(KdNode<T> node, Cairo.Context gr) where T : IVector {
			// Cannot render if not in visualization domain
			if (node.SplitDimension != _dim0 && node.SplitDimension != _dim1)
				return;
			
			int dim = node.SplitDimension;
			
			double sx,sy,dx,dy;
			if (dim == _dim0) {
				sx = node.SplitLocation;
				sy = node.Bounds.Lower[_dim1];
				dx = node.SplitLocation;
				dy = node.Bounds.Upper[_dim1];
			} else {
				sx = node.Bounds.Lower[_dim0];
				sy = node.SplitLocation;
				dx = node.Bounds.Upper[_dim0];
				dy = node.SplitLocation;
			}
			
			_world_to_image.TransformPoint(ref sx, ref sy);
			_world_to_image.TransformPoint(ref dx, ref dy);
			
			gr.LineWidth = _line_width;
			gr.Color = _line_color;
			gr.MoveTo(sx, sy);
			gr.LineTo(dx, dy);
			gr.Stroke();
		}
		
		
		private Cairo.Color _point_color;
		private Cairo.Color _line_color;
		private double _line_width;
		private double _point_size; // diameter
		private Cairo.Matrix _world_to_image;
		private int _dim0, _dim1;
		private Vector _image_size;
	}
}
