using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator
{
	class PasswordGenerator
	{
		private readonly FileReader fileReader;
		private readonly RandomNumberGenerator random;

		private Dictionary<char, List<char>> LetterSubstitution = new Dictionary<char, List<char>>()
		{
			['a'] = { '@', '^' },
			['b'] = { '&' },
			['c'] = { '?', '(', '<', '6' },
			['d'] = { '&', '|' },
			['e'] = { '6', '~' },
			['f'] = { '7' },
			['g'] = { '&', '9' },
			['h'] = { '4', '|' },
			['i'] = { '!', '`', '\'', '\\' },
			['j'] = { '/', '\'' },
			['k'] = { '/', '<', '{' },
			['l'] = { '|' },
			['m'] = { '#' },
			['n'] = { '^' },
			['o'] = { '@', '0' },
			['p'] = { '9' },
			['q'] = { '2', '6' },
			['r'] = { '7' },
			['s'] = { '2', '~', '$' },
			['t'] = { '7', '-' },
			['u'] = { '[', ']' },
			['v'] = { '{', '^' },
			['w'] = { '#' },
			['x'] = { '*', '|' },
			['y'] = { '/' },
			['z'] = { ']', '$' },
			[' '] = { '-', '_'}
		};

		public PasswordGenerator(FileReader fileReader)
		{
			this.fileReader = fileReader;
			random = RandomNumberGenerator.Create();
		}

		public string GenerateSuperCommonPassword()
		{
			var top = fileReader.ReadTop100();
			return top[GetRandomInt(top.Length)];
		}

		public string GenerateRegularCommonPassword()
		{
			var top = fileReader.ReadTopMil();
			Random rand = new Random(top.Length);
			return top[GetRandomInt(top.Length)];
		}

		public string GenerateHumanLike()
		{
			int numOfAdj = fileReader.topAdjectives.Length;
			string adjective = fileReader.topAdjectives[GetRandomInt(numOfAdj)];

			return "";
		}

		private string TwoWordPass(string first, string second)
		{
			StringBuilder pass = new StringBuilder(first);
			char? separator = UseSeparator();
			if (separator.HasValue)
			{
				pass.Append(separator.Value);
			}

			return SubstituteSymbols(pass).ToString();

			char? UseSeparator()
			{
				char? separator = null;
				int rand = GetRandomInt(3);
				if (rand == 0)
					separator = '-';
				else if (rand == 1)
					separator = '_';
				return separator;
			}
		}

		private StringBuilder SubstituteSymbols(StringBuilder stringBuilder)
		{
			int length = stringBuilder.Length;
			while(GetRandomInt(length) < length / 5)
			{
				int letterNumber;
				do
				{
					letterNumber = GetRandomInt(length);
				} while (!Char.IsLetter(stringBuilder[letterNumber]));
				var possibleSubstitutions = LetterSubstitution[stringBuilder[letterNumber]];
				int subNum = possibleSubstitutions.Count();
				stringBuilder[letterNumber] = possibleSubstitutions[GetRandomInt(subNum)];
			}
			return stringBuilder;
		}

		private string NounAdj()
		{
			string noun = RandomNoun();
			string adj = RandomAdj();
			return TwoWordPass(noun, adj);
		}

		private string AdwerbAdj()
		{
			string adw = RandomAdwerb();
			string adj = RandomAdj();
			return TwoWordPass(adw, adj);
		}

		private string AdwNoun()
		{
			string adw = RandomAdwerb();
			string noun = RandomNoun();
			return TwoWordPass(noun, adw);
		}

		private string RandomNoun()
		{
			int num = fileReader.topNouns.Length;
			string word = fileReader.topNouns[GetRandomInt(num)];
			return word;
		}

		private string RandomAdwerb()
		{
			int num = fileReader.topAdwerbs.Length;
			string word = fileReader.topAdwerbs[GetRandomInt(num)];
			return word;
		}

		private string RandomAdj()
		{
			int numOfAdj = fileReader.topAdjectives.Length;
			string word = fileReader.topAdjectives[GetRandomInt(numOfAdj)];
			return word;
		}

		private int GetRandomInt(int max = 0)
		{
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			byte[] rand = new byte[4];
			rng.GetBytes(rand);
			int i = BitConverter.ToInt32(rand, 0);

			int result = i;
			if (max != 0) {
				result = i % max;
			}
			return result;
		}

	}
}
