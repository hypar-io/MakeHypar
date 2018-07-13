using Hypar.Elements;
using Amazon.Lambda.Core;
using System.Collections.Generic;

namespace Junk
{
    public class Junk
    {
        public Dictionary<string,object> Handler(Dictionary<string,object> input, ILambdaContext context)
        {
            var model = new Model();

            // Insert your code here.

            return model.ToHypar();
        }
    }
}