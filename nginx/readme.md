# 🌐 Nginx Web App (Dockerized)

This project demonstrates how to build and run a simple Nginx-based web application using Docker. Follow the steps below to get the application up and running locally.

---

## 📦 Prerequisites

Make sure you have the following installed on your system:

* Docker (latest version recommended)
* Terminal / Command Line access

To verify Docker installation:
docker --version

---

## 🚀 Getting Started

### 1. Navigate to Project Directory

Open your terminal and move into the project folder:

cd path/to/nginx-project

---

### 2. Build Docker Image

Build the Docker image using the following command:

docker build -t web_app_image:latest .

---

### 3. Verify Image Creation

Check if the image has been successfully created:

docker image ls

You should see `web_app_image` listed.

---

### 4. Run the Docker Container

Start a container from the image:

docker run -d -p 8080:80 --name my_web_app web_app_image:latest

* `-d` → Run in detached mode
* `-p 8080:80` → Map port 8080 (host) to port 80 (container)
* `--name` → Assign a name to the container

---

### 5. Access the Application

Open your browser and go to:

[http://localhost:8080](http://localhost:8080)

You should see your Nginx web app running 🎉

---

## 🛠️ Useful Commands

### Stop the Container

docker stop my_web_app

### Start the Container

docker start my_web_app

### Remove the Container

docker rm my_web_app

### Remove the Image

docker rmi web_app_image:latest

---

## 📁 Project Structure (Example)

nginx-project/
│── Dockerfile
│── index.html
│── nginx.conf (optional)

---

## 📌 Notes

* Ensure port `8080` is not already in use.
* Modify the Dockerfile or Nginx configuration as needed.
* You can change the exposed port by editing the `docker run` command.

---

## 🧑‍💻 Author

Your Name

---

## 📄 License

This project is licensed under the MIT License.
