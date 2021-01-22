# Basic Concepts
In the Anime Browser API the Anime object may be different than what it should be. Let me explain the difference

## Anime
In the API I use the word `anime` as a container. The Anime object contains basic informations about the real anime, ie:
- Title
- Description
- Start and End date
...
It has no ratings, it functions only as a container. If you search for anything, you have to add a flag that adds the Anime objects to the result.
If you don't add that flag, it will only search in Seasons.
Ie: The results for __JoJo__ should be the following:
- JoJo's Bizarre Adventure (2012)
- JoJo's Bizarre Adventure: Stardust Crusaders
- JoJo's Bizarre Adventure: Stardust Crusaders - Battle in Egypt
- JoJo's Bizarre Adventure: Diamond is Unbreakable
- JoJo's Bizarre Adventure: Golden Wind
And if you include the Anime, it would add the basic __JoJo's Bizarre Adventure__ anime, that contains the start date of _Phantom Blood_ and the end date of _Golden Wind_.
So the _JoJo's Bizarre Adventure_'s description should contains that 
> The series are about the Joestar family and the seasons follow the family member's life...

## Season
The Season object contains one anime season.
It contains:
- Number of the selected season
- Description
- Season Title
- Number of episodes
...
In search results seasons are included and they are acting as a real anime, except that these are just a season of the anime.