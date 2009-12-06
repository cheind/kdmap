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

namespace Rendering {

  /// <summary>
  /// Helps in rendering to cairo surfaces
  /// </summary>
  public class CairoRenderer {

    public CairoRenderer() {
      _p0 = new Cairo.PointD(); 
      _p1 = new Cairo.PointD();
      _world_to_surface = null;
      _projection = new Pair<int, int>(0, 1);
    }

    public void SetupWorldToSurfaceTransform(AABB world, Pair<int,int> projection, double swidth, double sheight, bool center) 
    {
      _projection = projection;

			double inner_x = swidth * 0.9;
			double inner_y = sheight * 0.9;

      IVector diag = world.Diagonal;
			IVector world_center = world.Center;

      _world_to_surface = new Cairo.Matrix(1, 0, 0, -1, 0, 0);
      _world_to_surface.Translate(swidth * 0.5, -sheight * 0.5);
      _world_to_surface.Scale(inner_x / diag[_projection.First], inner_y / diag[_projection.Second]);
      if (center)
        _world_to_surface.Translate(-world_center[_projection.First], -world_center[_projection.Second]);
    }

    /// <summary>
    /// Render geometry of line
    /// </summary>
    public void RenderLine(IVector from, IVector to, Cairo.Context gr) {
      this.TransformPoint(from, ref _p0);
      this.TransformPoint(to, ref _p1);

      gr.MoveTo(_p0);
      gr.LineTo(_p1);
    }

    public void RenderAABB(AABB aabb, Cairo.Context gr) 
    {
      this.TransformPoint(aabb.Lower, ref _p0);
      this.TransformVector(aabb.Diagonal, ref _p1);
      gr.Rectangle(_p0, _p1.X, _p1.Y);
    }

    /// <summary>
    /// Render a list of points.
    /// </summary>
    public void RenderPoints(IEnumerable<IVector> points, Cairo.Context gr, double radius) {
      foreach (IVector iv in points) {
        this.TransformPoint(iv, ref _p0);
        gr.Arc(_p0.X, _p0.Y, radius, 0, 2 * Math.PI);
        gr.Fill();
      }
    }

    /// <summary>
    /// Render a single point
    /// </summary>
    public void RenderPoint(IVector iv, Cairo.Context gr, double radius) {
      this.TransformPoint(iv, ref _p0);
      gr.Arc(_p0.X, _p0.Y, radius, 0, 2 * Math.PI);
    }

    /// <summary>
    /// Transform an n-dimensional point. Projects the vector on the
    /// chosen axes first and then transforms it by the world to image
    /// transform.
    /// </summary>
    public void TransformPoint(IVector v, ref Cairo.PointD p) {
      double vx = v[_projection.First];
      double vy = v[_projection.Second];

      _world_to_surface.TransformPoint(ref vx, ref vy);
      p.X = vx;
      p.Y = vy;
    }

    /// <summary>
    /// Transform an n-dimensional vector (ignoring translational offset).
    /// </summary>
    public void TransformVector(IVector v, ref Cairo.PointD p) {
      double vx = v[_projection.First];
      double vy = v[_projection.Second];

      _world_to_surface.TransformDistance(ref vx, ref vy);
      p.X = vx;
      p.Y = vy;
    }

    private Cairo.PointD _p0, _p1;
    private Pair<int, int> _projection;   // Chosen projection dimensions
    private Cairo.Matrix _world_to_surface; // World to image transform
  }
}
