# Contributing to Fortune Teller API

Thank you for your interest in contributing! This guide will help you get started.

## Ways to Contribute

### 1. Report Bugs
Found a bug? [Open an issue](../../issues/new) with:
- Clear description
- Steps to reproduce
- Expected vs actual behavior
- Environment details (Node.js version, OS, etc.)

### 2. Suggest Features
Have an idea? [Open an issue](../../issues/new) with:
- Use case description
- Proposed solution
- Alternative approaches considered

### 3. Improve Documentation
- Fix typos or unclear sections
- Add examples
- Improve code comments

### 4. Submit Code Changes
- Bug fixes
- Performance improvements
- Test coverage improvements

## Development Setup

### NPM Package
```bash
cd npm/
npm install
npm test
```

### NuGet Package
```bash
cd nuget/
dotnet restore
dotnet build
dotnet test
```

### Python Package
```bash
cd python/
pip install -r requirements.txt
python -m pytest
```

### CocoaPods Package
```bash
cd cocoapods/
pod install
# Run tests in Xcode
```

### Android Package
```bash
cd android/
gradle build
gradle test
```

## Pull Request Process

1. **Fork** the repository
2. **Create** a feature branch (`git checkout -b feature/amazing-feature`)
3. **Make** your changes
4. **Test** thoroughly
5. **Commit** with clear messages (`git commit -m 'Add amazing feature'`)
6. **Push** to your fork (`git push origin feature/amazing-feature`)
7. **Open** a Pull Request

### PR Guidelines
- ✅ Clear description of changes
- ✅ Reference related issues
- ✅ Include tests for new features
- ✅ Update documentation
- ✅ Follow existing code style

## Code Style

- **JavaScript/Node.js**: Follow existing conventions
- **.NET/C#**: Follow Microsoft C# guidelines
- **Python**: Follow PEP 8
- **Swift**: Follow Swift API Design Guidelines
- **Java**: Follow Google Java Style Guide

## Testing

All contributions must include tests:
- Unit tests for new features
- Integration tests for API calls
- All existing tests must pass

## Questions?

- 📚 **Documentation**: [https://docs.apiverve.com/ref/fortuneteller](https://docs.apiverve.com/ref/fortuneteller)
- 💬 **Discussions**: [GitHub Discussions](../../discussions)
- 🆘 **Support**: [https://apiverve.com/contact](https://apiverve.com/contact)

## Code of Conduct

This project follows the [Code of Conduct](CODE_OF_CONDUCT.md). By participating, you agree to uphold this code.

## License

By contributing, you agree that your contributions will be licensed under the MIT License.
