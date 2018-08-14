using Amazon.Lambda.Core;
using Hypar.Geometry;
using Hypar.Elements;
using Hypar.GeoJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Hypar
{
    /// <summary>
    /// MassCounter implements the IDataExtractor interface.
    /// When added to a model, this extractor just counts the number of masses.
    /// This is a silly example because you'd probably want to extract more
    /// meaningful data.
    /// </summary>
    public class MassCounter : Hypar.Elements.IDataExtractor
    {
        public double ExtractData(Model m)
        {
            var masses = m.Elements.Where(e=>e.Value.GetType() == typeof(Mass));
            return masses.Any()?masses.Count():0;
        }
    }

    public class Function
    {
        /// <summary>
        /// The Handler method is executed by Hypar.
        /// </summary>
        /// <param name="input">A dictionary containing input data. 
        /// This dictionary will be deserialized by the platform.</param>
        /// <param name="context">A context object used by Lambda.
        /// DO NOT use this, it will most likely go away in the future.</param>
        /// <returns>A dictionary containing results.</returns>
        public Dictionary<string,object> Handler(Dictionary<string,object> input, ILambdaContext context)
        {
            var length = double.Parse(input["length"].ToString());  // Find the length and convert to a number.
            var width = double.Parse(input["width"].ToString());    // Find the width and convert to a number.
            var height = double.Parse(input["height"].ToString());  // Find the height and convert to a number.
            var features = JsonConvert.DeserializeObject<Feature[]>(input["location"].ToString());
            
            // Extract location data.
            // The GeoJSON may contain a number of features. Here we just
            // take the first one assuming it's a Polygon, and we use
            // its first point as the origin. 
            var outline = (Polygon)features[0].Geometry;
            var origin = outline.Coordinates[0][0];
            var offset = origin.ToVectorMeters();
            var plines = outline.ToPolylines();
            var pline = plines[0];
            var boundary = new Polyline(pline.Vertices.Select(v=>new Vector3(v.X - offset.X, v.Y - offset.Y, v.Z)).Reverse());

            var mass = Mass.WithBottomProfile(boundary)
                            .WithTopProfile(boundary)
                            .WithBottomAtElevation(0)
                            .WithTopAtElevation(length);

            // Add your mass element to a new Model.
            var model = new Model();
            model.AddElement(mass);

            // Set the origin of the model to convey to Hypar
            // where to position the generated 3D model.
            model.Origin = origin;

            // Add an extractor to count the masses.
            model.AddDataExtractor("number_of_masses", new MassCounter());

            // Use the ToHypar convenience method to serialize your Model.
            return model.ToHypar();
        }
    }
};
