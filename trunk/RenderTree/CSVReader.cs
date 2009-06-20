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
using Accelerators;
using System.IO;

namespace RenderTree
{
  /// <summary>
  /// Simple CSV reader to parse vectors.
  /// </summary>
	class CSVReader
	{
    public CSVReader(char deliminator)
    {
      _deliminators = new char[] { deliminator };
    }

    public IList<IVector> Parse(string path)
    {
      List<IVector> vecs = new List<IVector>();
      using (StreamReader sr = new StreamReader(path)) {
        string line;
        while ((line = sr.ReadLine()) != null) {
          string[] values = line.Split(_deliminators);
          Vector v = new Vector(values.Length);
          for(int i = 0; i < v.Dimensions; ++i)
            v[i] = Double.Parse(values[i]);
          vecs.Add(v);
        }
      }
      return vecs;
    }

    private char[] _deliminators;
	}
}
