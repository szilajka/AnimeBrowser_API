 # appsettings.json file contents
 ## ConnectionStrings
 - AnimeBrowser
    - This contains the database connection string
 ## IdentityServerSettings
- AuthorityUrl
    - This is the IdentityServer's url (the base url of the site that contains the discovery document)
- ValidAudiences
    - This is an array.
    - It contains the valid `audiences` that must be accepted if they are in the JWT token's `aud` claim.
    - The Identity Server sends a JWT token with api resources. In [appsettings.development.json](../src/AnimeBrowser.API/appsettings.development.json#L77) there are two valid audiences, those are created in Identity Server, those are the names of api resources that are belongs to the Anime Browser.
