using System;
using System.Collections.Generic;
using System.Text;
using Accelerators;
using System.IO;

namespace RenderTree {
  
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
