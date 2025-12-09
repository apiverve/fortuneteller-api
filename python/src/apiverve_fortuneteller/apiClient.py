import requests

class FortunetellerAPIClient:
    def __init__(self, api_key, secure=True):
        """
        Initialize the FortunetellerAPIClient with the API key and secure flag
        :param api_key: API key
        :param secure: Deprecated. Always set to True
        """
        
        self.api_key = api_key
        self.secure = secure
        self.base_url = 'https://api.apiverve.com/v1/fortuneteller'
        self.headers = {
            'x-api-key': self.api_key,
            'auth-mode': 'pypi-package'
        }

    def execute(self, params=None):
        """
        Execute the API request
        :param params: API parameters, if any
        :return: API response
        """

        method = "GET"

        try:
            if method.upper() == 'POST':
                response = requests.post(self.base_url, headers=self.headers, json=params)
            else:
                response = requests.get(self.base_url, headers=self.headers, params=params)
            
            response.raise_for_status()
            return response.json()
        except requests.exceptions.RequestException as e:
            print(f"An error occurred: {e}")
            raise e