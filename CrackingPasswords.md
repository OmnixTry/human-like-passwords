# Cracking Passowrds With Hashcat
In this task I've taken hashes from Hmara Vlad and used hashcat to break them. 
I started with md5 hashes as it is supposedly the weakest and the fastest algorythm to crack. 
Especially in this case, where hashes hvae no salt.

## Dictionary Atack
The first and the fastest option is regualr dictionary search. To be more precise I took a list of top million popular passwords and used it as a dictionary. 
This method revealed 40.6% of the passwords wrom the whole list. And it was the fastest way of searching too, as the number of options is very limited. 
Dictionary atack turned out to be the fastest from all I tried. It didn't take more than half a minute to crack a top million popular passwords.
So reasonable to use this method first, because the percentage of these passwords is the biggest from all the others in the list. 
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
As for the fime spent this was a little longer than a dictionay search, which is reasonable, because the number of variants is much bigger but it still took about 1 to 3 minutes
to compute.

## Hybrid Atack
Some of the passwords in this set of hashes may have several symbols possible for one position in password. For example passwords that consist of noun, recent date and special symbol. Generating a set of dates for this password is not possible with regular dictionary rules. But making a bruteforce atack is very wasteful. For this reason i cracked passwords like this using Hybrid Atack.
This type of atack uses dictionary and adds pasks to the and or beginning of dictionaty element. This helps saving time because list of popular nouns can be used as a dictionary, which is faster than bruteforcing.

Time to build the mask. It ocnsists of 8 decimad digits and a special symbol. Generally a mask `?d?d?d?d?d?d?d?d?s` would work. BUT it is too general. It will take a lot of time to try all the combinations (Trust me. I tried). So it's necessary to narrow the selection down. Hashcat supplies users with Alphabets we can use in masks. For example alphabet `d` used in the mask above consists of all the decimal digits. But in some places of date not all the digits are possible. For this situation i used custom alphabets. It's possible to use up to 4 alphabets at the same time in hashcat. They are declared using `-1` `-2` `-3` `-4` commands sublying set of characters after a command. 

I used the following command to break these types of passwords:
`.\hashcat.exe -m 0 -a 6 -1 .\CustomCharsets\specialCharacters.txt -2 0123 -3 09 -4 012789  md5.csv .\dictionaries\nouns.txt ?2?d?2?d?2?3?4?d?1  >> out-noun-date-symbol.txt`
- `-1` references a file that contains only the possible special symbols: `@!$`. (The default special symbol alphabet of hashcat is too wide for our purpose).
- `-2` defines a set of characters suitable for first digit of day, month and year. It is a bit wider than nexessary for the first digit of year and month,  but the amount of custom charactersets is limited, so there had to be a compromise.
- `-3` `-4` is used for second and third digits of the year respectively. It was possible to narrow down this part as we know a boundary of possible date.

This was a very timeconsuming atack. Without narrowing down the mask option it took more than 15 minutes to compute. (I can't tell for sure. I aborted and then ran it with custom alphabets). However if the mask is reasonable and doesn't give very inappropriate results it is possible to solve task in 3-5 minutes.

## Bute Force
This method is the most time consuming and the least effective. Hence I didn't rely on it heavily. It worked for several passwords from completely random category, but over an hour of work on my hardware it chacked only about 1 percent of the passwords.

# Results
Using all methods mentioned above i managed to chack 72.44% of all passwords. 
- The biggest amount was chacked with dictionary atack with top Million most popular passwords (about 40.6%). Those passwords ware the most numerous. Combined with the fastest type atack it was the fastest progress throughout the whole exercise.
- Combinator atack, being pretty similar to dictionary was useful too, but it had a 2 little downsides. Having big dictionaries makes the process a bit more lengthy. And usage of rules in this type of atack is very limited which reduces flexibility.
- The most difficult in use were hybrid and bruteforce atacks. They give a lot of freedom but without a really good mask they don't give any acceptable result. Hybrid atack may be chosen instead of dictionary because masks can generate more options than dictionary rules (when it comes to picking number at the end of a word for example), while taking reasonable time.

# Recomendations
- Old hashing schemes should not be used for passowrds at all. They work fast, anough to make password cracking very easy.
- Adding Salt slows down the process of cracking passwords, which makes it **necessary** for safety of your passwords. (Atacking passwords without salt was very fast on my hardware, which is not top tier, honestly)
- Hashing algorythms that take more time should be preferred, as it makes password cracking way less efective. (Cracking passwords hashed with bcrypt was insanely long compared to md5 and salted sha1.)
- Using names surnames or dates is a very bad strategy for a password. This kind of passwords are easy to crack with simple types of atack like dictionary or combinator, that are among fastest. If possible these passwords should be forbidden to use.
- Any use of a lexical word makes password voulnerable to hybrid atack. Avoid using complete words. Substituting words with symbols might help a bit but those kinds of passwors can be cracked with dictionary rules.
- It's best to use the salt generated by libraries that provide the hashing algorythm itself. It should be random enough, othervise it's useles.
