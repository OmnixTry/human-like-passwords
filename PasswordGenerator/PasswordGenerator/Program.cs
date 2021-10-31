using System;

namespace PasswordGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			FileReader fileReader = new FileReader();
			PasswordGenerator passwordGenerator = new PasswordGenerator(fileReader);

			/*Console.WriteLine(passwordGenerator.GenerateSuperCommonPassword());
			Console.WriteLine(passwordGenerator.GenerateRegularCommonPassword());
			Console.WriteLine(passwordGenerator.NounAdj());
			Console.WriteLine(passwordGenerator.AdwNoun());*/
			Console.WriteLine(passwordGenerator.AdwerbAdj());
			Console.WriteLine(passwordGenerator.AdwerbAdj());
			Console.WriteLine(passwordGenerator.AdwerbAdj());
			Console.WriteLine(passwordGenerator.AdwerbAdj());
			Console.WriteLine(passwordGenerator.AdwerbAdj());
			Console.WriteLine(passwordGenerator.AdwerbAdj());
		}
	}
}
