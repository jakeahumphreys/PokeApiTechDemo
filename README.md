# PokéApi Tool

<div align="center">
<img src="https://i.imgur.com/rf6WdtT.png" alt="Markdown Monster icon" style="margin-right: 10px; border-style: solid; margin-bottom: 20px;" width="400" height="350"/>
</div>

# Background

I wrote this application as a small demonstration for an interview for Radius Payment Solutions to act as a brief showcase of some of the skills I have. Naturally it doesn't cover everything, but I've aimed to tick a few things of the job requirements list. Namely:

- .Net 4.7 (Although this is written in 4.8).
- C#.
- SQL Knowledge.
    - Knowledge of relational databases.
- REST.

Following the initial interview I continued to develop the application, added some polish and released a 1.0.


# What does this application do?
This application takes the search text you use and makes a web request to the PokeAPI (https://pokeapi.co/). Strictly speaking this only makes GET requests, but it's a fun little tool nonetheless. Read on for some of the ins and outs.

It's written in .NET Framework 4.8, for the simple reason that I *love* Rider by JetBrains, and they've not given us Winforms previewing for .NET Core (6.0). This could however, be rewritten in Core by using WPF

## Data Access
This application has a small SQLite cache built in, the idea being we can prevent calls to the web API if you've already retrieved the data recently. In this case if the data is in cache, and it's less than 10 minutes old, it'll display that data instead

In this example all the data access is provided using raw SQL. This is to demonstrate an ability to put together queries in a concise and safe manner. Every query uses a **prepared statement** making the queries safe in that it protects against injection, but also: easy to locate and edit! (Amazing if another developer needs to change the query).

The data logic is stored in a repository class, this means I can implement an interface here and easily unit test the business logic in the service class higher up if necessary. It also means I could go a little further and look to implement the repository pattern, but as this is a single data table for a small cache, it's not strictly needed here.

~~I've also added a simple settings store, technically windows forms application can store settings internally in a file alongside the exe but to demonstrate knowledge of SQL I've made this a table. This is more applicable if the application was powered off the web or the data stored off-machine.~~
(Settings store was fairly useless so I swapped it back out for a settings file).

The Data Access logic is also unit tested, because why not.

## Planned Features
> ❗There's no certainty as to if or when these features will be implemented.

- Settings menu, currently they're hidden and only edited at the development stage.
  - Toggleable debug logs.
  - Edit Shiny Chance.
  - Edit Cache grace period.


- Detailed view of Pokémon, currently it's fairly barebones.
- Type effectiveness matrix or some way to show which pokémon types they're effective against or weak to.

## Improvements

No application is perfect, especially this one as it was written fairly quickly to highlight a skillset. Here's some ways it can be improved further:

- For persistent data I'd like to use Postgres. Primarily because postgres has baked in datetime and JSON blob support.
	- Let's go further! Postgres could handle a larger scale cache to exist more of an audit and then a Redis layer could be implemented on top to allow super fast cache retrieval.
- SPA - This could easily be implemented as an SPA, currently as a windows application it's rather limited but rewriting it as an SPA would make the application work on any number of platforms. It could even be wrapped with an electron shell and run on the desktop that way.

~~I'd also eventually make this application more self sufficient, currently if you were to download it without the DB, it makes no attempt to validate and rectify any errors with the data structure. This would be easily solved by making the application check the required files / tables are present at load, and seed them if not.~~
(This was implemented in [this](https://github.com/jakeahumphreys/PokeApiTechDemo/commit/cf798104e49cbe6b50a0d26e9e8bf086e32aca7d) commit.)
