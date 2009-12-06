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
using Accelerators;
using System.IO;

namespace Rendering {
  
  /// <summary>
  /// Render kd-tree
  /// </summary>
  public class RenderTreeCairo {

    private Cairo.Surface CreateSurface(string filename, double width, double height) {
      FileInfo fi = new FileInfo(filename);
      _render_image = false;
      switch (fi.Extension) {
        case ".pdf":
          return new Cairo.PdfSurface(filename, width, height);
        case ".ps":
          return new Cairo.PSSurface(filename, width, height);
        case ".svg":
          return new Cairo.SvgSurface(filename, width, height);
        case ".png":
          _render_image = true;
          return new Cairo.ImageSurface(Cairo.Format.Argb32, (int)width, (int)height);
        default:
          throw new ArgumentException(String.Format("Unknown file extension {0}", fi.Extension));
      }
    }

    public void Render(KdNode<IVector> tree, Pair<int,int> projection, string filename, double width, double height, double linewidth, double pointsize) {
      using (Cairo.Surface surface = CreateSurface(filename, width, height)) {
        using (Cairo.Context gr = new Cairo.Context(surface)) {

          Cairo.Color black = new Cairo.Color(0, 0, 0);
          Cairo.Color green = new Cairo.Color(0, 0.4, 0);
          Cairo.Color white = new Cairo.Color(1, 1, 1);

          gr.LineWidth = linewidth;
          gr.Antialias = Cairo.Antialias.Default;

          // Background
          gr.Color = white;
          gr.Rectangle(0, 0, width, height);
          gr.Fill();
          gr.Stroke();

          // Prepare for world to surface rendering
          CairoRenderer cr = new CairoRenderer();
          cr.SetupWorldToSurfaceTransform(tree.InternalBounds, projection, width, height, true);

          // Render world bounds
          gr.Color = black;
          cr.RenderAABB(tree.InternalBounds, gr);
          gr.Stroke();

          // Render leaves
          gr.Color = green;
          foreach (KdNode<IVector> iv in tree.Leaves) {
            cr.RenderPoints(iv.Vectors, gr, pointsize);  
          }
          gr.Stroke();

          // Render intermediates
          gr.Color = black;
          foreach (KdNode<IVector> n in tree.PreOrder) {
            if (n.Intermediate) {
              AABB split_bounds = n.SplitBounds;
              if (n.SplitDimension == projection.First) {
                Vector from = Vector.Create(n.SplitLocation, split_bounds.Lower[projection.Second]);
                Vector to = Vector.Create(n.SplitLocation, split_bounds.Upper[projection.Second]);
                cr.RenderLine(from, to, gr);
              } else if (n.SplitDimension == projection.Second) {
                Vector from = Vector.Create(split_bounds.Lower[projection.First], n.SplitLocation);
                Vector to = Vector.Create(split_bounds.Upper[projection.First], n.SplitLocation);
                cr.RenderLine(from, to, gr);   
              }
            }
          }
          gr.Stroke();

          if (_render_image)
            surface.WriteToPng(filename);
          else
            gr.ShowPage();
        }
      }
    }

    private bool _render_image;
  }
}
