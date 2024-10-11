# BitwiseMind HolidaysAndClosures

## Description 
BitwiseMind HolidaysAndClosures is a utility library that handles public holidays and closures for various 
European countries. It provides classes to determine whether a specific date is a holiday or a closure date, 
making it ideal for applications that need to handle business operations around regional public holidays. 
Additionally, it includes timezone support for the UK, Romania, and other European regions, ensuring 
accurate date and time management across countries.

## Installation

For local development, it is necessary to set up and run the Docker services, including Redis and RedisInsight, follow these steps:

### Prerequisites
- Install [Docker](https://docs.docker.com/get-docker/) on your machine. You can download and install Docker on multiple platforms.
- Install [Docker Compose](https://docs.docker.com/compose/install/). The easiest and recommended way to get Docker Compose is to install Docker Desktop. Docker Desktop includes Docker Compose along with Docker Engine and Docker CLI which are Compose prerequisites.

These tools are required for local development to debug code and applications that will use the HolidaysAndClosures library.

### Step-by-Step Installation

1. **What is Installed**

   The setup includes two services Redis and RedisInsight, that are defined in the `docker-compose.yaml` file. 

- **Redis**: A NoSQL in-memory data structure store used as a database, cache, and message broker. The Redis container (`redis/redis-stack-server:latest`) runs on port 6379 and persists data to the host using mounted volumes.
- **RedisInsight**: A visualization and management tool for Redis. It depends on the Redis service and runs on port 5540. Data is persisted to the host using mounted volumes.

Each service is built as a Docker container to ensure proper separation and scalability.

2. **Configure Environment Variables**

   Update the `.env` file with the necessary environment variables if you want to further customize `docker-compose.yaml`. The `.env` file may include configurations such as API keys, database credentials, and other service-specific settings.

3. **Build and Start Docker Services**

   Use the provided batch file to build and start the services:
   ```sh
   build-docker-compose.bat
   ```
   This script will execute Docker Compose commands to build the images and start all the services as defined in the `docker-compose.yaml` file.

4. **Verify Services**

   Check that all services are up and running by using Docker Compose logs:
   ```sh
   docker-compose logs
   ```

### Stopping Services

To stop the running services, use the following command:
```sh
docker-compose down
```
This command will stop and remove all containers created by the `docker-compose.yaml` file.