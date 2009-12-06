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

namespace Accelerators.Searches
{
  
  /// <summary>
  /// Search for the closest neighbors incrementally.
  /// </summary>
  /// <remarks>
  /// Based on "Distance Browsing in Spatial Databases", Gısli R. Hjaltason and Hanan Samet
  /// </remarks>
  public class IncrementalDistanceSearch<T> : KdTreeSearch<T> where T : IVector 
  {
    /// <summary>
    /// Initialize with the kd-tree node to start search at.
    /// </summary>
    public IncrementalDistanceSearch(KdNode<T> tree) : base(tree) {}
    
    /// <summary>
    /// Search for closest elements to the given query in increasing order of distance (L2)
    /// </summary>
    public IEnumerable<T> Find(IVector x) {
      // Setup the priority queue for the search
      _pq = new PriorityQueue<PQEntry, PQEntry>();
      _closest = new Vector(x.Dimensions);
      
      // Search is started at root
      this.Tree.InternalBounds.Closest(x, ref _closest);
      double dist2 = VectorReductions.SquaredL2NormDistance(x, _closest);
      if (dist2 <= this.SquaredDistanceLimit) {
        PQEntry root_entry = PQEntry.CreateNodeEntry(dist2, this.Tree);
        _pq.Push(root_entry, root_entry);
      }
      
      int found = 0;
      while (found < this.CountLimit && _pq.Count > 0) {
        PQEntry entry = _pq.Pop();
        if (entry.NodeEntry) {
          this.ProcessNodeEntry(entry, x);
        } else {
          yield return entry.Element;
          found += 1;
        }
      }
    }
    
    /// <summary>
    /// Process a node
    /// </summary>
    private void ProcessNodeEntry(PQEntry entry, IVector x) {
      KdNode<T> node = entry.Node;
      double dist2;
      
      if (node.Leaf) {
        foreach(T t in node.Vectors) {
          dist2 = VectorReductions.SquaredL2NormDistance(x, t);
          if (dist2 <= this.SquaredDistanceLimit) {
            PQEntry e = PQEntry.CreateElementEntry(dist2, t);
            _pq.Push(e, e);
          }
        }
      } else {
        node.Left.InternalBounds.Closest(x, ref _closest);
        dist2 = VectorReductions.SquaredL2NormDistance(x, _closest);
        if (dist2 <= this.SquaredDistanceLimit) {
          PQEntry left = PQEntry.CreateNodeEntry(dist2, node.Left);
          _pq.Push(left, left);
        }
        
        node.Right.InternalBounds.Closest(x, ref _closest);
        dist2 = VectorReductions.SquaredL2NormDistance(x, _closest);
        if (dist2 <= this.SquaredDistanceLimit) {
          PQEntry right = PQEntry.CreateNodeEntry(dist2, node.Right);
          _pq.Push(right, right);
        }
      }
    }
    
    /// <summary>
    /// Entry in the priority queue
    /// </summary>
    private class PQEntry : IComparable<PQEntry> {
      /// <summary>
      /// Construct from distance, node or element (mutual exclusive).
      /// </summary>
      public PQEntry(double distance, KdNode<T> node, T element) {
        _node = node;
        _element = element;
        _distance = distance;
      }
      
      /// <summary>
      /// Create a new element entry from distance and element
      /// </summary>
      public static PQEntry CreateElementEntry(double distance, T element) {
        return new PQEntry(distance, null, element);
      }
      
      /// <summary>
      /// Create a new node entry from distance and node
      /// </summary>
      public static PQEntry CreateNodeEntry(double distance, KdNode<T> node) {
        return new PQEntry(distance, node, default(T));
      }
                                            
      
      /// <summary>
      /// Provide a strong ordering of the entries
      /// </summary>
      public int CompareTo(PQEntry other)
      {
        if (this.NodeEntry && other.ElementEntry) {
          // Node types come before element types.
          return -1;
        } else if (this.ElementEntry && other.NodeEntry) {
          // Node types come before element types.
          return 1;
        } else {
          // both are of same type
          return _distance.CompareTo(other._distance);
        }
      }

      
      /// <value>
      /// Test if entry corresponds to a node 
      /// </value>
      public bool NodeEntry {
        get {return _node != null;}
      }
      
      /// <value>
      /// Test if entry corresponds to an element 
      /// </value>
      public bool ElementEntry {
        get {return !this.NodeEntry;}
      }
      
      /// <value>
      /// Access the node  
      /// </value>
      public KdNode<T> Node {
        get {return _node;}
      }
      
      /// <value>
      /// Access the element 
      /// </value>
      public T Element {
        get {return _element;}
      }
      
      private KdNode<T> _node;
      private T _element;
      private double _distance;
    }
    
    private PriorityQueue<PQEntry, PQEntry> _pq;
    private IVector _closest;
  }
}
