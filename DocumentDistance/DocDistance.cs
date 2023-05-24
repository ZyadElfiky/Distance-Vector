using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace DocumentDistance
{
    class DocDistance
    {
        // *****
        // DON'T CHANGE CLASS OR FUNCTION NAME
        // YOU CAN ADD FUNCTIONS IF YOU NEED TO
        // *****
        /// <summary>
        /// Write an efficient algorithm to calculate the distance between two documents
        /// </summary>
        /// <param name="doc1FilePath">File path of 1st document</param>
        /// <param name="doc2FilePath">File path of 2nd document</param>
        /// <returns>The angle (in degree) between the 2 documents</returns>
        public static double CalculateDistance(string doc1FilePath, string doc2FilePath)
        {
            Dictionary<string, long> firstfile = new Dictionary<string, long>();
            Dictionary<string, long> secondfile = new Dictionary<string, long>();
            double sum1, sum2, sum;
            string data1, data2;

            data1 = new StreamReader(doc1FilePath).ReadToEnd().ToLower();
            data2 = new StreamReader(doc2FilePath).ReadToEnd().ToLower();

            if (data1.Equals(data2))
                return (double)0;

            // Load the text for each document in parallel
            Parallel.Invoke(
                () => { firstfile = loadtext(data1); },
                () => { secondfile = loadtext(data2); }
            );

            sum = DotProduct(firstfile, secondfile);
            if (sum == -1)
                return (double)90;

            sum1 = DotProduct(firstfile, firstfile);
            sum2 = DotProduct(secondfile, secondfile);
            return Math.Acos(sum / Math.Sqrt(sum1 * sum2)) * 180 / Math.PI;
        }
        private static long DotProduct(Dictionary<string, long> first, Dictionary<string, long> second)
        {
            long sum = 0, cnt = 0;
            foreach (var i in first)
                if (second.ContainsKey(i.Key))
                    sum += i.Value * second[i.Key];
                else
                    cnt++;
            return (cnt == first.Count()) ? -1 : sum;
        }
        public static Dictionary<string, long> loadtext(string data)
        {
            StringBuilder result = new StringBuilder();
            Dictionary<string, long> dic = new Dictionary<string, long>();
            string res;
            long cnt;

            foreach (char w in data)
            {
                if (char.IsLetterOrDigit(w))
                    result.Append(w);
                else
                {
                    res = result.ToString();
                    if (res != "")
                    {
                        dic.TryGetValue(res, out cnt);
                        dic[res] = cnt + 1;
                    }
                    result.Clear();
                }
            }
            if (result.Length != 0)
            {
                res = result.ToString();
                if (res != "")
                {
                    dic.TryGetValue(res, out cnt);
                    dic[res] = cnt + 1;
                }
            }
            return dic;
        }
    }
}