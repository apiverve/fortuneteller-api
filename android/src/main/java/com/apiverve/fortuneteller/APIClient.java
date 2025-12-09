package com.apiverve.fortuneteller;

import org.json.JSONObject;
import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLEncoder;
import java.nio.charset.StandardCharsets;
import java.util.Map;

/**
 * Main API client for Fortune Teller
 * Provides a simple interface to access the APIVerve Fortune Teller
 */
public class FortuneTellerAPIClient {
    private final String apiKey;
    private final String baseURL;
    private static final int CONNECT_TIMEOUT = 30000; // 30 seconds
    private static final int READ_TIMEOUT = 30000; // 30 seconds

    /**
     * Initialize the API client
     * @param apiKey Your APIVerve API key from https://apiverve.com
     * @throws IllegalArgumentException if API key is invalid
     */
    public FortuneTellerAPIClient(String apiKey) {
        if (apiKey == null || apiKey.trim().isEmpty()) {
            throw new IllegalArgumentException("API key must be provided. Get your API key at: https://apiverve.com");
        }

        // Validate API key format (alphanumeric with hyphens)
        if (!apiKey.matches("^[a-zA-Z0-9-]+$")) {
            throw new IllegalArgumentException("Invalid API key format. API key must be alphanumeric and may contain hyphens");
        }

        // Check minimum length (GUIDs are typically 36 chars with hyphens, or 32 without)
        String trimmedKey = apiKey.replace("-", "");
        if (trimmedKey.length() < 32) {
            throw new IllegalArgumentException("Invalid API key. API key appears to be too short");
        }

        this.apiKey = apiKey;
        this.baseURL = "https://api.apiverve.com/v1/fortuneteller";
    }

    /**
     * Execute the API request
     * @param parameters Query parameters or request body (can be null)
     * @return APIResponse object containing the response
     * @throws APIException if the request fails
     */
    public APIResponse execute(Map<String, Object> parameters) throws APIException {
        return executeGet(parameters);
    }

    /**
     * Execute the API request without parameters
     * @return APIResponse object containing the response
     * @throws APIException if the request fails
     */
    public APIResponse execute() throws APIException {
        return execute(null);
    }

    private APIResponse executeGet(Map<String, Object> parameters) throws APIException {
        try {
            String urlString = baseURL;

            // Add query parameters
            if (parameters != null && !parameters.isEmpty()) {
                StringBuilder queryString = new StringBuilder("?");
                boolean first = true;
                for (Map.Entry<String, Object> entry : parameters.entrySet()) {
                    if (!first) {
                        queryString.append("&");
                    }
                    queryString.append(URLEncoder.encode(entry.getKey(), "UTF-8"));
                    queryString.append("=");
                    queryString.append(URLEncoder.encode(entry.getValue().toString(), "UTF-8"));
                    first = false;
                }
                urlString += queryString.toString();
            }

            URL url = new URL(urlString);
            HttpURLConnection conn = (HttpURLConnection) url.openConnection();
            conn.setRequestMethod("GET");
            conn.setConnectTimeout(CONNECT_TIMEOUT);
            conn.setReadTimeout(READ_TIMEOUT);
            conn.setRequestProperty("x-api-key", apiKey);
            conn.setRequestProperty("auth-mode", "android-package");

            return handleResponse(conn);
        } catch (Exception e) {
            throw new APIException("Network error: " + e.getMessage(), e);
        }
    }

    private APIResponse handleResponse(HttpURLConnection conn) throws APIException {
        try {
            int statusCode = conn.getResponseCode();
            BufferedReader reader;

            if (statusCode == 200) {
                reader = new BufferedReader(new InputStreamReader(conn.getInputStream(), StandardCharsets.UTF_8));
            } else {
                reader = new BufferedReader(new InputStreamReader(conn.getErrorStream(), StandardCharsets.UTF_8));
            }

            StringBuilder response = new StringBuilder();
            String line;
            while ((line = reader.readLine()) != null) {
                response.append(line);
            }
            reader.close();

            String responseBody = response.toString();

            if (statusCode != 200) {
                throw new APIException("HTTP error " + statusCode + ": " + responseBody, statusCode);
            }

            return new APIResponse(responseBody);
        } catch (APIException e) {
            throw e;
        } catch (Exception e) {
            throw new APIException("Failed to process response: " + e.getMessage(), e);
        }
    }
}
