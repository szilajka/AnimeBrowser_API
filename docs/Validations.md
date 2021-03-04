# Validations

This document contains validation rules applied to models.

__Table of Contents__
- [Genre](#Genre)
    - [Creation validation rules](#Genre-Creation-validation-rules)
    - [Editing validation rules](#Genre-Editing-validation-rules)
    - [Delete validation rules](#Genre-Delete-validation-rules)
- [Anime Info](#Anime-Info)
    - [Creation validation rules](#Anime-Info-Creation-validation-rules)
    - [Editing validation rules](#Anime-Info-Editing-validation-rules)
    - [Delete validation rules](#Anime-Info-Delete-validation-rules)
- [Season](#Season)
    - [Creation validation rules](#Season-Creation-validation-rules)
    - [Editing validation rules](#Season-Editing-validation-rules)
    - [Delete validation rules](#Season-Delete-validation-rules)
- [Episode](#Episode)
    - [Creation validation rules](#Episode-Creation-validation-rules)
    - [Editing validation rules](#Episode-Editing-validation-rules)
    - [Delete validation rules](#Episode-Delete-validation-rules)


## Genre
### Genre Creation validation rules

- Model is not null
    - If null: throws `EmptyObjectException<GenreCreationRequestModel>`
- Validator: 
    - Trimmed __GenreName__ is not empty
    - Trimmed __GenreName__'s length <= 100 characters
    - Trimmed __Description__ is not empty
    - Trimmed __Description__'s length <= 10.000 characters
    - If any: throws `ValidationException`
- Searches for existing object with same __GenreName__
    - If found: throw `AlreadyExistingObjectException<Genre>`

### Genre Editing validation rules

- Given id and model's id must be the same
    - If not equals: throws `MismatchingIdException`
- Validator:
    - Model not null
    - __Id__ > 0
    - Trimmed __GenreName__ is not empty
    - Trimmed __GenreName__'s length <= 100
    - Trimmed __Description__ is not empty
    - Trimmed __Description__'s length <= 10.000 characters
    - If any: throws `ValidationException`
- Searches for existing objects that have the same __GenreName__ (of course, itself is not included)
    - If found: throw `AlreadyExistingObjectException<Genre>`
- Searches for existing object with same __Id__
    - If no object is found: throws `NotFoundObjectException<Genre>`

### Genre Delete validation rules

- __Id__ > 0
    - If <= 0: throws `NotExistingIdException`
- Searches for existing object with given id
    - If no object is found: throws `NotFoundObjectException<Genre>`



## Anime Info
### Anime Info Creation validation rules

- Model is not null
    - If null: throws `EmptyObjectException<AnimeInfoCreationRequestModel>`
- Validator: 
    - Trimmed __Title__ is not empty
    - Trimmed __Title__'s length <= 255 characters
    - Trimmed __Description__'s length <= 30.000 characters
    - If any: throws `ValidationException`

### Anime Info Editing validation rules

- Given id and model's id must be the same
    - If not equals: throws `MismatchingIdException`
- Validator:
    - Model not null
    - __Id__ > 0
    - Trimmed __Title__ is not empty
    - Trimmed __Title__'s length <= 255
    - Trimmed __Description__'s length <= 30.000
    - If any: throws `ValidationException`
- Searches for existing object with given id
    - If no object is found: throws `NotFoundObjectException<AnimeInfo>`

### Anime Info Delete validation rules

- __Id__ > 0
    - If <= 0: throws `NotExistingIdException`
- Searches for existing object with given id
    - If no object is found: throws `NotFoundObjectException<AnimeInfo>`



## Season
### Season Creation validation rules

Minimum date: right now it is `1900-01-01T00:00Z`
Maximum date: today + 10 years (in UTC)
- Model is not null
    - If null: throws `EmptyObjectException<SeasonCreationRequestModel>`
- Validator: 
    - __SeasonNumber__ not null
    - __SeasonNumber__ > 0
    - Trimmed __Title__ is not empty
    - Trimmed __Title__'s length <= 255 characters
    - Trimmed __Description__'s length <= 30.000 characters
    - If __NumberOfEpisodes__ has value, then __NumberOfEpisodes__ > 0
    - If __CoverCarousel__ not null, then __CoverCarousel__ not empty
    - If __Cover__ not null, then __Cover__ not empty
    - __AnimeInfoId__ not null
    - __AnimeInfoId__ > 0
    - __AirStatus__ has a valid enum value
    - If __AirStatus__ == Airing, then
        - __StartDate__ is not null
            - If __StartDate__ has value, then it must be between the minimum and maximum date
        - If __EndDate__ and __StartDate__ has value, 
            - Then __EndDate__ must be >= __StartDate__
            - Then __EndDate__ must be <= maximum date
    - If __AirStatus__ == Aired
        - __StartDate__ is not null
            - If __StartDate__ has value, then it must be between the minimum and maximum date
        - __EndDate__ is not null
        - If __EndDate__ and __StartDate__ has value, 
            - Then __EndDate__ must be >= __StartDate__
            - Then __EndDate__ must be <= maximum date
    - If __StartDate__ has value _AND_ __AirStatus__ != Airing _AND_ __AirStatus__ != Aired
        - __StartDate__ must be between minimum and maximum date
    - If __EndDate__ has value _AND_ __AirStatus__ != Airing _AND_ __AirStatus__ != Aired
        - __StartDate__ must have a value
    - If __StartDate__ has value _AND_ __EndDate__ has value _AND_ __AirStatus__ != Airing _AND_ __AirStatus__ != Aired
        - __EndDate__ must be >= __StartDate__
        - __EndDate__ must be <= maximum date
    - If any: throws `ValidationException`
- Searches for existing objects that have the same __AnimeInfoId__ and __SeasonNumber__
    - If found any: throws `AlreadyExistingObjectException<Season>`
- Searches for existing object with given __AnimeInfoId__
    - If not found any: throws `NotFoundObjectException<AnimeInfo>`

### Season Editing validation rules

Minimum date: right now it is `1900-01-01T00:00Z`
Maximum date: today + 10 years (in UTC)
- Given id and model's id must be the same
    - If not equals: throws `MismatchingIdException`
- Validator:
    - Model not null
    - If Model not null, then
        - __Id__ > 0
        - __SeasonNumber__ not null
        - __SeasonNumber__ > 0
        - Trimmed __Title__ is not empty
        - Trimmed __Title__'s length <= 255 characters
        - Trimmed __Description__'s length <= 30.000 characters
        - If __NumberOfEpisodes__ has value, then __NumberOfEpisodes__ > 0
        - If __CoverCarousel__ not null, then __CoverCarousel__ not empty
        - If __Cover__ not null, then __Cover__ not empty
        - __AnimeInfoId__ not null
        - __AnimeInfoId__ > 0
        - __AirStatus__ has a valid enum value
        - If __AirStatus__ == Airing, then
            - __StartDate__ is not null
                - If __StartDate__ has value, then it must be between the minimum and maximum date
            - If __EndDate__ and __StartDate__ has value, 
                - Then __EndDate__ must be >= __StartDate__
                - Then __EndDate__ must be <= maximum date
        - If __AirStatus__ == Aired
            - __StartDate__ is not null
                - If __StartDate__ has value, then it must be between the minimum and maximum date
            - __EndDate__ is not null
            - If __EndDate__ and __StartDate__ has value, 
                - Then __EndDate__ must be >= __StartDate__
                - Then __EndDate__ must be <= maximum date
        - If __StartDate__ has value _AND_ __AirStatus__ != Airing _AND_ __AirStatus__ != Aired
            - __StartDate__ must be between minimum and maximum date
        - If __EndDate__ has value _AND_ __AirStatus__ != Airing _AND_ __AirStatus__ != Aired
            - __StartDate__ must have a value
        - If __StartDate__ has value _AND_ __EndDate__ has value _AND_ __AirStatus__ != Airing _AND_ __AirStatus__ != Aired
            - __EndDate__ must be >= __StartDate__
            - __EndDate__ must be <= maximum date
    - If any: throws `ValidationException`
- Searches for existing objects that have the same __AnimeInfoId__ and __SeasonNumber__ (of course, itself is not included)
    - If found any: throws `AlreadyExistingObjectException<Season>`
- Searches for existing object with given __Id__
    - If not found: throws `NotFoundObjectException<Season>`
- Searches for existing object with given __AnimeInfoId__
    - If not found any: throws `NotFoundObjectException<AnimeInfo>`

### Season Delete validation rules

- __Id__ > 0
    - If <= 0: throws `NotExistingIdException`
- Searches for existing object with given __Id__
    - If no object is found: throws `NotFoundObjectException<Season>`


## Episode
### Episode Creation validation rules

Minimum date: If the found Season's __StartDate__ has value, then __StartDate__, otherwise `1900-01-01T00:00Z`
Maximum date: If the found Season's __EndDate__ has value, then __EndDate__, otherwise (now + 10 years in UTC)
- Model is not null
    - If null: throws `EmptyObjectException<EpisodeCreationRequestModel>`
- Searches for existing object with given __SeasonId__
    - If not found: throws `NotFoundObjectException<Season>`
- Found Season's __AnimeInfoId__ must be the same as the request model's ___AnimeInfoId__
    - If not equals: throws `MismatchingIdException`
- Validator: 
    - __EpisodeNumber__ > 0
    - __AirStatus__ must have a valid enum value
    - If __Title__' is not empty, then trimmed __Title__'s length <= 255 characters
    - If __Description__ is not empty, then trimmed __Description__'s length <= 30.000 characters
    - If __AirStatus__ == Airing, then
        - __AirDate__ not empty
        - If __AirDate__ has value, then __AirDate__ must be between (today - 2 days) and (today + 2 days)
    - If __AirStatus__ == Aired, then
        - __AirDate__ not empty
        - If __AirDate__ has value, then __AirDate__ must be between minimum and maximum date
    - If __AirDate__ has value _AND_ __AirStatus__ != Airing _AND_ __AirStatus__ != Aired
        - Then __AirDate__ must be between minimum and maximum date
    - __Cover__ not empty
    - If any: throws `ValidationException`
- Searches for existing object that have the same __SeasonId__ and __EpisodeNumber__
    - If found any: throws `AlreadyExistingObjectException<Episode>`

### Episode Editing validation rules

Minimum date: If the found Season's __StartDate__ has value, then __StartDate__, otherwise `1900-01-01T00:00Z`
Maximum date: If the found Season's __EndDate__ has value, then __EndDate__, otherwise (now + 10 years in UTC)
- Given id and model's id must be the same
    - If not equals: throws `MismatchingIdException`
- Searches for existing object with given __SeasonId__
    - If not found: throws `NotFoundObjectException<Season>`
- Found Season's __AnimeInfoId__ must be the same as the request model's ___AnimeInfoId__
    - If not equals: throws `MismatchingIdException`
- Validator: 
    - __Id__ > 0
    - __EpisodeNumber__ > 0
    - __AirStatus__ must have a valid enum value
    - If __Title__' is not empty, then trimmed __Title__'s length <= 255 characters
    - If __Description__ is not empty, then trimmed __Description__'s length <= 30.000 characters
    - If __AirStatus__ == Airing, then
        - __AirDate__ not empty
        - If __AirDate__ has value, then __AirDate__ must be between (today - 2 days) and (today + 2 days)
    - If __AirStatus__ == Aired, then
        - __AirDate__ not empty
        - If __AirDate__ has value, then __AirDate__ must be between minimum and maximum date
    - If __AirDate__ has value _AND_ __AirStatus__ != Airing _AND_ __AirStatus__ != Aired
        - Then __AirDate__ must be between minimum and maximum date
    - __Cover__ not empty
    - If any: throws `ValidationException`
- Searches for existing object with given __Id__
    - If not foun: throws `NotFoundObjectException<Episode>`
- Searches for existing object that have the same __SeasonId__ and __EpisodeNumber__
    - If found any: throws `AlreadyExistingObjectException<Episode>`

### Episode Delete validation rules

- __Id__ > 0
    - If <= 0: throws `NotExistingIdException`
- Searches for existing object with given __Id__
    - If no object is found: throws `NotFoundObjectException<Episode>`
