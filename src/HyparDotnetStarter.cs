using Hypar.Elements;
using Hypar.Functions;
using Hypar.Geometry;
using System.Linq;

namespace Hypar
{
    /// <summary>
    /// The Hypar starter generator.
    /// </summary>
  	public class HyparDotnetStarter
	{
		public Output Execute(Input input)
		{
			// Extract location data.
            // The GeoJSON may contain a number of features. Here we just
            // take the first one assuming it's a Polygon, and we use
            // its first point as the origin. 
            var outline = (Hypar.GeoJSON.Polygon)input.Location[0].Geometry;
            var origin = outline.Coordinates[0][0];
            var offset = origin.ToVectorMeters();
            var plines = outline.ToPolygons();
            var pline = plines[0];
            var boundary = new Hypar.Geometry.Polygon(pline.Vertices.Select(v => new Vector3(v.X - offset.X, v.Y - offset.Y, v.Z)).ToList());

            var mass = new Mass(boundary, 0, input.Height);

            // Add your mass element to a new Model.
            var model = new Model();
            model.AddElement(mass);

            // Set the origin of the model to convey to Hypar
            // where to position the generated 3D model.
            model.Origin = origin;

            var output = new Output(model, mass.Profile.Perimeter.Area);

            return output;
		}
  	}
}
