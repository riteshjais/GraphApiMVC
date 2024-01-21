# Microsoft Graph API Integration with ASP.NET Core for OneDrive File Management

## Overview
This project is designed to integrate Microsoft Graph API into an ASP.NET Core application, enabling users to manage their OneDrive files seamlessly. The key functionalities include uploading small and large files, retrieving file lists, fetching specific files using file IDs, and ensuring secure authentication through Microsoft Graph API.

## Features

### 1. Authentication
- The project implements secure authentication using Microsoft Graph API, ensuring that only authorized users can access their OneDrive.

### 2. Small File Upload
- Users can upload small files (less than 4MB) to their OneDrive through the application. The authentication code is generated, and the obtained access token is included in the API call header for secure file uploads.

### 3. File Retrieval
- Retrieve a list of all files stored in OneDrive, providing an organized view of the user's OneDrive content.

### 4. Single File Retrieval
- Fetch a specific file from OneDrive by providing the file ID, enhancing precision in file retrieval for individual files.

### 5. Large File Upload
- Support for uploading large files, demonstrating scalability and versatility in managing files of varying sizes. The authentication process ensures the security of these large file uploads.

## Technologies Used
- **ASP.NET Core:** A robust framework for scalable and cross-platform development.
- **Microsoft Graph API:** Utilized for seamless integration with OneDrive, enabling secure authentication and file management.
- **C#:** The primary programming language used for the development of the ASP.NET Core application.
- **OAuth 2.0:** Authentication implemented using OAuth 2.0 for secure and authorized access to Microsoft Graph API.

## Project Benefits
- Streamlined file management: Users can easily upload, retrieve, and manage their files on OneDrive directly through the application.
- Secure authentication: The use of OAuth 2.0 and Microsoft Graph API ensures a secure and authorized user experience.
- Versatility: The project supports both small and large file uploads, catering to diverse user requirements.

## Getting Started
1. Clone the repository.
2. Configure authentication with your Microsoft Graph API credentials.
3. Build and run the application.
