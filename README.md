
This API scans through a given webpage, and display a single consolidated top 10 frequent words and the top 10 frequent word pairs (two words in the same order) along with their frequency. In case the webpage contains hyperlinks, these hyperlinked urls need to be expanded and the words on these pages also should be scanned to come up with the frequent words.

This code will travel upto 4 level to get content.

## Solution: ##
Contains web api build with .net core 3.1 

This contains following projects-
1. WebParser.Api - Web api with .net core 3.1. API only creates a job in Queue asynchronously and returns results  
2. WebParser.Api.BackgroundProcessor - this background processor is done with Microsoft.Extensions.Hosting.BackgroundService with queue implementation. This runs as a background process. API layer logs scan item in queue and background processor takes items from queue and performs the scanning
3. WebParser.Api.Common - some common functions
4. WebParser.Api.Scan - Scanning business layer
5. WebParser.Api.Storage - Storage layer for scanning and data. This is implemented with inmemory cache Microsoft.Extensions.Caching.Memory

Todo: Implement unit tests