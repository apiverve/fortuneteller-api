package com.apiverve.fortuneteller;

/**
 * Exception thrown when API requests fail
 */
public class APIException extends Exception {
    private final int statusCode;

    /**
     * Construct an APIException
     * @param message Error message
     */
    public APIException(String message) {
        super(message);
        this.statusCode = 0;
    }

    /**
     * Construct an APIException with cause
     * @param message Error message
     * @param cause Underlying cause
     */
    public APIException(String message, Throwable cause) {
        super(message, cause);
        this.statusCode = 0;
    }

    /**
     * Construct an APIException with HTTP status code
     * @param message Error message
     * @param statusCode HTTP status code
     */
    public APIException(String message, int statusCode) {
        super(message);
        this.statusCode = statusCode;
    }

    /**
     * Get the HTTP status code if available
     * @return HTTP status code or 0 if not available
     */
    public int getStatusCode() {
        return statusCode;
    }

    /**
     * Check if this is an authentication error (401 or 403)
     * @return true if authentication error
     */
    public boolean isAuthenticationError() {
        return statusCode == 401 || statusCode == 403;
    }

    /**
     * Check if this is a rate limit error (429)
     * @return true if rate limit error
     */
    public boolean isRateLimitError() {
        return statusCode == 429;
    }

    /**
     * Check if this is a server error (5xx)
     * @return true if server error
     */
    public boolean isServerError() {
        return statusCode >= 500 && statusCode < 600;
    }

    /**
     * Check if this is a client error (4xx)
     * @return true if client error
     */
    public boolean isClientError() {
        return statusCode >= 400 && statusCode < 500;
    }
}
