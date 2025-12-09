# Fortune Teller API - Examples

This folder contains example code for using the Fortune Teller API across different platforms.

## 📁 Examples Structure

- **[javascript/](./javascript/)** - Node.js and Browser examples
- **[python/](./python/)** - Python examples
- **[csharp/](./csharp/)** - C# / .NET examples

## 🚀 Quick Examples

### JavaScript (Node.js)
```javascript
// See: ./javascript/basic.js
const response = await fetch('https://api.apiverve.com/v1/fortuneteller', {
  headers: { 'x-api-key': 'YOUR_API_KEY' }
});
const data = await response.json();
```

### Python
```python
# See: ./python/basic.py
import requests

response = requests.get('https://api.apiverve.com/v1/fortuneteller',
  headers={'x-api-key': 'YOUR_API_KEY'})
data = response.json()
```

### C#
```csharp
// See: ./csharp/basic.cs
var client = new HttpClient();
client.DefaultRequestHeaders.Add("x-api-key", "YOUR_API_KEY");
var response = await client.GetAsync("https://api.apiverve.com/v1/fortuneteller");
var data = await response.Content.ReadAsStringAsync();
```

## 📚 Documentation

- **API Reference:** [https://docs.apiverve.com/ref/fortuneteller](https://docs.apiverve.com/ref/fortuneteller)
- **Get API Key:** [https://apiverve.com](https://apiverve.com)

## 💡 Tips

1. **Never commit your API key** - Use environment variables
2. **Handle errors gracefully** - Check response status codes
3. **Respect rate limits** - Implement exponential backoff
4. **Cache responses** - When appropriate for your use case

## 🆘 Need Help?

- 📧 Email: support@apiverve.com
- 💬 Issues: [GitHub Issues](../../issues)
- 📖 Docs: [https://docs.apiverve.com](https://docs.apiverve.com)
