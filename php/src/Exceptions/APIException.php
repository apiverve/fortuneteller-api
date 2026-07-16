<?php

declare(strict_types=1);

namespace APIVerve\Fortuneteller\Exceptions;

/**
 * Exception thrown when an API request fails
 */
class APIException extends \Exception
{
    private ?int $statusCode;
    private ?array $response;

    /**
     * @param string $message Error message
     * @param int|null $statusCode HTTP status code
     * @param array|null $response Full response data
     */
    public function __construct(string $message, ?int $statusCode = null, ?array $response = null)
    {
        $this->statusCode = $statusCode;
        $this->response = $response;
        parent::__construct($message, $statusCode ?? 0);
    }

    /**
     * Get the HTTP status code
     *
     * @return int|null
     */
    public function getStatusCode(): ?int
    {
        return $this->statusCode;
    }

    /**
     * Get the full response data
     *
     * @return array|null
     */
    public function getResponse(): ?array
    {
        return $this->response;
    }
}
