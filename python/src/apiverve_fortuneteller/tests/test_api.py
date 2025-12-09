import unittest
from apiverve_fortuneteller.apiClient import FortunetellerAPIClient

class TestClient(unittest.TestCase):

    def setUp(self):
        self.api_key = 'test_api_key'
        self.client = FortunetellerAPIClient(self.api_key)

    def test_get(self):
        # Assuming there's an endpoint 'test_endpoint' for testing purposes
        try:
            response = self.client.get('test_endpoint')
            self.assertIsInstance(response, dict)
        except Exception as e:
            self.fail(f"GET request failed with exception {e}")

if __name__ == '__main__':
    unittest.main()
