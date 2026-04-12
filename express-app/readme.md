Here's a comprehensive README file for your Node.js Express project:

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
| GET | `/` | Returns "Hello world" message |
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

## Useful Docker Commands

### View running containers
```bash
docker ps
```

### View container logs
```bash
docker logs my-node-app

# Follow logs in real-time
docker logs -f my-node-app
```

### Stop the container
```bash
docker stop my-node-app
```

### Start an existing container
```bash
docker start my-node-app
```

### Remove the container
```bash
docker rm my-node-app
```

### Remove the image
```bash
docker rmi node-api-app
```

### Run container in interactive mode (for debugging)
```bash
docker run -it -p 3000:3000 node-api-app
```

## Running Locally (without Docker)

If you have Node.js installed on your system:

1. Install dependencies:
```bash
npm install
```

2. Run the application:
```bash
node src/index.js
```

3. Access the API at `http://localhost:3000`

## Environment Variables

The application uses the following port configuration:

| Variable | Default | Description |
|----------|---------|-------------|
| port | 3000 | The port the server listens on |

To change the port when running with Docker:

```bash
# First, modify the port in index.js or use environment variable
docker run -d -p 8080:3000 --name my-node-app node-api-app
```

This maps host port 8080 to container port 3000. Access the API at `http://localhost:8080`

## Troubleshooting

### Port already in use error
If you see `port is already allocated` error, either:
- Stop the existing container using that port
- Use a different host port: `-p 3001:3000`

### Container exits immediately
Check the logs for errors:
```bash
docker logs my-node-app
```

### Permission denied for Docker commands
Run Docker commands with `sudo` or add your user to the Docker group:
```bash
sudo usermod -aG docker $USER
```

## Cleaning Up

To remove all unused containers, images, and networks:

```bash
# Remove stopped containers
docker container prune

# Remove unused images
docker image prune

# Remove everything (containers, images, networks, build cache)
docker system prune -a
```

## License

MIT

## Support

For issues or questions, please check the Docker logs or verify your Node.js installation.
```

This README provides:
- Clear project overview
- Step-by-step Docker build and run instructions
- Useful Docker commands reference
- Troubleshooting tips
- Local development options
- API documentation