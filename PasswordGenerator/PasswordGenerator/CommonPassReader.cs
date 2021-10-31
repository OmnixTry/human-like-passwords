using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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


		public FileReader()
		{
			topPasswords = ReadFile(TopMillion);
			topNouns = ReadFile(Nouns);
			topAdjectives = ReadFile(Adjectives);
			topAdwerbs = ReadFile(Adwerbs);
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
}
