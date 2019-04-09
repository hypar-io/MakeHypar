using Elements;
using Elements.Geometry;
using Elements.Geometry.Profiles;
using System;
using System.Collections.Generic;

namespace ConstructHypar
{
    /// <summary>
    /// Construct a hypar.
    /// </summary>
  	public class ConstructHypar
    {
        public Output Execute(Input input)
        {
            double minLength, maxLength, minEl, maxEl;
            var beams = ContstructBeams(input.XAmplitude, input.YAmplitude, 
                out maxLength, out minLength, out minEl, out maxEl);
            var model = new Model();
            model.AddElements(beams);
            return new Output(model, maxLength, minLength, minEl, maxEl);
        }

        public List<Beam> ContstructBeams(double xAmp, double yAmp, 
            out double maxLength, out double minLength, out double minEl, out double maxEl)
        {
            minLength = double.PositiveInfinity;
            maxLength = double.NegativeInfinity;
            minEl = double.PositiveInfinity;
            maxEl = double.NegativeInfinity;

            var pts = Hypar(xAmp, yAmp);
            var m1 = new Material("red", Colors.Red, 0f, 0f);
            var m2 = new Material("green", Colors.Green, 0f, 0f);

            var t1 = new StructuralFramingType("W16x31", WideFlangeProfileServer.Instance.GetProfileByName("W16x31"), m1);
            var t2 = new StructuralFramingType("W16x31", WideFlangeProfileServer.Instance.GetProfileByName("W16x31"), m2);
            
            var beams = new List<Beam>();

            for(var j=0; j<pts.Count; j++)
            {
                var colA = pts[j];
                List<Vector3> colB = null;
                if(j+1 < pts.Count)
                {
                    colB = pts[j+1];
                }

                for(var i=0; i<colA.Count; i++)
                {
                    var a = colA[i];
                    Vector3 b = null;
                    if(i+1 < colA.Count)
                    {
                        b = colA[i+1];
                        var line1 = new Line(a,b);

                        minLength = Math.Min(line1.Length(), minLength);
                        maxLength = Math.Max(line1.Length(), maxLength);
                        minEl = Math.Min(a.Z, minEl);
                        minEl = Math.Min(b.Z, minEl);
                        maxEl = Math.Max(a.Z, maxEl);
                        maxEl = Math.Max(b.Z, maxEl);

                        var beam1 = new Beam(line1, t1);
                        beams.Add(beam1);
                    }

                    if(colB != null)
                    {
                        var c = colB[i];
                        var line2 = new Line(a,c);

                        minLength = Math.Min(line2.Length(), minLength);
                        maxLength = Math.Max(line2.Length(), maxLength);
                        minEl = Math.Min(a.Z, minEl);
                        minEl = Math.Min(c.Z, minEl);
                        maxEl = Math.Max(a.Z, maxEl);
                        maxEl = Math.Max(c.Z, maxEl);

                        var beam2 = new Beam(line2, t2);
                        beams.Add(beam2);
                    }
                }
            }
            return beams;
        }

        private List<List<Vector3>> Hypar(double a, double b)
        {
            var result = new List<List<Vector3>>();
            for(var x = -5; x<=5; x++)
            {
                var column = new List<Vector3>();
                for(var y=-5; y<=5; y++)
                {
                    var z = Math.Pow(y,2)/Math.Pow(b,2) - Math.Pow(x,2)/Math.Pow(a,2);
                    column.Add(new Vector3(x, y, z));
                }
                result.Add(column);
            }

            return result;
        }
    }
}