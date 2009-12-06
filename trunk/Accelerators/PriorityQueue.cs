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

namespace Accelerators {

  /// <summary>
  /// A min priority queue
  /// </summary>
  /// 
  /// <remarks>
  /// Implementation is based on a heap-like data structure stored in an array. 
  /// When the initial capacity is reached the queue dynamically grows by doubling the size of the internal data structure.
  /// The queue never releases acquired memory until finalization. 
  /// 
  /// By default the queue is a min-priority queue. That is, the top element is the smallest according the the comparer used.
  /// To change this behaviour simply exchange the comparer.
  /// </remarks>
  public class PriorityQueue<TPriority, TValue> {

    /// <summary>
    /// Default constructor. Requires that a comparer for TPriority exists.
    /// </summary>
    public PriorityQueue() {
      _comp = Comparer<TPriority>.Default;
      _heap = null;
      _count = 0;
      this.Capacity = 31;
    }

    /// <summary>
    /// Construct with custom comparer.
    /// </summary>
    public PriorityQueue(IComparer<TPriority> comparer) {
      _comp = comparer;
      _heap = null;
      _count = 0;
      this.Capacity = 31;
    }

    /// <summary>
    /// Access the capacity. The returned capacity might deviate
    /// from the previously set capacity, because the heap is stored as
    /// a full balanced tree in array form which requires to round up the
    /// supplied capacity to the next possible value.
    /// </summary>
    /// <remarks>
    /// The capacity is calculated from the given value, v, by solving for x in
    /// 2^x - 1 >= v, where x is constraint to be an integer value.
    /// </remarks>
    public int Capacity {
      get {
        return _heap.Length;
      }
      set {
        // Heap is stored as full balanced tree
        double x = Math.Ceiling(Math.Log(value + 1) / Math.Log(2));
        int capacity = ((int)Math.Pow(2, x)) - 1;
        // Allocate and copy if required
        Pair<TPriority, TValue>[] newHeap = new Pair<TPriority, TValue>[capacity];
        if (_heap != null) {
          Array.Copy(_heap, 0, newHeap, 0, _count);
        }
        _heap = newHeap;
      }
    }

    /// <value>
    /// Access the number of elements in the queue 
    /// </value>
    public int Count {
      get {
        return _count;
      }
    }

    /// <summary>
    /// Enqueue an element with priority
    /// </summary>
    public void Push(TPriority p, TValue v) {
      if (_count == this.Capacity) {
        this.Capacity = 2 * this.Capacity + 1;
      }
      _count += 1;
      this.UpHeap(_count - 1, new Pair<TPriority, TValue>(p, v));
    }

    /// <summary>
    /// Remove the top element according to the comparer used.
    /// </summary>
    public TValue Pop() {
      if (_count == 0)
        throw new InvalidOperationException("Cannot pop from empty priority queue.");

      TValue first = _heap[0].Second;
      _count -= 1;
      this.DownHeap(0, _heap[_count]);
      return first;
    }

    /// <summary>
    /// Peek at the top element
    /// </summary>
    public TValue Peek() {
      if (_count == 0)
        throw new InvalidOperationException("Cannot pop from empty priority queue.");
      return _heap[0].Second;
    }

    /// <summary>
    /// Peek at the top priority 
    /// </summary>
    public TPriority PeekPriority() {
      if (_count == 0) {
        throw new InvalidOperationException("Cannot pop from empty priority queue.");
      }
      return _heap[0].First;
    }

    /// <summary>
    /// Up-heap the entry by starting comparison with the parent of the supplied index.
    /// </summary>
    private void UpHeap(int index, Pair<TPriority, TValue> entry) {
      int parent = (index - 1) / 2;

      // While there is a parent and the priority of the parent greater than the priority of the supplied entry
      while ((index > 0) && (_comp.Compare(_heap[parent].First, entry.First) > 0)) {
        // down-shuffle the parent
        _heap[index] = _heap[parent];
        // continue search at parent
        index = parent;
        // compare with new parent
        parent = (index - 1) / 2;
      }
      // at this point index marks the position of the entry
      _heap[index] = entry;
    }

    /// <summary>
    /// Down-Heap 
    /// </summary>
    private void DownHeap(int index, Pair<TPriority, TValue> entry) {
      // First set to left child of index to be removed.
      int child = (index * 2) + 1; // left child
      // As long as we haven't reached [past] the last child.
      while (child < _count) {
        // If the right child is in reach and the priority of the right child is less than the priority of the left child
        // then move to the left child.
        if ((child + 1 < _count) && (_comp.Compare(_heap[child].First, _heap[child + 1].First) > 0)) {
          child += 1;
        }
        // Move the lesser child upward
        _heap[index] = _heap[child];
        index = child;
        // Goto left child
        child = (index * 2) + 1;
      }
      this.UpHeap(index, entry);
    }

    private IComparer<TPriority> _comp;
    private Pair<TPriority, TValue>[] _heap;
    private int _count;
  }
}
