using Amazon.Lambda.Core;
using Hypar.Geometry;
using Hypar.Elements;
using System.Collections.Generic;
using System.Linq;

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
            var masses = m.Elements.Where(e=>e.GetType() == typeof(Mass));
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

            var profile = Profiles.Rectangular(Hypar.Geometry.Vector3.Origin(), width, height);

            var mass = Mass.WithBottomProfile(profile)
                            .WithBottomAtElevation(0)
                            .WithTopAtElevation(length);

            // Add your mass element to a new Model.
            var model = new Model();
            model.AddElement(mass);

            // Add an extractor to count the masses.
            model.AddDataExtractor("number_of_masses", new MassCounter());

            // Use the ToHypar convenience method to serialize your Model.
            return model.ToHypar();
        }
    }
};
