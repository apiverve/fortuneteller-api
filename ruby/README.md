# Fortune Teller API - Ruby Gem

Fortune Teller uses AI to generate personalized mystical fortunes across multiple categories including love, career, health, wealth, and travel.

## Installation

Add this line to your application's Gemfile:

```ruby
gem 'apiverve_fortuneteller'
```

And then execute:

```bash
$ bundle install
```

Or install it yourself as:

```bash
$ gem install apiverve_fortuneteller
```

## Getting Started

Get your API key at [APIVerve](https://apiverve.com)

### Basic Usage

```ruby
require 'apiverve_fortuneteller'

# Initialize the client
client = APIVerve::Fortuneteller::Client.new(api_key: "YOUR_API_KEY")

# Make a request
response = client.execute({
  category: "general",
  question: "What does the future hold for me?"
})

# Print the response
puts response
```


### Error Handling

```ruby
begin
  response = client.execute({ category: "general", question: "What does the future hold for me?" })
  puts response["data"]
rescue APIVerve::Fortuneteller::ValidationError => e
  puts "Validation error: #{e.errors.join(', ')}"
rescue APIVerve::Fortuneteller::APIError => e
  puts "API error: #{e.message}"
  puts "Status code: #{e.status_code}"
end
```

### Debug Mode

```ruby
# Enable debug logging
client = APIVerve::Fortuneteller::Client.new(
  api_key: "YOUR_API_KEY",
  debug: true
)
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

## Documentation

For more information, visit the [API Documentation](https://docs.apiverve.com/ref/fortuneteller?utm_source=rubygems&utm_medium=referral&utm_content=readme).

## Support

- Website: [https://apiverve.com/marketplace/fortuneteller?utm_source=ruby&utm_medium=referral&utm_content=readme](https://apiverve.com/marketplace/fortuneteller?utm_source=ruby&utm_medium=referral&utm_content=readme)
- Email: hello@apiverve.com

## License

This gem is available under the [MIT License](LICENSE).
