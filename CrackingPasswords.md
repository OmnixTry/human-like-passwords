# Cracking Passowrds With Hashcat
In this task I've taken hashes from Hmara Vlad and used hashcat to break them. 
I started with md5 hashes as it is supposedly the weakest and the fastest algorythm to crack. 
Especially in this case, where hashes hvae no salt.

## Dictionary Atack
The first and the fastest option is regualr dictionary search. To be more precise I took a list of top million popular passwords and used it as a dictionary. 
This method revealed 40.6% of the passwords wrom the whole list. And it was the fastest way of searching too, as the number of options is very limited. 
It is also reasonable to use this method first, because the percentage of these passwords is the biggest from all the others in the list. 
This will initially narrow down the search, for hashcat which cashes the passwords he successfully cracked.

Comand used for this:
`.\hashcat.exe -m 0 -a 0 md5.csv topMil.txt`

Dictionaty search also allows using rules that allow changing elements of the dictionary but in this situation it wasn't necessary. The work of rules will be demonstrated in the Combination search paragraph.

## Combinator Atack
Another instrument supplied by Hashcat is combination search. It uses 2 dictionaries and tries all the possible combinations of words from the first one to the words with the second one.
It is also possible to add 1 rule to every dictionary (use key `-j` to add rules to every word of the first dictionary. `-k` to do the same with the second one).

A lot of passwords in this set consist of components i can use dictionary for. For example password that consists of Adjective, some delimiter, Human name. There are a few options to solve passwords from this category. It is known that possible delimiters are `., _, -`
1. Use combination atack and append a special symbol to the left dictionary using a rule. The downside that u can append only one symbol at a time so I'll have to run command 3 times(for each symbol).
2. The second option is to create an intermediate dictionary that contains all the words from the first dictionary combined with all the necesary symbols, and than run combination atack on 2 dictionaries.

To Create an intermediate dicionary run command: `.\hashcat.exe -a 1 --stdout .\dictionaries\adjectives.txt .\dictionaries\delimiters.txt >> .\combinations\adjectives-delimiters.txt`

`.\dictionaries\adjectives.txt` is a dictionary with most common adjectives and `.\dictionaries\delimiters.txt` contains all the possible delimiters.

The following command will stert combinator atack on the hashes with 2 dictionaries `.\hashcat.exe -m 0 -a 1 md5.csv .\combinations\adjectives-delimiters.txt .\dictionaries\names.txt >> out-adjdelname-nodictionary.txt`

To demonstrate use of rules in combinator atack here are the commands to crack passwords that consisto of Adjective, delimiter, noun, special symbol. As you can see an intermediate dictionary with adjectives and delimiters can be reused here. All the special symbols are added with a rule to the right rictionary.
- `.\hashcat.exe -m 0 -a 1 md5.csv .\combinations\adjectives-delimiters.txt .\dictionaries\nouns.txt -k '$?' >> adj-noun-spec-symbol.txt`
- `.\hashcat.exe -m 0 -a 1 md5.csv .\combinations\adjectives-delimiters.txt .\dictionaries\nouns.txt -k '$$' >> adj-noun-spec-symbol.txt`
- `.\hashcat.exe -m 0 -a 1 md5.csv .\combinations\adjectives-delimiters.txt .\dictionaries\nouns.txt -k '$!' >> adj-noun-spec-symbol.txt`

All of the passwords that consist of multiple blocks can be with both capital and lowercase first letter. Listed above method will check the lowercased version. To crack Capitalized paswords add `-j 'c'` and run command again. 

## Hybrid Atack
