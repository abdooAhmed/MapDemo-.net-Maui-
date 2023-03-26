using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapDemo.Models
{
    public class EntireData
    {
        public List<Neuron> Items { get; set; }

        public int Count { get; set; }


    }

    public class Neuron
    {
        public string Id { get; set; }

        public string Tag { get; set; }

        public Object Terminal { get; set; }

        [JsonIgnore]
        public RelativeType Type { get; set; }

        public int Version { get; set; }

        public Object Region { get; set; }

        public Object Creation { get; set; }

        public Object LastModification { get; set; }

        public Object UnifiedLastModification { get; set; }

        public string ExternalReferenceUrl { get; set; }

        public bool Active { get; set; }

        public string Url { get; set; }

        public Object Validation { get; set; }
    }

    public enum RelativeType
    {
        NotSet,
        Postsynaptic,
        Presynaptic
    }
}
