# MeetUp API Testing with Postman

This directory contains Postman files to help you test the MeetUp API.

## Files

- `MeetUp_API.postman_collection.json`: The main collection containing all API requests organized by service
- `MeetUp_API.postman_environment.json`: Environment variables for the collection

## Setup Instructions

1. Install [Postman](https://www.postman.com/downloads/) if you haven't already
2. Import the collection file:
   - Open Postman
   - Click "Import" button
   - Select the `MeetUp_API.postman_collection.json` file
3. Import the environment file:
   - Click "Import" button
   - Select the `MeetUp_API.postman_environment.json` file
4. Select the imported environment from the environment dropdown (top right)

## Testing Workflow

1. **Authentication**:

   - First, register a user with the "Register User" request
   - Then get a token with the "Get Token" request (this will automatically save the token to the environment variables)

2. **Testing Services**:

   - Use the organized folders to test different microservices
   - For authenticated endpoints, the token will be automatically included

3. **Gateway Testing**:
   - You can also test requests through the API gateway
   - These are available in the "Gateway Tests" folder

## Variables

The collection uses the following variables:

- `baseUrl`: Base URL for all services (default: http://localhost)
- `identityPort`, `eventPort`, etc.: Port numbers for each microservice
- `access_token`: Automatically populated when you request a token
- `username` and `password`: Default credentials for testing
- `event_id` and `conversation_id`: Can be populated after creating resources

## Testing Sequence

1. Register a user
2. Get auth token
3. Create a user profile
4. Create an event
5. Test search functionality
6. Create a conversation
7. Test various CRUD operations

## Notes

- Replace placeholder IDs (like `00000000-0000-0000-0000-000000000000`) with actual IDs from your database
- Some endpoints require authentication - make sure to get a token first
- Check the "Tests" tab in the "Get Token" request - it automatically sets the token as an environment variable
