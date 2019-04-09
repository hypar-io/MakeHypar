using System;
using System.IO;
using Xunit;
using Elements.Serialization.glTF;

namespace ConstructHypar
{
    /// <summary>
    /// This test suite simulates the execution of your function when running on Hypar.
    /// </summary>
    public class FunctionTest
    {
        // Some test data that replicates the payload sent to your function.
        private const string _testData = @"{
                ""X Amplitude"": 2.0,
                ""Y Amplitude"": 2.0
            }";

        private Input _data;

        public FunctionTest()
        {
            var serializer = new Amazon.Lambda.Serialization.Json.JsonSerializer();
            
            // Construct a stream from our test data to replicate how Hypar 
            // will get the data.
            using (var stream = GenerateStreamFromString(_testData))
            {
                _data = serializer.Deserialize<Input>(stream);
            }

            // Add a model to the input to simulate a model
            // passing from a previous execution.
            // _data.Model = new Model();
            // var spaceProfile = new Profile(Polygon.Rectangle(2, 2));
            // var space = new Space(spaceProfile, 2, 0);
            // _data.Model.AddElement(space);
        }

        [Fact]
        public async void Test()
        {
            // Execute the function.
            // As part of the execution, the model will be
            // written to /<tmp>/<execution_id>_model.glb
            var func = new Function();
            var output = await func.Handler(_data, null);

            Assert.NotNull(output.Model);

            // Output the model to the live directory.
            // This will enable 
            output.Model.ToGlTF("../../../../model.glb");

            // Check that the computed values are as expected.
            Assert.True(Math.Abs(output.MaximumBeamLength) > 0.0);
        }

        private static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
