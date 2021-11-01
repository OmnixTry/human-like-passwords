using System;

namespace PasswordGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			FileReader fileReader = new FileReader();
			PasswordGenerator passwordGenerator = new PasswordGenerator(fileReader);



			string[] pass = passwordGenerator.GenerateManyPasswords(100);
			Console.WriteLine(passwordGenerator.VP);
			Console.WriteLine(passwordGenerator.NVP);
			Console.WriteLine(passwordGenerator.HL);

			foreach (var item in pass)
			{
				Console.WriteLine(item);
			}

		}
	}
}
