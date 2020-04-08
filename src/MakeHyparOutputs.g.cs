// This code was generated by Hypar.
// Edits to this code will be overwritten the next time you run 'hypar init'.
// DO NOT EDIT THIS FILE.

using Elements;
using Elements.GeoJSON;
using Elements.Geometry;
using Hypar.Functions;
using Hypar.Functions.Execution;
using Hypar.Functions.Execution.AWS;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace MakeHypar
{
    public class MakeHyparOutputs: ResultsBase
    {
		/// <summary>
		/// The maximum length of a beam.
		/// </summary>
		[JsonProperty("Maximum Beam Length")]
		public double MaximumBeamLength {get;}

		/// <summary>
		/// The minimum length of a beam.
		/// </summary>
		[JsonProperty("Minimum Beam Length")]
		public double MinimumBeamLength {get;}

		/// <summary>
		/// The minimum elevation of any beam's center line vertices.
		/// </summary>
		[JsonProperty("Minimum Elevation")]
		public double MinimumElevation {get;}

		/// <summary>
		/// The maximum elevation of any beam's center line vertices.
		/// </summary>
		[JsonProperty("Maximum Elevation")]
		public double MaximumElevation {get;}


        
        /// <summary>
        /// Construct a MakeHyparOutputs with default inputs.
        /// This should be used for testing only.
        /// </summary>
        public MakeHyparOutputs() : base()
        {

        }


        /// <summary>
        /// Construct a MakeHyparOutputs specifying all inputs.
        /// </summary>
        /// <returns></returns>
        [JsonConstructor]
        public MakeHyparOutputs(double maximumbeamlength, double minimumbeamlength, double minimumelevation, double maximumelevation): base()
        {
			this.MaximumBeamLength = maximumbeamlength;
			this.MinimumBeamLength = minimumbeamlength;
			this.MinimumElevation = minimumelevation;
			this.MaximumElevation = maximumelevation;

		}

		public override string ToString()
		{
			var json = JsonConvert.SerializeObject(this);
			return json;
		}
	}
}