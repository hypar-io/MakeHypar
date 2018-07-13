using Amazon.Lambda.Core;
using Hypar.Geometry;
using Hypar.Elements;
using System.Collections.Generic;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Hypar
{
    public class Function
    {
        public Dictionary<string,object> Handler(Dictionary<string,object> input, ILambdaContext context)
        {
            var length = double.Parse(input["length"].ToString());  // Find the length and convert to a number.
            var width = double.Parse(input["width"].ToString());    // Find the width and convert to a number.
            var height = double.Parse(input["height"].ToString());  // Find the height and convert to a number.

            var profile = Profiles.Rectangular(Hypar.Geometry.Vector3.Origin(), width, height);

            var mass = Mass.WithBottomProfile(profile)
                            .WithBottomAtElevation(0)
                            .WithTopAtElevation(length);

            var model = new Model();
            model.AddElement(mass);
            return model.ToHypar();
        }
    }
};
