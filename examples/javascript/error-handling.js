/**
 * Fortune Teller API - Error Handling Example
 *
 * This example demonstrates proper error handling when using the Fortune Teller API.
 * API Documentation: https://docs.apiverve.com/ref/fortuneteller
 */

const API_KEY = process.env.APIVERVE_API_KEY || 'YOUR_API_KEY_HERE';
const API_URL = 'https://api.apiverve.com/v1/fortuneteller';

class APIError extends Error {
  constructor(message, statusCode, responseData) {
    super(message);
    this.name = 'APIError';
    this.statusCode = statusCode;
    this.responseData = responseData;
  }
}

/**
 * Make an API call with comprehensive error handling
 */
async function callFortuneTellerAPIWithErrorHandling(queryParams = {}) {
  try {
    console.log('📤 Making API request...');

    const params = new URLSearchParams(queryParams);
    const url = `${API_URL}?${params}`;

    const response = await fetch(url, {
      method: 'GET',
      headers: {
        'x-api-key': API_KEY
      }
    });

    // Parse response
    const data = await response.json();

    // Handle different HTTP status codes
    if (!response.ok) {
      switch (response.status) {
        case 400:
          throw new APIError('Bad Request - Check your parameters', 400, data);
        case 401:
          throw new APIError('Unauthorized - Invalid API key', 401, data);
        case 403:
          throw new APIError('Forbidden - Insufficient permissions', 403, data);
        case 429:
          throw new APIError('Rate Limit Exceeded - Please wait before retrying', 429, data);
        case 500:
          throw new APIError('Server Error - Please try again later', 500, data);
        default:
          throw new APIError(`HTTP Error ${response.status}`, response.status, data);
      }
    }

    // Check API-level status
    if (data.status !== 'ok') {
      throw new APIError(
        `API Error: ${data.error || 'Unknown error'}`,
        response.status,
        data
      );
    }

    console.log('✓ Request successful');
    return data.data;

  } catch (error) {
    // Handle different error types
    if (error instanceof APIError) {
      console.error(`\n✗ API Error (${error.statusCode}):`);
      console.error(`  Message: ${error.message}`);
      if (error.responseData) {
        console.error(`  Details:`, JSON.stringify(error.responseData, null, 2));
      }

      // Suggest actions based on error type
      if (error.statusCode === 401) {
        console.error(`  💡 Suggestion: Check your API key in environment variables`);
      } else if (error.statusCode === 429) {
        console.error(`  💡 Suggestion: Implement exponential backoff retry logic`);
      }

    } else if (error instanceof TypeError && error.message.includes('fetch')) {
      console.error('\n✗ Network Error:');
      console.error('  Could not connect to API server');
      console.error('  💡 Suggestion: Check your internet connection');

    } else {
      console.error('\n✗ Unexpected Error:');
      console.error(`  ${error.message}`);
    }

    throw error; // Re-throw for upstream handling
  }
}

/**
 * Retry logic with exponential backoff
 */
async function callWithRetry(maxRetries = 3, initialDelay = 1000) {
  for (let attempt = 1; attempt <= maxRetries; attempt++) {
    try {
      console.log(`\n🔄 Attempt ${attempt}/${maxRetries}`);

      const result = await callFortuneTellerAPIWithErrorHandling({
        // Your query parameters here
      });

      return result;

    } catch (error) {
      if (error instanceof APIError && error.statusCode === 429 && attempt < maxRetries) {
        const delay = initialDelay * Math.pow(2, attempt - 1);
        console.log(`⏳ Waiting ${delay}ms before retry...`);
        await new Promise(resolve => setTimeout(resolve, delay));
      } else {
        throw error; // Don't retry other errors or if max retries reached
      }
    }
  }
}

// Run example with retry logic
callWithRetry()
  .then(result => {
    console.log('\n✓ Final Success!');
    console.log(JSON.stringify(result, null, 2));
  })
  .catch(error => {
    console.log('\n✗ Final Failure');
    process.exit(1);
  });
