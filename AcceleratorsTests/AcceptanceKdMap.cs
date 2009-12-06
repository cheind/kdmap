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
using NUnit.Framework;
using System.Collections.Generic;

namespace AcceleratorsTests
{
  
  [TestFixture()]
  public class AcceptanceKdMap
  {

    /// <summary>
    /// A two-dimensional point for illustration purposes.
    /// GeoCoord implements IVector and can thus be used in classes and methods of the Accelerators namespace.
    /// </summary>
    class GeoCoord : Accelerators.IVector {
      public GeoCoord(double latitude, double longitude) { _coordinates = new double[] { latitude, longitude }; }

      /// <summary>
      /// Access dimensionality
      /// </summary>
      public int Dimensions {
        get { return 2; }
      }

      /// <summary>
      /// Access individual coordinates
      /// </summary>
      public double this[int index] {
        get { return _coordinates[index]; }
        set { _coordinates[index] = value; }
      }

      public override string ToString() {
        return "[" + _coordinates[0] + "," + _coordinates[1] + "]";
      }

      private double[] _coordinates;
    }

    /// <summary>
    /// Illustrates usage of KdMap.
    /// </summary>
    // [Test()]
    public void IllustrateKdMap() {
      // Create a new KdMap (with the desired number of dimensions) 
      // and access it through the generic IDictionary interface.
      IDictionary<GeoCoord, string> cities = new Accelerators.KdMap<GeoCoord, string>(2);

      // Add some elements to the dictionary. There are no duplicate keys in terms of their
      // coordinates.
      cities.Add(new GeoCoord(48.2, 16.3), "Vienna");
      cities.Add(new GeoCoord(40.7, -73.9), "New York");
      cities.Add(new GeoCoord(59.9, 30.3), "Sankt Petersburg");
      cities.Add(new GeoCoord(52.5, 13.4), "Berlin");
      cities.Add(new GeoCoord(47.0, 15.4), "Graz");
      cities.Add(new GeoCoord(47.8, 13.0), "Salzburg");
      cities.Add(new GeoCoord(48.8, 2.3), "Paris");
      cities.Add(new GeoCoord(41.9, 12.4), "Rome");

      
      // Adding an element whose coordinates are already present throws and exception
      try {
        cities.Add(new GeoCoord(48.2, 16.3), "Vienna2");
      } catch (ArgumentException) {
        System.Console.WriteLine("Key (48.2, 16.3) is already present.");
      }
      
      // The item property is a way to access values associated with keys
      System.Console.WriteLine("Label at (48.2, 16.3) is '{0}'.", cities[new GeoCoord(48.2, 16.3)]);

      // Accessing keys is provided through a read-only collection
      ICollection<GeoCoord> keys = cities.Keys;
      // Accessing values is provided as well.
      ICollection<string> values = cities.Values;

      // Elements are removed by matching their coordinates via the IVector interface.
      // Testing equality of double values is considered numerically instable, which is
      // why you should first search for the closest element and than remove the element returned
      // from this search.
      cities.Remove(new GeoCoord(47.8, 13.0));

      // Iterating over the collection of key-value pairs
      foreach (KeyValuePair<GeoCoord, string> kvp in cities) {
        System.Console.WriteLine("<{0}, {1}>", kvp.Key, kvp.Value);
      }

    }
  }
}
