using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator
{
	class PasswordGenerator
	{
		private readonly FileReader fileReader;

		public PasswordGenerator(FileReader fileReader)
		{
			this.fileReader = fileReader;
		}

		public string GenerateSuperCommonPassword()
		{
			var top = fileReader.ReadTop100();
			Random rand = new Random(top.Length);
			return top[rand.Next()];
		}

		public string GenerateRegularCommonPassword()
		{
			var top = fileReader.ReadTopMil();
			Random rand = new Random(top.Length);
			return top[rand.Next()];
		}


	}
}
