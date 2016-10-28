using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hulk_Evaluator
{
    class Detection
    {
        public string Detector { get; set; }
        public List<DetectionResult> Results { get; set; }

        public Detection(string name)
        {
            Detector = name;
            Results = new List<DetectionResult>();
        }

        public void addResult(DetectionResult result)
        {
            if (Results == null)
            {
                Results = new List<DetectionResult>();
            }
            Results.Add(result);
        }
    }
}
