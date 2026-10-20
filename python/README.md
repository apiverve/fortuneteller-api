Fortune Teller API
============

Fortune Teller uses AI to generate personalized mystical fortunes across multiple categories including love, career, health, wealth, and travel.


[![PyPI version](https://img.shields.io/pypi/v/apiverve-fortuneteller.svg)](https://pypi.org/project/apiverve-fortuneteller/)

This is a Python wrapper for the [Fortune Teller API](https://apiverve.com/marketplace/fortuneteller?utm_source=pypi&utm_medium=referral&utm_content=readme)

---

## Installation

Using `pip`:

```bash
pip install apiverve-fortuneteller
```

Using `pip3`:

```bash
pip3 install apiverve-fortuneteller
```

---

## Configuration

Before using the Fortune Teller API client, you have to setup your account and obtain your API Key.
You can get it by signing up at [https://apiverve.com](https://apiverve.com?utm_source=pypi&utm_medium=referral&utm_content=readme)

---

## Quick Start

Here's a simple example to get you started quickly:

```python
from apiverve_fortuneteller.apiClient import FortunetellerAPIClient

# Initialize the client with your APIVerve API key
api = FortunetellerAPIClient("[YOUR_API_KEY]")

query = {
    "category": "general",
    "question": "What does the future hold for me?"
}

try:
    # Make the API call
    result = api.execute(query)

    # Print the result
    print(result)
except Exception as e:
    print(f"Error: {e}")
```

---

## Usage

The Fortune Teller API documentation is found here: [https://docs.apiverve.com/ref/fortuneteller](https://docs.apiverve.com/ref/fortuneteller?utm_source=pypi&utm_medium=referral&utm_content=readme).
You can find parameters, example responses, and status codes documented here.

### Setup

```python
# Import the client module
from apiverve_fortuneteller.apiClient import FortunetellerAPIClient

# Initialize the client with your APIVerve API key
api = FortunetellerAPIClient("[YOUR_API_KEY]")
```

---

## Perform Request

Using the API client, you can perform requests to the API.

###### Define Query

```python
query = {
    "category": "general",
    "question": "What does the future hold for me?"
}
```

###### Simple Request

```python
# Make a request to the API
result = api.execute(query)

# Print the result
print(result)
```

###### Example Response

```json
{
  "status": "ok",
  "error": null,
  "data": {
    "fortune": "The threads of destiny are weaving a tapestry of unexpected joy; embrace the unknown, for within its depths lie blossoms of serendipity. A golden opportunity awaits, carried upon the wings of synchronicity.",
    "insight": "Trust in the whispers of your intuition; it shall guide you through the maze of life.",
    "timeframe": "in the coming weeks",
    "category": "general",
    "question": null,
    "luckyNumbers": [
      87,
      10,
      16,
      83,
      99,
      34
    ],
    "luckyElement": "Wood",
    "luckyColor": "Purple",
    "luckyDay": "Tuesday",
    "timestamp": "2025-12-16T22:23:31.796Z"
  }
}
```

---

## Error Handling

The API client provides comprehensive error handling through the `FortunetellerAPIClientError` exception. Here are some examples:

### Basic Error Handling

```python
from apiverve_fortuneteller.apiClient import FortunetellerAPIClient, FortunetellerAPIClientError

api = FortunetellerAPIClient("[YOUR_API_KEY]")

query = {
    "category": "general",
    "question": "What does the future hold for me?"
}

try:
    result = api.execute(query)
    print("Success!")
    print(result)
except FortunetellerAPIClientError as e:
    print(f"API Error: {e.message}")
    if e.status_code:
        print(f"Status Code: {e.status_code}")
    if e.response:
        print(f"Response: {e.response}")
```

### Handling Specific Error Types

```python
from apiverve_fortuneteller.apiClient import FortunetellerAPIClient, FortunetellerAPIClientError

api = FortunetellerAPIClient("[YOUR_API_KEY]")

query = {
    "category": "general",
    "question": "What does the future hold for me?"
}

try:
    result = api.execute(query)

    # Check for successful response
    if result.get('status') == 'success':
        print("Request successful!")
        print(result.get('data'))
    else:
        print(f"API returned an error: {result.get('error')}")

except FortunetellerAPIClientError as e:
    # Handle API client errors
    if e.status_code == 401:
        print("Unauthorized: Invalid API key")
    elif e.status_code == 429:
        print("Rate limit exceeded")
    elif e.status_code >= 500:
        print("Server error - please try again later")
    else:
        print(f"API error: {e.message}")
except Exception as e:
    # Handle unexpected errors
    print(f"Unexpected error: {str(e)}")
```

### Using Context Manager (Recommended)

The client supports the context manager protocol for automatic resource cleanup:

```python
from apiverve_fortuneteller.apiClient import FortunetellerAPIClient, FortunetellerAPIClientError

query = {
    "category": "general",
    "question": "What does the future hold for me?"
}

# Using context manager ensures proper cleanup
with FortunetellerAPIClient("[YOUR_API_KEY]") as api:
    try:
        result = api.execute(query)
        print(result)
    except FortunetellerAPIClientError as e:
        print(f"Error: {e.message}")
# Session is automatically closed here
```

---

## Advanced Features

### Debug Mode

Enable debug logging to see detailed request and response information:

```python
from apiverve_fortuneteller.apiClient import FortunetellerAPIClient

# Enable debug mode
api = FortunetellerAPIClient("[YOUR_API_KEY]", debug=True)

query = {
    "category": "general",
    "question": "What does the future hold for me?"
}

# Debug information will be printed to console
result = api.execute(query)
```

### Manual Session Management

If you need to manually manage the session lifecycle:

```python
from apiverve_fortuneteller.apiClient import FortunetellerAPIClient

api = FortunetellerAPIClient("[YOUR_API_KEY]")

query = {
    "category": "general",
    "question": "What does the future hold for me?"
}

try:
    result = api.execute(query)
    print(result)
finally:
    # Manually close the session when done
    api.close()
```

---

## Customer Support

Need any assistance? [Get in touch with Customer Support](https://apiverve.com/contact?utm_source=pypi&utm_medium=referral&utm_content=readme).

---

## Updates
Stay up to date by following [@apiverveHQ](https://twitter.com/apiverveHQ) on Twitter.

---

## Legal

All usage of the APIVerve website, API, and services is subject to the [APIVerve Terms of Service](https://apiverve.com/terms?utm_source=pypi&utm_medium=referral&utm_content=readme) and all legal documents and agreements.

---

## License
Licensed under the MIT License (MIT)

Copyright (&copy;) 2026 APIVerve, and EvlarSoft LLC

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
