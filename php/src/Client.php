<?php

declare(strict_types=1);

namespace APIVerve\Fortuneteller;

use GuzzleHttp\Client as HttpClient;
use GuzzleHttp\Exception\GuzzleException;
use APIVerve\Fortuneteller\Exceptions\APIException;
use APIVerve\Fortuneteller\Exceptions\ValidationException;

/**
 * Client for the Fortune Teller API
 *
 * @example Basic usage
 * ```php
 * $client = new Client('your_api_key');
 * $response = $client->execute(['category' => 'general', 'question' => 'What does the future hold for me?']);
 * print_r($response);
 * ```
 *
 * @see https://apiverve.com/marketplace/fortuneteller?utm_source=php&utm_medium=readme
 */
class Client
{
    private const BASE_URL = 'https://api.apiverve.com/v1/fortuneteller';
    private const DEFAULT_TIMEOUT = 30;

    private string $apiKey;
    private HttpClient $httpClient;
    private bool $debug;

    /**
     * Validation rules for parameters
     */
    private const VALIDATION_RULES = [
        'category' => ['type' => 'string', 'required' => false],
        'question' => ['type' => 'string', 'required' => false, 'maxLength' => 500]
    ];

    /**
     * Format validation patterns
     */
    private const FORMAT_PATTERNS = [
        'email' => '/^[^\s@]+@[^\s@]+\.[^\s@]+$/',
        'url' => '/^https?:\/\/.+/',
        'ip' => '/^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$|^([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}$/',
        'date' => '/^\d{4}-\d{2}-\d{2}$/',
        'hexColor' => '/^#?([0-9a-fA-F]{3}|[0-9a-fA-F]{6})$/'
    ];

    /**
     * Initialize the client
     *
     * @param string $apiKey Your APIVerve API key
     * @param int $timeout Request timeout in seconds (default: 30)
     * @param bool $debug Enable debug logging (default: false)
     * @throws \InvalidArgumentException If API key is invalid
     */
    public function __construct(string $apiKey, int $timeout = self::DEFAULT_TIMEOUT, bool $debug = false)
    {
        $this->validateApiKey($apiKey);

        $this->apiKey = $apiKey;
        $this->debug = $debug;

        $this->httpClient = new HttpClient([
            'base_uri' => self::BASE_URL,
            'timeout' => $timeout,
            'headers' => [
                'x-api-key' => $this->apiKey,
                'auth-mode' => 'packagist-package',
                'Content-Type' => 'application/json'
            ]
        ]);
    }

    /**
     * Execute the API request
     *
     * @param array $params Query parameters or request body
     * @return array API response
     * @throws APIException If the request fails
     * @throws ValidationException If parameter validation fails
     */
    public function execute(array $params = []): array
    {
        $this->validateParams($params);

        $this->log("Making GET request to " . self::BASE_URL);
        if (!empty($params)) {
            $this->log("Parameters: " . json_encode($params));
        }

        try {
            $response = $this->httpClient->get('', [
                'query' => $params
            ]);

            return $this->handleResponse($response);
        } catch (GuzzleException $e) {
            throw new APIException("Request failed: " . $e->getMessage(), $e->getCode());
        }
    }


    /**
     * Validate the API key format
     *
     * @param string $apiKey
     * @throws \InvalidArgumentException If API key is invalid
     */
    private function validateApiKey(string $apiKey): void
    {
        if (empty(trim($apiKey))) {
            throw new \InvalidArgumentException(
                "API key is required. Get your API key at: https://apiverve.com"
            );
        }

        if (!preg_match('/^[a-zA-Z0-9_-]+$/', $apiKey)) {
            throw new \InvalidArgumentException(
                "Invalid API key format. API key should only contain letters, numbers, hyphens, and underscores."
            );
        }
    }

    /**
     * Validate parameters against schema rules
     *
     * @param array $params
     * @throws ValidationException If validation fails
     */
    private function validateParams(array $params): void
    {
        if (empty(self::VALIDATION_RULES)) {
            return;
        }

        $errors = [];

        foreach (self::VALIDATION_RULES as $paramName => $rules) {
            $value = $params[$paramName] ?? null;

            // Check required
            if (($rules['required'] ?? false) && ($value === null || $value === '')) {
                $errors[] = "Required parameter [{$paramName}] is missing.";
                continue;
            }

            if ($value === null) {
                continue;
            }

            $type = $rules['type'] ?? 'string';

            // Type validation
            switch ($type) {
                case 'integer':
                case 'number':
                    if (!is_numeric($value)) {
                        $errors[] = "Parameter [{$paramName}] must be a valid {$type}.";
                        continue 2;
                    }
                    $numValue = $type === 'number' ? (float)$value : (int)$value;
                    if (isset($rules['min']) && $numValue < $rules['min']) {
                        $errors[] = "Parameter [{$paramName}] must be at least {$rules['min']}.";
                    }
                    if (isset($rules['max']) && $numValue > $rules['max']) {
                        $errors[] = "Parameter [{$paramName}] must be at most {$rules['max']}.";
                    }
                    break;

                case 'string':
                    if (!is_string($value)) {
                        $errors[] = "Parameter [{$paramName}] must be a string.";
                        continue 2;
                    }
                    if (isset($rules['minLength']) && strlen($value) < $rules['minLength']) {
                        $errors[] = "Parameter [{$paramName}] must be at least {$rules['minLength']} characters.";
                    }
                    if (isset($rules['maxLength']) && strlen($value) > $rules['maxLength']) {
                        $errors[] = "Parameter [{$paramName}] must be at most {$rules['maxLength']} characters.";
                    }
                    if (isset($rules['format']) && isset(self::FORMAT_PATTERNS[$rules['format']])) {
                        if (!preg_match(self::FORMAT_PATTERNS[$rules['format']], $value)) {
                            $errors[] = "Parameter [{$paramName}] must be a valid {$rules['format']}.";
                        }
                    }
                    break;

                case 'boolean':
                    if (!is_bool($value) && !in_array($value, ['true', 'false', '0', '1'], true)) {
                        $errors[] = "Parameter [{$paramName}] must be a boolean.";
                    }
                    break;
            }

            // Enum validation
            if (isset($rules['enum']) && !in_array($value, $rules['enum'], true)) {
                $errors[] = "Parameter [{$paramName}] must be one of: " . implode(', ', $rules['enum']) . ".";
            }
        }

        if (!empty($errors)) {
            throw new ValidationException($errors);
        }
    }

    /**
     * Handle the API response
     *
     * @param \Psr\Http\Message\ResponseInterface $response
     * @return array
     * @throws APIException If the response indicates an error
     */
    private function handleResponse($response): array
    {
        $statusCode = $response->getStatusCode();
        $body = (string)$response->getBody();

        $this->log("Response status: {$statusCode}");

        try {
            $data = json_decode($body, true, 512, JSON_THROW_ON_ERROR);
        } catch (\JsonException $e) {
            throw new APIException("Invalid JSON response: " . $e->getMessage(), $statusCode);
        }

        if (($data['status'] ?? '') === 'error') {
            throw new APIException($data['error'] ?? 'Unknown API error', $statusCode, $data);
        }

        if ($statusCode >= 400) {
            throw new APIException($data['error'] ?? "HTTP {$statusCode} error", $statusCode, $data);
        }

        $this->log("Request successful");
        return $data;
    }

    /**
     * Log a debug message
     *
     * @param string $message
     */
    private function log(string $message): void
    {
        if ($this->debug) {
            echo "[APIVerve\FortunetellerClient] {$message}\n";
        }
    }
}
