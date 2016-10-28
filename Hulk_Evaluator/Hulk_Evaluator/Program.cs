using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hulk_Evaluator
{


    class Program
    {

        static DetectionResult lineToDetectionResult(string line)
        {
            string[] split = line.Split('=');
            DetectionResult result = new DetectionResult();
            result.Name = split[0];

            if(split.Length > 1)
            {
                result.Value = split[1];
            }


            return result;
        }

        static List<Detection> fileToDetectionList(string file)
        {
            List<Detection> resolve = new List<Detection>();

            foreach (string line in File.ReadAllLines(file))
            {
                if (line.ToLower().Contains("detector") || line.ToLower().Contains("calculator"))
                {
                    resolve.Add(new Detection(line));
                }
                else if (line.Contains("----"))
                {
                    continue;
                }
                else
                {
                    resolve.Last().addResult(lineToDetectionResult(line));
                }
            }

            return resolve;
        }


        static void initResults(List<Detection> preResolve, List<Detection> postResolve)
        {
            foreach (Detection postDetection in postResolve)
            {
                Detection preDetection = preResolve.Where(d => d.Detector == postDetection.Detector).First();

                foreach (DetectionResult postResult in postDetection.Results)
                {
                    IEnumerable<DetectionResult> preResults = preDetection.Results.Where(r => r.Name == postResult.Name);//.FirstOrDefault();
                    
                    if (!preResults.Any())
                    {
                        postResult.IsNew = true;
                    }
                    else
                    {
                        postResult.Changed = true;

                        foreach(DetectionResult preResult in preResults)
                        {
                            if(preResult.Value == postResult.Value)
                            {
                                postResult.Changed = false;
                            }
                        }

                    }
                }
            }



            foreach (Detection preDetection in preResolve)
            {
                Detection postDetection = postResolve.Where(d => d.Detector == preDetection.Detector).First();

                foreach (DetectionResult preResult in preDetection.Results)
                {
                    IEnumerable<DetectionResult> postResults = postDetection.Results.Where(r => r.Name == preResult.Name);//.FirstOrDefault();

                    if (!postResults.Any())
                    {
                        preResult.IsRemoved = true;
                        postDetection.addResult(preResult);
                    }
                }
            }




        }

        static string valueString(DetectionResult result)
        {
            if (String.IsNullOrWhiteSpace(result.Value))
            {
                return "";
            }

            return " = " + result.Value;
        }


        static void Main(string[] args)
        {


            while (true)
            {
                System.Console.WriteLine("preResolveFile : ");
                string preResolveFile = Console.ReadLine().Replace("\"", "");


                System.Console.WriteLine("postResolveFile : ");
                string postResolveFile = Console.ReadLine().Replace("\"", "");

                List<Detection> preResolve = fileToDetectionList(preResolveFile);

                List<Detection> postResolve = fileToDetectionList(postResolveFile);

                initResults(preResolve, postResolve);


                List<string> output = new List<string>();
                foreach (Detection detection in postResolve)
                {
                    output.Add(detection.Detector);
                    foreach (DetectionResult result in detection.Results)
                    {
                        if (result.Changed)
                        {
                            string oldValue = preResolve.Where(d => d.Detector == detection.Detector).First().Results.Where(r => r.Name == result.Name).First().Value;

                            string str = String.Format("{0,-100} {1}", result.Name + valueString(result), "(oldValue = " + oldValue) + ")";

                            output.Add(str);
                        }
                        if (result.IsNew)
                        {
                            string str = String.Format("{0,-100} {1}", result.Name + valueString(result), "(new Value)");
                            output.Add(str);
                        }
                        if (result.IsRemoved)
                        {
                            string str = String.Format("{0,-100} {1}", result.Name + valueString(result), "(removed Value)");
                            output.Add(str);
                        }
                    }
                    output.Add("-----------------------------------------------------");
                    output.Add("");
                }

                File.WriteAllLines(Path.GetDirectoryName(preResolveFile) + "\\differences", output.ToArray());
            }
        }
    }
}
