using Hypar;
using Hypar.Functions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace test
{
    public class FunctionTest
    {
        // Some test data that replicates the payload sent to your function.
        private const string _testData = @"{
                ""height"": 20.0,
                ""location"": [
                    {
                        ""geometry"": {
                        ""type"": ""Polygon"",
                        ""coordinates"": [
                            [
                                [
                                    -96.78204,
                                    32.78411
                                ],
                                [
                                    -96.78191,
                                    32.78359
                                ],
                                [
                                    -96.78050,
                                    32.78383
                                ],
                                [
                                    -96.78063,
                                    32.78438
                                ],
                                [
                                    -96.78204,
                                    32.78411
                                ]
                            ]
                        ]
                    },
                    ""type"": ""Feature"",
                    ""properties"": {}
                }
            ]
            }";

        private Input _data;

        public FunctionTest()
        {
            var serializer = new Amazon.Lambda.Serialization.Json.JsonSerializer();
            
            // Construct a stream from our test data to replicate how Lambda 
            // will get the data.
            using (var stream = GenerateStreamFromString(_testData))
            {
                _data = serializer.Deserialize<Input>(stream);
            }
            
            // Cache the data so it's available to the function test below.
            //_data = JsonConvert.DeserializeObject<Dictionary<string,object>>(_testData);
        }

        [Fact]
        public void Test()
        {
            var func = new Function();
            var output = func.Handler(_data, null);
            var computed = JsonConvert.DeserializeObject<Dictionary<string,object>>((string)output["computed"]);
            Assert.True(Math.Abs((double)computed["area"]) > 0.0);

            // Save the model to a .glb so we can view it
            File.WriteAllBytes("../../../../model.glb", Convert.FromBase64String((string)output["model"]));

            // Serialize the results to json, so we can preview the results.
            // When Lambda runs the function, this is not necessary because it
            // handles serializing the result to a stream.
            var json = JsonConvert.SerializeObject(output);
            Console.WriteLine(json);
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
