package com.apiverve.fortuneteller;

import org.json.JSONException;
import org.json.JSONObject;

/**
 * Response object from the Fortune Teller
 */
public class APIResponse {
    private final JSONObject rawResponse;
    private final String status;
    private final String error;
    private final JSONObject data;
    private final int code;

    /**
     * Construct an APIResponse from JSON string
     * @param jsonString Raw JSON response from API
     * @throws APIException if JSON parsing fails
     */
    public APIResponse(String jsonString) throws APIException {
        try {
            this.rawResponse = new JSONObject(jsonString);
            this.status = rawResponse.optString("status", "error");
            this.error = rawResponse.optString("error", null);
            this.data = rawResponse.optJSONObject("data");
            this.code = rawResponse.optInt("code", 0);
        } catch (JSONException e) {
            throw new APIException("Failed to parse API response: " + e.getMessage(), e);
        }
    }

    /**
     * Get the full raw JSON response
     * @return JSONObject containing the complete response
     */
    public JSONObject getRawResponse() {
        return rawResponse;
    }

    /**
     * Get the response status
     * @return Status string (e.g., "ok", "error")
     */
    public String getStatus() {
        return status;
    }

    /**
     * Get the error message if present
     * @return Error message or null if no error
     */
    public String getError() {
        return error;
    }

    /**
     * Get the response data
     * @return JSONObject containing response data or null
     */
    public JSONObject getData() {
        return data;
    }

    /**
     * Get the response code
     * @return Response code
     */
    public int getCode() {
        return code;
    }

    /**
     * Check if the response indicates success
     * @return true if successful, false otherwise
     */
    public boolean isSuccess() {
        return "ok".equals(status) || "success".equals(status);
    }

    /**
     * Check if the response indicates an error
     * @return true if error, false otherwise
     */
    public boolean isError() {
        return "error".equals(status) || error != null;
    }

    /**
     * Get the response as a formatted JSON string
     * @return Pretty-printed JSON string
     */
    @Override
    public String toString() {
        try {
            return rawResponse.toString(2);
        } catch (JSONException e) {
            return rawResponse.toString();
        }
    }
}
