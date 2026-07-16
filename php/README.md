# Fortune Teller API - PHP Package

Fortune Teller uses AI to generate personalized mystical fortunes across multiple categories including love, career, health, wealth, and travel.

## Installation

Install via Composer:

```bash
composer require apiverve/fortuneteller
```

## Getting Started

Get your API key at [APIVerve](https://apiverve.com)

### Basic Usage

```php
<?php

require_once 'vendor/autoload.php';

use APIVerve\Fortuneteller\Client;

// Initialize the client
$client = new Client('YOUR_API_KEY');

// Make a request
$response = $client->execute([
    'category' => 'general',
    'question' => 'What does the future hold for me?'
]);

// Print the response
print_r($response);
```


### Error Handling

```php
use APIVerve\Fortuneteller\Client;
use APIVerve\Fortuneteller\Exceptions\APIException;
use APIVerve\Fortuneteller\Exceptions\ValidationException;

try {
    $response = $client->execute(['category' => 'general', 'question' => 'What does the future hold for me?']);
    print_r($response['data']);
} catch (ValidationException $e) {
    echo "Validation error: " . implode(', ', $e->getErrors());
} catch (APIException $e) {
    echo "API error: " . $e->getMessage();
    echo "Status code: " . $e->getStatusCode();
}
```

### Debug Mode

```php
// Enable debug logging
$client = new Client(
    apiKey: 'YOUR_API_KEY',
    debug: true
);
```

## Example Response

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

## Requirements

- PHP 7.4 or higher
- Guzzle HTTP client

## Documentation

For more information, visit the [API Documentation](https://docs.apiverve.com/ref/fortuneteller?utm_source=packagist&utm_medium=readme).

## Support

- Website: [https://apiverve.com/marketplace/fortuneteller?utm_source=php&utm_medium=readme](https://apiverve.com/marketplace/fortuneteller?utm_source=php&utm_medium=readme)
- Email: hello@apiverve.com

## License

This package is available under the [MIT License](LICENSE).
