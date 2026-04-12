```markdown
# Node.js Express API Project

A simple Node.js Express application that provides a REST API endpoint to return dummy user data.

## Prerequisites

- Docker installed on your system
- Node.js (only for local development, optional)

## Project Structure

```
project/
├── Dockerfile
├── package.json
├── package-lock.json
├── src/
│   └── index.js
└── README.md
```

## API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/users` | Returns 5 dummy users in JSON format |

### Sample Response - GET /users

```json
[
    {
        "id": 1,
        "name": "John Doe",
        "email": "john.doe@example.com",
        "phone": "+1-555-0101",
        "role": "Admin"
    },
    {
        "id": 2,
        "name": "Jane Smith",
        "email": "jane.smith@example.com",
        "phone": "+1-555-0102",
        "role": "User"
    }
]
```

## Running with Docker

### Step 1: Build the Docker Image

Open a terminal in the project root directory and run:

```bash
docker build -t node-api-app .
```

**Explanation:**
- `docker build` - Command to build a Docker image
- `-t node-api-app` - Tags the image with the name "node-api-app"
- `.` - Uses the current directory as build context

### Step 2: Run the Docker Container

```bash
docker run -d -p 3000:3000 --name my-node-app node-api-app
```

**Explanation:**
- `docker run` - Command to run a container
- `-d` - Runs the container in detached mode (in the background)
- `-p 3000:3000` - Maps port 3000 on your host to port 3000 in the container
- `--name my-node-app` - Names the container "my-node-app"
- `node-api-app` - The name of the image to use

### Step 3: Test the API

Open another terminal or use your browser:

```bash
# Test the root endpoint
curl http://localhost:3000/

# Test the users endpoint
curl http://localhost:3000/users
```

Or open your browser and navigate to:
- `http://localhost:3000/`
- `http://localhost:3000/users`
