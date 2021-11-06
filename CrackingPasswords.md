# Cracking Passowrds With Hashcat
In this task I've taken hashes from Hmara Vlad and used hashcat to break them. 
I started with md5 hashes as it is supposedly the weakest and the fastest algorythm to crack. 
Especially in this case, where hashes hvae no salt.

## Dictionary search
The first and the fastest option is regualr dictionary search. To be more precise I took a list of top million popular passwords and used it as a dictionary. 
This method revealed 40.6% of the passwords wrom the whole list. And it was the fastest way of searching too, as the number of options is very limited. 
It is also reasonable to use this method first, because the percentage of these passwords is the biggest from all the others in the list. 
This will initially narrow down the search, for hashcat which cashes the passwords he successfully cracked.

Comand used for this:
`.\hashcat.exe -m 0 -a 0 md5.csv topMil.txt`

