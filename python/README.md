Fortune Teller API
============

Fortune Teller uses AI to generate personalized mystical fortunes across multiple categories including love, career, health, wealth, and travel.

![Build Status](https://img.shields.io/badge/build-passing-green)
![Code Climate](https://img.shields.io/badge/maintainability-B-purple)
![Prod Ready](https://img.shields.io/badge/production-ready-blue)

This is a Python API Wrapper for the [Fortune Teller API](https://apiverve.com/marketplace/api/fortuneteller)

---

## Installation
	pip install apiverve-fortuneteller

---

## Configuration

Before using the fortuneteller API client, you have to setup your account and obtain your API Key.  
You can get it by signing up at [https://apiverve.com](https://apiverve.com)

---

## Usage

The Fortune Teller API documentation is found here: [https://docs.apiverve.com/api/fortuneteller](https://docs.apiverve.com/api/fortuneteller).  
You can find parameters, example responses, and status codes documented here.

### Setup

```
# Import the client module
from apiverve_fortuneteller.apiClient import FortunetellerAPIClient

# Initialize the client with your APIVerve API key
api = FortunetellerAPIClient("[YOUR_API_KEY]")
```

---


### Perform Request
Using the API client, you can perform requests to the API.

###### Define Query

```
query = { "category": "general", "question": "What does the future hold for me?" }
```

###### Simple Request

```
# Make a request to the API
result = api.execute(query)

# Print the result
print(result)
```

###### Example Response

```
{
  "status": "ok",
  "error": null,
  "data": {
    "fortune": "The threads of destiny are aligning, weaving a tapestry of unexpected opportunities. Embrace the whispers of change, for they carry the keys to unlocking your hidden potential.",
    "insight": "Trust your intuition; it is the compass guiding you toward your truest self.",
    "timeframe": "in the coming weeks",
    "category": "general",
    "question": null,
    "luckyNumbers": [
      69,
      94,
      18,
      63,
      41,
      62
    ],
    "luckyElement": "Fire",
    "luckyColor": "Purple",
    "luckyDay": "Friday",
    "timestamp": "2025-11-30T10:24:58.097Z"
  }
}
```

---

## Customer Support

Need any assistance? [Get in touch with Customer Support](https://apiverve.com/contact).

---

## Updates
Stay up to date by following [@apiverveHQ](https://twitter.com/apiverveHQ) on Twitter.

---

## Legal

All usage of the APIVerve website, API, and services is subject to the [APIVerve Terms of Service](https://apiverve.com/terms) and all legal documents and agreements.

---

## License
Licensed under the The MIT License (MIT)

Copyright (&copy;) 2025 APIVerve, and EvlarSoft LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.