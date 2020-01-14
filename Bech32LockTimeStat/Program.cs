using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Bech32LockTimeStat
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("locktime-data.txt");
            var myDic = new Dictionary<int, int>();
            foreach (var line in lines)
            {
                var word = line.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
                var currentBlock = int.Parse(word[1]);
                var locktime = int.Parse(word[2]);

                var diff = locktime - currentBlock;
                int relativeLocktime;
                if (locktime == 0)
                {
                    relativeLocktime = 1;
                }
                else if (diff == 0)
                {
                    relativeLocktime = 0;
                }
                else if (diff > 0)
                {
                    continue;
                }
                else
                {
                    relativeLocktime = diff;
                }
                // Console.WriteLine($"locktime: {relativeLocktime}");

                if (myDic.ContainsKey(relativeLocktime))
                {
                    myDic[relativeLocktime]++;
                }
                else
                {
                    myDic.Add(relativeLocktime, 1);
                }
            }

            var totalValid = myDic.Sum(x => x.Value);
            foreach (var pair in myDic.OrderByDescending(x => x.Key))
            {
                var locktimeString = pair.Key == 1 ? "zero" : pair.Key == 0 ? "tip" : pair.Key.ToString();
                Console.WriteLine($"locktime {locktimeString}, count: {pair.Value}, percentage: {Math.Round((((decimal)pair.Value / totalValid) * 100), 2)}%");
            }

            Console.WriteLine("Press a key to exit...");
            Console.ReadKey();
        }
    }
}
