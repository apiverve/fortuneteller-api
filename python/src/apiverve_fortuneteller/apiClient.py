import re
from typing import Dict, Optional, Any
from dataclasses import dataclass, List
import requests


class FortunetellerAPIClientError(Exception):
    """Custom exception for API client errors"""
    def __init__(self, message: str, status_code: Optional[int] = None, response: Optional[Dict] = None):
        self.message = message
        self.status_code = status_code
        self.response = response
        super().__init__(self.message)


class ValidationError(FortunetellerAPIClientError):
    """Custom exception for parameter validation errors"""
    def __init__(self, errors: List[str]):
        self.errors = errors
        message = f"Validation failed: {' '.join(errors)}"
        super().__init__(message)


class FortunetellerAPIClient:
    # Validation rules for parameters (generated from schema)
    VALIDATION_RULES = {
        "category": {"type": "string", "required": False, "default": "general"},
        "question": {"type": "string", "required": False, "maxLength": 500}
    }

    # Format validation patterns
    FORMAT_PATTERNS = {
        'email': r'^[^\s@]+@[^\s@]+\.[^\s@]+$',
        'url': r'^https?://.+',
        'ip': r'^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$|^([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}$',
        'date': r'^\d{4}-\d{2}-\d{2}$',
        'hexColor': r'^#?([0-9a-fA-F]{3}|[0-9a-fA-F]{6})$'
    }

    def __init__(self, api_key: str, secure: bool = True, debug: bool = False):
        """
        Initialize the FortunetellerAPIClient with the API key and optional settings

        :param api_key: Your APIVerve API key
        :param secure: Deprecated. Always set to True
        :param debug: Enable debug logging (default: False)
        :raises FortunetellerAPIClientError: If API key is invalid
        """

        self._validate_api_key(api_key)

        self.api_key = api_key
        self.secure = secure
        self.debug = debug
        self.base_url = 'https://api.apiverve.com/v1/fortuneteller'
        self.headers = {
            'x-api-key': self.api_key,
            'auth-mode': 'pypi-package'
        }

        # Create a session for connection pooling
        self.session = requests.Session()
        self.session.headers.update(self.headers)

    def _validate_api_key(self, api_key: str) -> None:
        """
        Validate the API key format

        :param api_key: API key to validate
        :raises FortunetellerAPIClientError: If API key is invalid
        """
        if not api_key or not api_key.strip():
            raise FortunetellerAPIClientError(
                "API key is required. Get your API key at: https://apiverve.com"
            )

        # Check format (GUID, prefixed keys like apv_xxx, or alphanumeric)
        if not re.match(r'^[a-zA-Z0-9_-]+$', api_key):
            raise FortunetellerAPIClientError(
                "Invalid API key format. API key should only contain letters, numbers, hyphens, and underscores. "
                "Get your API key at: https://apiverve.com"
            )

    def _log(self, message: str) -> None:
        """
        Log a debug message if debug mode is enabled

        :param message: Message to log
        """
        if self.debug:
            print(f"[FortunetellerAPIClient DEBUG] {message}")

    def _validate_params(self, params: Optional[Dict[str, Any]]) -> None:
        """
        Validate parameters against schema rules

        :param params: Parameters to validate
        :raises ValidationError: If validation fails
        """
        if not self.VALIDATION_RULES:
            return

        errors = []
        params = params or {}

        for param_name, rules in self.VALIDATION_RULES.items():
            value = params.get(param_name)

            # Check required
            if rules.get('required') and (value is None or value == ''):
                errors.append(f"Required parameter [{param_name}] is missing.")
                continue

            # Skip validation if value is not provided and not required
            if value is None:
                continue

            param_type = rules.get('type', 'string')

            # Type validation
            if param_type in ('integer', 'number'):
                try:
                    num_value = float(value) if param_type == 'number' else int(value)

                    # Min/max validation
                    if 'min' in rules and num_value < rules['min']:
                        errors.append(f"Parameter [{param_name}] must be at least {rules['min']}.")
                    if 'max' in rules and num_value > rules['max']:
                        errors.append(f"Parameter [{param_name}] must be at most {rules['max']}.")
                except (ValueError, TypeError):
                    errors.append(f"Parameter [{param_name}] must be a valid {param_type}.")
                    continue

            elif param_type == 'string':
                if not isinstance(value, str):
                    errors.append(f"Parameter [{param_name}] must be a string.")
                    continue

                # Length validation
                if 'minLength' in rules and len(value) < rules['minLength']:
                    errors.append(f"Parameter [{param_name}] must be at least {rules['minLength']} characters.")
                if 'maxLength' in rules and len(value) > rules['maxLength']:
                    errors.append(f"Parameter [{param_name}] must be at most {rules['maxLength']} characters.")

                # Format validation
                if 'format' in rules and rules['format'] in self.FORMAT_PATTERNS:
                    pattern = self.FORMAT_PATTERNS[rules['format']]
                    if not re.match(pattern, value, re.IGNORECASE):
                        errors.append(f"Parameter [{param_name}] must be a valid {rules['format']}.")

            elif param_type == 'boolean':
                if not isinstance(value, bool) and value not in ('true', 'false', True, False):
                    errors.append(f"Parameter [{param_name}] must be a boolean.")

            elif param_type == 'array':
                if not isinstance(value, list):
                    errors.append(f"Parameter [{param_name}] must be an array.")

            # Enum validation
            if 'enum' in rules and value not in rules['enum']:
                errors.append(f"Parameter [{param_name}] must be one of: {', '.join(map(str, rules['enum']))}.")

        if errors:
            raise ValidationError(errors)

    def execute(self, params: Optional[Dict[str, Any]] = None) -> Dict[str, Any]:  # Returns FortuneTellerData in 'data' field
        """
        Execute the API request

        :param params: API parameters (optional)
        :return: API response as a dictionary
        :raises FortunetellerAPIClientError: If the request fails or returns an error
        :raises ValidationError: If parameter validation fails
        """

        # Validate parameters against schema rules
        self._validate_params(params)

        method = "GET"

        self._log(f"Making {method} request to {self.base_url}")
        if params:
            self._log(f"Parameters: {params}")

        try:
            if method.upper() == 'POST':
                self._log("Sending POST request with JSON body")
                response = self.session.post(self.base_url, json=params, timeout=30)
            else:
                self._log("Sending GET request with query parameters")
                response = self.session.get(self.base_url, params=params, timeout=30)

            self._log(f"Response status code: {response.status_code}")

            # Try to parse JSON response
            try:
                response_data = response.json()
            except ValueError as e:
                raise FortunetellerAPIClientError(
                    f"Invalid JSON response from API: {str(e)}",
                    status_code=response.status_code
                )

            # Check for API-level errors in the response
            if response_data.get('status') == 'error':
                error_message = response_data.get('error', 'Unknown API error')
                self._log(f"API returned error: {error_message}")
                raise FortunetellerAPIClientError(
                    error_message,
                    status_code=response.status_code,
                    response=response_data
                )

            # Check HTTP status code
            if not response.ok:
                error_message = response_data.get('error', f"HTTP {response.status_code} error")
                self._log(f"HTTP error: {error_message}")
                raise FortunetellerAPIClientError(
                    error_message,
                    status_code=response.status_code,
                    response=response_data
                )

            self._log("Request successful")
            return response_data

        except requests.exceptions.Timeout as e:
            self._log(f"Request timeout: {str(e)}")
            raise FortunetellerAPIClientError(f"Request timeout: {str(e)}")
        except requests.exceptions.ConnectionError as e:
            self._log(f"Connection error: {str(e)}")
            raise FortunetellerAPIClientError(f"Connection error: {str(e)}")
        except requests.exceptions.RequestException as e:
            self._log(f"Request exception: {str(e)}")
            raise FortunetellerAPIClientError(f"Request failed: {str(e)}")


    def close(self) -> None:
        """
        Close the session and release resources
        """
        self._log("Closing session")
        self.session.close()

    def __enter__(self):
        """Context manager entry"""
        return self

    def __exit__(self, exc_type, exc_val, exc_tb):
        """Context manager exit"""
        self.close()
