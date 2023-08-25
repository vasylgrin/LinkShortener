At the moment, to use the link shortener, you need to launch WebApi and enter the link in Swager. Paste the resulting link into the browser and that's it.

The algorithm of the link shortener is as follows:
1. Get a link to shorten.
2. Create a unique link(see below).
3. Store the incoming and short links in the database.
4. Create an endpoint for all unprocessed requests.
5. Configure the endpoint to search for a link in the database.
6. If the link is found, we make a redirect.

Algorithm for creating a short link:
1. Create a character string that will be used to generate links.
2. Create a randomizer and a cycle in which the generation will take place.
3. In the loop, take the generated number and get the character from the string by index and repeat until the link is generated.
4. To generate a unique link, another loop is used in which a search is made in the database to see if such a link already exists.
5. if the uniqueness of the link is achieved, we give it to the user.
