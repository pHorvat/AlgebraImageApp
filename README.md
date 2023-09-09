# AlgebraImageApp

[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

Welcome to the Image Viewing Backend App repository! This .NET application serves as the backend for an image viewing app. It follows the Model-View-Controller (MVC) architecture and provides a robust foundation for managing user authentication, image uploads, and other functionalities.

## Features

- **User Authentication**: User authentication is accomplished using BCrypt for password hashing and JWT (JSON Web Tokens) for secure authentication and authorization.
- **Image Uploads**: Allow users to upload images along with descriptions and hashtags for easy searching and categorization.
- **Usage Package Selection**: Enable users to choose and edit their usage package, providing different levels of access or features.
- **Image Viewing**: Allow users to view images uploaded by other users and search for images using hashtags or descriptions.

## Project Structure

The project follows a common MVC structure, separating different components of the application:

```
├── Controllers/              # Contains MVC controllers for handling incoming requests
├── Models/                   # Data models for representing users, images, etc.
├── Services/                 # Business logic and services for handling application functionality
├── Repositories/             # Data access layer for interacting with the database
├── Program.cs                # Entry point of the application
└── ...
```

## License

This project is licensed under the [MIT License](LICENSE). Feel free to use and modify the code as per your needs.
