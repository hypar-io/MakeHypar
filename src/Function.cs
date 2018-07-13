using Amazon.Lambda.Core;
using Hypar.Elements;
using System.Collections.Generic;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace Hypar
{
    public class Function
    {
        public Dictionary<string,object> Handler(Dictionary<string,object> input, ILambdaContext context)
        {
            var profile = Profiles.Rectangular();

            var mass = Mass.WithBottomProfile(profile)
                            .WithBottomAtElevation(0)
                            .WithTopAtElevation(1);

            var model = new Model();
            model.AddElement(mass);
            return model.ToHypar();
        }
    }
};
