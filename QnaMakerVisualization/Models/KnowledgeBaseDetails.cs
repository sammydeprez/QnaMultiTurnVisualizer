using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QnaMakerVisualization.Models
{
    public class KnowledgeBaseDetails
    {
        public string Endpoint { get; set; }
        public string SubscriptionKey { get; set; }
        public string Environment { get; set; }
        public string KnowledgeBaseId { get; set; }
    }
}
