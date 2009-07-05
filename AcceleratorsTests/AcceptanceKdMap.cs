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
      IDictionary<GeoCoord, string> marks = new Accelerators.KdMap<GeoCoord, string>(2);

      // Add some elements to the dictionary. There are no duplicate keys in terms of their
      // coordinates.
      marks.Add(new GeoCoord(48.2, 16.3), "Vienna");
      marks.Add(new GeoCoord(40.7, -73.9), "New York");
      marks.Add(new GeoCoord(59.9, 30.3), "Sankt Petersburg");
      marks.Add(new GeoCoord(52.5, 13.4), "Berlin");
      marks.Add(new GeoCoord(47.0, 15.4), "Graz");
      marks.Add(new GeoCoord(47.8, 13.0), "Salzburg");
      marks.Add(new GeoCoord(48.8, 2.3), "Paris");
      marks.Add(new GeoCoord(41.9, 12.4), "Rome");

      
      // Adding an element whose coordinates are already present throws and exception
      try {
        marks.Add(new GeoCoord(48.2, 16.3), "Vienna2");
      } catch (ArgumentException) {
        System.Console.WriteLine("Key (48.2, 16.3) is already present.");
      }
      
      // The item property is a way to access values associated with keys
      System.Console.WriteLine("Label at (48.2, 16.3) is '{0}'.", marks[new GeoCoord(48.2, 16.3)]);

      // Accessing keys is provided through a read-only collection
      ICollection<GeoCoord> keys = marks.Keys;
      // Accessing values is provided as well.
      ICollection<string> values = marks.Values;

      // Elements are removed by matching their coordinates via the IVector interface.
      // Testing equality of double values is considered numerically instable, which is
      // why you should first search for the closest element and than remove the element returned
      // from this search.
      marks.Remove(new GeoCoord(47.8, 13.0));

      // Iterating over the collection of key-value pairs
      foreach (KeyValuePair<GeoCoord, string> kvp in marks) {
        System.Console.WriteLine("<{0}, {1}>", kvp.Key, kvp.Value);
      }

    }
  }
}
