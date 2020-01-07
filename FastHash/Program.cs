using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastHash
{
	class Program
	{
		static void Main(string[] args)
		{
			// This is a collision test, a comparison can be made through this link, where the results of other algorithms are published:
			// https://softwareengineering.stackexchange.com/questions/49550/which-hashing-algorithm-is-best-for-uniqueness-and-speed
			var file = "words.txt";
			var path = System.IO.Directory.GetCurrentDirectory();
			var fileName = path + @"\" + file;
			if (!System.IO.File.Exists(fileName))
				fileName = path + @"\..\..\" + file;
			if (!System.IO.File.Exists(fileName))
				Console.WriteLine("The collision test can not continue, file not found: " + file);
			else
			{
				Console.WriteLine("Collision tests, and benchmarks are underway, these operations take some time, wait ...");
				string[] lines = System.IO.File.ReadAllLines(fileName);
				for (int i = 0; i < lines.Length; i++)
				{
					lines[i] = lines[i].ToLower();
				}
				var hashs = new List<byte[]>();
				Console.WriteLine("Search for collisions in a dictionary of " + lines.Length + " lowercase English words");
				var collisions = 0;
				Parallel.ForEach(lines, word =>
				{
					var hash = BrunoHash.GetBrunoHash(Encoding.ASCII.GetBytes(word));
					if (!hashs.Contains(hash))
						hashs.Add(hash);
					else
					{
						var x = hashs[hashs.IndexOf(hash)];
						collisions += 1;
						Console.WriteLine("Collisions found: " + collisions);
					}
				});
				Console.WriteLine("The test was completed, collisions found: " + collisions);

				hashs.Clear();
				var endNumber = 216553;
				Console.WriteLine("Search for collisions in a numerical sequence from 1 to " + endNumber);
				collisions = 0;
				var toExclusive = endNumber + 1;
				Parallel.For(1, toExclusive, n =>
				{
					var hash = BrunoHash.GetBrunoHash(BitConverter.GetBytes(n));
					if (!hashs.Contains(hash))
						hashs.Add(hash);
					else
					{
						var x = hashs[hashs.IndexOf(hash)];
						collisions += 1;
						Console.WriteLine("Collisions found: " + collisions);
					}
				});
				Console.WriteLine("The test was completed, collisions found: " + collisions);

				hashs.Clear();
				Console.WriteLine("Search for collisions in " + endNumber + " GUIDs random");
				collisions = 0;
				Parallel.For(1, toExclusive, n =>
				{
					var hash = BrunoHash.GetBrunoHash(Guid.NewGuid().ToByteArray());
					if (!hashs.Contains(hash))
						hashs.Add(hash);
					else
					{
						var x = hashs[hashs.IndexOf(hash)];
						collisions += 1;
						Console.WriteLine("Collisions found: " + collisions);
					}
				});
				Console.WriteLine("The test was completed, collisions found: " + collisions);
			}

			// Benchmark test:
			// Comparison between SHA256 and BrunoHash256
			Random rnd = new Random();
			byte[] b = new byte[1024 * 1012];
			rnd.NextBytes(b);
			Console.WriteLine("Start speed test SHA256");
			var start = DateTime.Now;
			for (int i = 0; i < 10000; i++)
			{
				var x1 = GetHash(b);
			}
			var totalSeconds1 = (DateTime.Now - start).TotalSeconds;
			Console.WriteLine("The test was completed in " + totalSeconds1 + " sec.");
			Console.WriteLine("Start speed test BrunoHash256");
			start = DateTime.Now;
			for (int i = 0; i < 10000; i++)
			{
				var x1 = BrunoHash.GetBrunoHash(b);
			}
			var totalSeconds2 = (DateTime.Now - start).TotalSeconds;
			Console.WriteLine("The test was completed in " + totalSeconds2 + " sec.");
			Console.WriteLine("The algorithm BrunoHash256 is faster than SHA256 by " + Math.Round(totalSeconds1 / totalSeconds2, 1) + " times");
			Console.WriteLine("Test concluded, press any key");
			Console.ReadKey();
		}

		public static byte[] GetHash(byte[] data)
		{
			System.Security.Cryptography.HashAlgorithm hashType = new System.Security.Cryptography.SHA256Managed();
			return hashType.ComputeHash(data);
		}



	}
}
