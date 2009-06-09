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

namespace Accelerators {

  /// <summary>
  /// Min-Priority queue implementation.
  /// </summary>
  public class PriorityQueue<TPriority, TValue> {

    public PriorityQueue() {
      _comp = Comparer<TPriority>.Default;
      _heap = null;
      _count = 0;
      this.Capacity = 31;
    }

    public PriorityQueue(IComparer<TPriority> comparer) {
      _comp = comparer;
      _heap = null;
      _count = 0;
      this.Capacity = 31;
    }

    /// <summary>
    /// Access the capacity. 
    /// </summary>
    public int Capacity {
      get {
        return _heap.Length;
      }
      set {
        // Heap is stored as full balanced tree: we need to find
        // (2^x - 1) >= capacity.
        double x = Math.Ceiling(Math.Log(value + 1) / Math.Log(2));
        int capacity = ((int)Math.Pow(2, x)) - 1;
        // Allocate and copy
        Pair<TPriority, TValue>[] newHeap = new Pair<TPriority, TValue>[capacity];
        if (_heap != null) {
          Array.Copy(_heap, 0, newHeap, 0, _count);
        }
        _heap = newHeap;
      }
    }

    public int Count {
      get {
        return _count;
      }
    }

    public void Push(TPriority p, TValue v) {
      if (_count == this.Capacity) {
        this.Capacity = 2 * this.Capacity + 1;
      }
      _count += 1;
      this.UpHeap(_count - 1, new Pair<TPriority, TValue>(p, v));
    }

    public TValue Pop() {
      if (_count == 0)
        throw new InvalidOperationException("Cannot pop from empty priority queue.");

      TValue first = _heap[0].Second;
      _count -= 1;
      this.DownHeap(0, _heap[_count]);
      return first;
    }

    public TValue Peek() {
      if (_count == 0)
        throw new InvalidOperationException("Cannot pop from empty priority queue.");
      return _heap[0].Second;
    }

    public TPriority PeekPriority() {
      if (_count == 0) {
        throw new InvalidOperationException("Cannot pop from empty priority queue.");
      }
      return _heap[0].First;
    }

    private void UpHeap(int index, Pair<TPriority, TValue> entry) {
      int parent = (index - 1) / 2;

      while ((index > 0) && (_comp.Compare(_heap[parent].First, entry.First) > 0)) {
        _heap[index] = _heap[parent];
        index = parent;
        parent = (index - 1) / 2;
      }
      _heap[index] = entry;
    }

    private void DownHeap(int index, Pair<TPriority, TValue> entry) {
      int child = (index * 2) + 1; // left child
      while (child < _count) {
        if ((child + 1 < _count) && (_comp.Compare(_heap[child].First, _heap[child + 1].First) > 0)) {
          child += 1;
        }
        _heap[index] = _heap[child];
        index = child;
        child = (index * 2) + 1;
      }
      this.UpHeap(index, entry);
    }

    private IComparer<TPriority> _comp;
    private Pair<TPriority, TValue>[] _heap;
    private int _count;
  }
}
