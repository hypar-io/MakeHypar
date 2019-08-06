using System;
using System.Collections.Generic;
using Elements;
using Elements.Geometry;
using Elements.Geometry.Profiles;

namespace MakeHypar
{
  	public static class MakeHypar
	{
		/// <summary>
		/// Construct a hypar.
		/// </summary>
		/// <param name="model">The model. 
		/// Add elements to the model to have them persisted.</param>
		/// <param name="input">The arguments to the execution.</param>
		/// <returns>A HyparOutputs instance containing computed results.</returns>
		public static MakeHyparOutputs Execute(Model model, MakeHyparInputs input)
		{
			double minLength, maxLength, minEl, maxEl;
            var beams = ContstructBeams(input.XAmplitude, input.YAmplitude, (int)input.Divisions, 
                out maxLength, out minLength, out minEl, out maxEl);
            model.AddElements(beams);
            return new MakeHyparOutputs(maxLength, minLength, minEl, maxEl);
		}

		public static List<Beam> ContstructBeams(double xAmp, double yAmp, int divisions,
            out double maxLength, out double minLength, out double minEl, out double maxEl)
        {
            minLength = double.PositiveInfinity;
            maxLength = double.NegativeInfinity;
            minEl = double.PositiveInfinity;
            maxEl = double.NegativeInfinity;

            var pts = ConstructHypar(xAmp, yAmp, divisions);
            var m1 = new Material("red", Colors.Yellow, 0f, 0f);
            var m2 = new Material("green", Colors.Blue, 0f, 0f);

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

                        var l = line1.Length();
                        minLength = Math.Min(l, minLength);
                        maxLength = Math.Max(l, maxLength);
                        minEl = Math.Min(a.Z, minEl);
                        minEl = Math.Min(b.Z, minEl);
                        maxEl = Math.Max(a.Z, maxEl);
                        maxEl = Math.Max(b.Z, maxEl);

                        var beam1 = new Beam(line1, t1);
                        beam1.AddProperty("length", new NumericProperty(l, UnitType.Distance));

                        beams.Add(beam1);
                    }

                    if(colB != null)
                    {
                        var c = colB[i];
                        var line2 = new Line(a,c);

                        var l = line2.Length();
                        minLength = Math.Min(l, minLength);
                        maxLength = Math.Max(l, maxLength);
                        minEl = Math.Min(a.Z, minEl);
                        minEl = Math.Min(c.Z, minEl);
                        maxEl = Math.Max(a.Z, maxEl);
                        maxEl = Math.Max(c.Z, maxEl);

                        var beam2 = new Beam(line2, t2);
                        beam2.AddProperty("length", new NumericProperty(l, UnitType.Distance));

                        beams.Add(beam2);
                    }
                }
            }
            return beams;
        }

        private static List<List<Vector3>> ConstructHypar(double a, double b, int div)
        {
            var result = new List<List<Vector3>>();
            for(var x = -div/2; x<=div/2; x++)
            {
                var column = new List<Vector3>();
                for(var y=-div/2; y<=div/2; y++)
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