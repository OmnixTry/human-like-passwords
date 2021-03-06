using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PasswordGenerator
{
	class FileReader
	{
		const string TopMillion = "topMil.txt";
		const string Nouns = "nouns.txt";
		const string Adjectives = "adjectives.txt";
		const string Adwerbs = "2syllableadverbs.txt";


		public readonly string[] topPasswords;
		public readonly string[] topNouns;
		public readonly string[] topAdjectives;
		public readonly string[] topAdwerbs;
		public readonly PairFrequency[] topPairs;
		public readonly PairFrequency[] topTrios;


		public FileReader()
		{
			topPasswords = ReadFile(TopMillion);
			topNouns = ReadFile(Nouns);
			topAdjectives = ReadFile(Adjectives);
			topAdwerbs = ReadFile(Adwerbs).Select(x =>
			{
				int ind = x.IndexOf('\r');
				if (ind != -1) return x.Remove(ind);
				else return x;
			}).ToArray();

			const string filename = "letterFrequency.json";
			string file = File.ReadAllText(filename);
			topPairs = JsonSerializer.Deserialize<List<PairFrequency>>(file).OrderByDescending(x => x.Chance).Take(300).ToArray();

			const string threeFilename = "letterFrequencyThree.json";
			file = File.ReadAllText(threeFilename);
			topTrios = JsonSerializer.Deserialize<List<PairFrequency>>(file).ToArray();
		}

		public string[] ReadTop100()
		{
			return topPasswords.Take(100).ToArray();
		}

		public string[] ReadTopMil()
		{
			return topPasswords;
		}

		private string[] ReadFile(string filename)
		{
			return File.ReadAllText(filename).Split('\n');
		}
	}

	class PairFrequency
	{
		public string Letter { get; set; }

		public double Chance { get; set; }
	}
}
