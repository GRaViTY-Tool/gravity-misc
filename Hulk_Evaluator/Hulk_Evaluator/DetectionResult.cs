using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hulk_Evaluator
{
    class DetectionResult
    {
        public bool Changed { get; set; }
        public bool IsNew { get; set; }
        public bool IsRemoved { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }

        public DetectionResult()
        {
            Changed = false;
            IsNew = false;
            IsRemoved = false;
        }
    }

}
