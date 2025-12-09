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

        // Validate API key format (GUID or alphanumeric with hyphens)
        const apiKeyPattern = /^[a-zA-Z0-9-]+$/;
        if (!apiKeyPattern.test(api_key)) {
            throw new Error('Invalid API key format. API key must be alphanumeric and may contain hyphens. Get your API key at: https://apiverve.com');
        }

        // Check minimum length (GUIDs are typically 36 chars with hyphens, or 32 without)
        const trimmedKey = api_key.replace(/-/g, '');
        if (trimmedKey.length < 32) {
            throw new Error('Invalid API key. API key appears to be too short. Get your API key at: https://apiverve.com');
        }

        if (typeof secure !== 'boolean') {
            throw new Error('Secure parameter must be a boolean value.');
        }

        this.APIKey = api_key;
        this.IsSecure = secure;

        // secure is deprecated, all requests must be made over HTTPS
        this.baseURL = 'https://api.apiverve.com/v1/fortuneteller';
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

        var requiredParams = [];
        if (requiredParams.length > 0) {
            for (var i = 0; i < requiredParams.length; i++) {
                if (!query[requiredParams[i]]) {
                    throw new Error(`Required parameter [${requiredParams[i]}] is missing. See documentation: https://docs.apiverve.com/ref/fortuneteller`);
                }
            }
        }

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
