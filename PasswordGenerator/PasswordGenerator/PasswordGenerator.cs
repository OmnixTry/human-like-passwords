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

		public int VP = 0;
		public int NVP = 0;
		public int HL = 0;

		enum HumanLikeType
		{
			NounAdj,
			AdwerbAdj,
			AdwNoun
		}

		enum PassType 
		{ 
			VeryPopular,
			RelativelyPopular,
			HumanLikeRandom
		}


		private Dictionary<char, List<char>> LetterSubstitution = new Dictionary<char, List<char>>()
		{
			['a'] = new List<char>(){ '@', '^' },
			['b'] = new List<char>() { '&' },
			['c'] = new List<char>() { '?', '(', '<', '6' },
			['d'] = new List<char>() { '&', '|' },
			['e'] = new List<char>() { '6', '~' },
			['f'] = new List<char>() { '7' },
			['g'] = new List<char>() { '&', '9' },
			['h'] = new List<char>() { '4', '|' },
			['i'] = new List<char>() { '!', '`', '\'', '\\' },
			['j'] = new List<char>() { '/', '\'' },
			['k'] = new List<char>() { '/', '<', '{' },
			['l'] = new List<char>() { '|' },
			['m'] = new List<char>() { '#' },
			['n'] = new List<char>() { '^' },
			['o'] = new List<char>() { '@', '0' },
			['p'] = new List<char>() { '9' },
			['q'] = new List<char>() { '2', '6' },
			['r'] = new List<char>() { '7' },
			['s'] = new List<char>() { '2', '~', '$' },
			['t'] = new List<char>() { '7', '-' },
			['u'] = new List<char>() { '[', ']' },
			['v'] = new List<char>() { '{', '^' },
			['w'] = new List<char>() { '#' },
			['x'] = new List<char>() { '*', '|' },
			['y'] = new List<char>() { '/' },
			['z'] = new List<char>() { ']', '$' },
			[' '] = new List<char>() { '-', '_'}
		};

		public string[] GenerateManyPasswords(int quantity)
		{
			PassType[] probabilityArr = new PassType[100];	
			int veryPopularChance = GetRandomInt(6) + 5;
			int notSoPopularChance = GetRandomInt(41) + 50;
			for (int i = 0; i < veryPopularChance; i++)
			{
				probabilityArr[i] = PassType.VeryPopular;
			}
			for (int i = veryPopularChance; i < veryPopularChance + notSoPopularChance; i++)
			{
				probabilityArr[i] = PassType.RelativelyPopular;
			}
			for(int i = veryPopularChance + notSoPopularChance; i < probabilityArr.Length; i++)
			{
				probabilityArr[i] = PassType.HumanLikeRandom;
			}

			string[] passwords = new string[quantity];
			for (int i = 0; i < quantity; i++)
			{
				int rand = GetRandomInt(100);
				switch (probabilityArr[rand])
				{
					case PassType.VeryPopular:
						passwords[i] = GenerateSuperCommonPassword();
						break;
					case PassType.RelativelyPopular:
						passwords[i] = GenerateRegularCommonPassword();
						break;
					case PassType.HumanLikeRandom:
						passwords[i] = GenerateHumanLike();
						break;
				}
			}
			return passwords;
		}
		public PasswordGenerator(FileReader fileReader)
		{
			this.fileReader = fileReader;
			random = RandomNumberGenerator.Create();
		}

		public string GenerateSuperCommonPassword()
		{
			VP++;
			var top = fileReader.ReadTop100();
			var index = GetRandomInt(top.Length);
			return top[index];
		}

		public string GenerateRegularCommonPassword()
		{
			NVP++;
			var top = fileReader.ReadTopMil();
			var index = GetRandomInt(top.Length);
			return top[index];
		}

		public string GenerateHumanLike()
		{
			HL++;
			var type = (HumanLikeType)GetRandomInt(3);
			switch (type)
			{
				case HumanLikeType.NounAdj:
					return NounAdj();
				case HumanLikeType.AdwerbAdj:
					return AdwerbAdj();
				case HumanLikeType.AdwNoun:
					return AdwNoun();
				default:
					return "";
			}
		}

		private string TwoWordPass(string first, string second)
		{
			StringBuilder pass = new StringBuilder(first);
			char? separator = UseSeparator();
			if (separator.HasValue)
			{
				pass.Append(separator.Value);
			}
			pass.Append(second);

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
			int numberOfSpecialCharacters = length / 6;
			for(int i = 0; i < numberOfSpecialCharacters; i++)
			{
				int letterNumber;
				do
				{
					letterNumber = GetRandomInt(length);
				} while (!Char.IsLetter(stringBuilder[letterNumber]));

				try
				{
					var possibleSubstitutions = LetterSubstitution[Char.ToLower(stringBuilder[letterNumber])];
					int subNum = possibleSubstitutions.Count();
					stringBuilder[letterNumber] = possibleSubstitutions[GetRandomInt(subNum)];
				}
				catch(Exception e) { }				
			}
			return stringBuilder;
		}

		public string NounAdj()
		{
			string noun = RandomNoun();
			string adj = RandomAdj();
			return TwoWordPass(noun, adj);
		}

		public string AdwerbAdj()
		{
			string adw = RandomAdwerb();
			string adj = RandomAdj();
			return TwoWordPass(adw, adj);
		}

		public string AdwNoun()
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
			int i = Math.Abs(BitConverter.ToInt32(rand, 0));

			int result = i;
			if (max != 0) {
				result = i % max;
			}
			return result;
		}

	}
}
