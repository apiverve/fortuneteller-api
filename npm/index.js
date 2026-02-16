const axios = require('axios');

class fortunetellerWrapper {

    constructor(options) {
        if (!options || typeof options !== 'object') {
            throw new Error('Options object must be provided. See documentation: https://docs.apiverve.com/ref/fortuneteller');
        }

        const { api_key, secure = true } = options;

        if (!api_key || typeof api_key !== 'string') {
            throw new Error('API key must be provided as a non-empty string. Get your API key at: https://apiverve.com');
        }

        // Validate API key format (GUID, prefixed keys like apv_xxx, or alphanumeric)
        const apiKeyPattern = /^[a-zA-Z0-9_-]+$/;
        if (!apiKeyPattern.test(api_key)) {
            throw new Error('Invalid API key format. API key must be alphanumeric and may contain hyphens and underscores. Get your API key at: https://apiverve.com');
        }

        if (typeof secure !== 'boolean') {
            throw new Error('Secure parameter must be a boolean value.');
        }

        this.APIKey = api_key;
        this.IsSecure = secure;

        // secure is deprecated, all requests must be made over HTTPS
        this.baseURL = 'https://api.apiverve.com/v1/fortuneteller';

        // Validation rules for parameters (generated from schema)
        this.validationRules = {"category":{"type":"string","required":false,"default":"general"},"question":{"type":"string","required":false,"maxLength":500}};
    }

    /**
     * Validate query parameters against schema rules
     * @param {Object} query - The query parameters to validate
     * @throws {Error} - If validation fails
     */
    validateParams(query) {
        const errors = [];

        for (const [paramName, rules] of Object.entries(this.validationRules)) {
            const value = query[paramName];

            // Check required
            if (rules.required && (value === undefined || value === null || value === '')) {
                errors.push(`Required parameter [${paramName}] is missing.`);
                continue;
            }

            // Skip validation if value is not provided and not required
            if (value === undefined || value === null) {
                continue;
            }

            // Type validation
            if (rules.type === 'integer' || rules.type === 'number') {
                const numValue = Number(value);
                if (isNaN(numValue)) {
                    errors.push(`Parameter [${paramName}] must be a valid ${rules.type}.`);
                    continue;
                }

                if (rules.type === 'integer' && !Number.isInteger(numValue)) {
                    errors.push(`Parameter [${paramName}] must be an integer.`);
                    continue;
                }

                // Min/max validation for numbers
                if (rules.min !== undefined && numValue < rules.min) {
                    errors.push(`Parameter [${paramName}] must be at least ${rules.min}.`);
                }
                if (rules.max !== undefined && numValue > rules.max) {
                    errors.push(`Parameter [${paramName}] must be at most ${rules.max}.`);
                }
            } else if (rules.type === 'string') {
                if (typeof value !== 'string') {
                    errors.push(`Parameter [${paramName}] must be a string.`);
                    continue;
                }

                // Length validation for strings
                if (rules.minLength !== undefined && value.length < rules.minLength) {
                    errors.push(`Parameter [${paramName}] must be at least ${rules.minLength} characters.`);
                }
                if (rules.maxLength !== undefined && value.length > rules.maxLength) {
                    errors.push(`Parameter [${paramName}] must be at most ${rules.maxLength} characters.`);
                }

                // Format validation
                if (rules.format) {
                    const formatPatterns = {
                        'email': /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                        'url': /^https?:\/\/.+/i,
                        'ip': /^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$|^([0-9a-fA-F]{1,4}:){7}[0-9a-fA-F]{1,4}$/,
                        'date': /^\d{4}-\d{2}-\d{2}$/,
                        'hexColor': /^#?([0-9a-fA-F]{3}|[0-9a-fA-F]{6})$/
                    };

                    if (formatPatterns[rules.format] && !formatPatterns[rules.format].test(value)) {
                        errors.push(`Parameter [${paramName}] must be a valid ${rules.format}.`);
                    }
                }
            } else if (rules.type === 'boolean') {
                if (typeof value !== 'boolean' && value !== 'true' && value !== 'false') {
                    errors.push(`Parameter [${paramName}] must be a boolean.`);
                }
            } else if (rules.type === 'array') {
                if (!Array.isArray(value)) {
                    errors.push(`Parameter [${paramName}] must be an array.`);
                }
            }

            // Enum validation
            if (rules.enum && Array.isArray(rules.enum)) {
                if (!rules.enum.includes(value)) {
                    errors.push(`Parameter [${paramName}] must be one of: ${rules.enum.join(', ')}.`);
                }
            }
        }

        if (errors.length > 0) {
            throw new Error(`Validation failed: ${errors.join(' ')} See documentation: https://docs.apiverve.com/ref/fortuneteller`);
        }
    }

    async execute(query, callback) {
        // Handle different argument patterns
        if(arguments.length === 0) {
            // execute() - no args
            query = {};
            callback = null;
        } else if(arguments.length === 1) {
            if (typeof query === 'function') {
                // execute(callback)
                callback = query;
                query = {};
            } else {
                // execute(query)
                callback = null;
            }
        } else {
            // execute(query, callback)
            if (!query || typeof query !== 'object') {
                throw new Error('Query parameters must be provided as an object.');
            }
        }

        // Validate parameters against schema rules
        this.validateParams(query);

        const method = 'GET';
        const url = method === 'POST' ? this.baseURL : this.constructURL(query);

        try {
            const response = await axios({
                method,
                url,
                headers: {
                    'Content-Type': 'application/json',
                    'x-api-key': this.APIKey,
                    'auth-mode': 'npm-package'
                },
                data: method === 'POST' ? query : undefined
            });

            const data = response.data;
            if (callback) callback(null, data);
            return data;
        } catch (error) {
            let apiError;

            if (error.response && error.response.data) {
                apiError = error.response.data;
            } else if (error.message) {
                apiError = { error: error.message, status: 'error' };
            } else {
                apiError = { error: 'An unknown error occurred', status: 'error' };
            }

            if (callback) {
                callback(apiError, null);
                return; // Don't throw if callback is provided
            }

            throw apiError;
        }
    }

    constructURL(query) {
        let url = this.baseURL;

        if(query && typeof query === 'object')
        {
            if (Object.keys(query).length > 0) {
                const queryString = Object.keys(query)
                    .map(key => `${encodeURIComponent(key)}=${encodeURIComponent(query[key])}`)
                    .join('&');
                url += `?${queryString}`;
            }
        }
        return url;
    }

}

module.exports = fortunetellerWrapper;
