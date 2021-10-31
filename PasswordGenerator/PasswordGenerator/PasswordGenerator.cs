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
			['z'] = { ']', '$' }
		};


		public PasswordGenerator(FileReader fileReader)
		{
			this.fileReader = fileReader;
			random = RandomNumberGenerator.Create();
		}

		public string GenerateSuperCommonPassword()
		{
			var top = fileReader.ReadTop100();
			return top[GetRandomInt(100)];
		}

		public string GenerateRegularCommonPassword()
		{
			var top = fileReader.ReadTopMil();
			Random rand = new Random(top.Length);
			return top[GetRandomInt(1000000)];
		}

		public string GenerateHumanLike()
		{
			return "";
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
