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
		
		private string[] topPasswords;
		private string[] topNouns;
		private string[] topAdjectives;


		public FileReader()
		{
			topPasswords = ReadFile(TopMillion);
			topNouns = ReadFile(Nouns);
			topAdjectives = ReadFile(Adjectives);
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
