# Fortune Teller API - Dart/Flutter Client

Fortune Teller uses AI to generate personalized mystical fortunes across multiple categories including love, career, health, wealth, and travel.

[![pub package](https://img.shields.io/pub/v/apiverve_fortuneteller.svg)](https://pub.dev/packages/apiverve_fortuneteller)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

This is the Dart/Flutter client for the [Fortune Teller API](https://apiverve.com/marketplace/fortuneteller?utm_source=dart&utm_medium=referral&utm_content=readme).

## Installation

Add this to your `pubspec.yaml`:

```yaml
dependencies:
  apiverve_fortuneteller: ^1.2.5
```

Then run:

```bash
dart pub get
# or for Flutter
flutter pub get
```

## Usage

```dart
import 'package:apiverve_fortuneteller/apiverve_fortuneteller.dart';

void main() async {
  final client = FortunetellerClient('YOUR_API_KEY');

  try {
    final response = await client.execute({
      'category': 'general',
      'question': 'What does the future hold for me?'
    });

    print('Status: ${response.status}');
    print('Data: ${response.data}');
  } catch (e) {
    print('Error: $e');
  }
}
```

## Response

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

## API Reference

- **API Home:** [Fortune Teller API](https://apiverve.com/marketplace/fortuneteller?utm_source=dart&utm_medium=referral&utm_content=readme)
- **Documentation:** [docs.apiverve.com/ref/fortuneteller](https://docs.apiverve.com/ref/fortuneteller?utm_source=dart&utm_medium=referral&utm_content=readme)

## Authentication

All requests require an API key. Get yours at [apiverve.com](https://apiverve.com?utm_source=dart&utm_medium=referral&utm_content=readme).

## License

MIT License - see [LICENSE](LICENSE) for details.

---

Built with Dart for [APIVerve](https://apiverve.com?utm_source=dart&utm_medium=referral&utm_content=readme)
