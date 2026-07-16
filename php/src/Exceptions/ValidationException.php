<?php

declare(strict_types=1);

namespace APIVerve\Fortuneteller\Exceptions;

/**
 * Exception thrown when parameter validation fails
 */
class ValidationException extends \Exception
{
    private array $errors;

    /**
     * @param array $errors List of validation error messages
     */
    public function __construct(array $errors)
    {
        $this->errors = $errors;
        parent::__construct("Validation failed: " . implode(' ', $errors));
    }

    /**
     * Get the list of validation errors
     *
     * @return array
     */
    public function getErrors(): array
    {
        return $this->errors;
    }
}
