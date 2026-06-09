pipeline {
    agent any
 
    environment {
        CONTAINER_NAME = 'football-web-api'
        IMAGE_NAME = 'football-backend'
        NETWORK_NAME = 'football-network'
        DB_CONTAINER = 'football-mysql-db'
        PORT_MAPPING = '5162:8080'
        DB_CONNECTION = "Server=football-mysql-db;Port=3306;Database=WorldCupPollDb;User=root;Password=root;"
        JWT_SECRET = 'SuperSecretKeyForFootballAppAuthJWTToken2026'
        JWT_ISSUER = 'FootballApi'
        JWT_AUDIENCE = 'FootballUi'
    }
 
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
 
        stage('Build Docker Image') {
            steps {
                bat "docker build --no-cache -t ${IMAGE_NAME}:latest -t ${IMAGE_NAME}:${BUILD_NUMBER} ."
            }
        }
 
        stage('Deploy Container') {
            steps {
                script {
                    // Ensure Docker network exists (ignoring error if it already exists)
                    bat "docker network create ${NETWORK_NAME} 2>nul || ver >nul"
                    
                    // Start the MySQL database if not already running (Internal network only, no host port binding to avoid collisions)
                    bat "docker start ${DB_CONTAINER} 2>nul || docker run -d --name ${DB_CONTAINER} --network ${NETWORK_NAME} -e MYSQL_ROOT_PASSWORD=root -e MYSQL_DATABASE=WorldCupPollDb mysql:8.0"
                    
                    // Give MySQL 15 seconds to initialize before the API tries to run auto-migrations
                    sleep time: 15, unit: 'SECONDS'
                   
                    // Stop and remove existing container if running
                    bat "docker stop ${CONTAINER_NAME} 2>nul || ver >nul"
                    bat "docker rm ${CONTAINER_NAME} 2>nul || ver >nul"
                   
                    // Launch new container using Windows Batch line continuation
                    bat """
                        docker run -d ^
                            --name ${CONTAINER_NAME} ^
                            --network ${NETWORK_NAME} ^
                            -p ${PORT_MAPPING} ^
                            -e ConnectionStrings__DefaultConnection="${DB_CONNECTION}" ^
                            -e Jwt__Key="${JWT_SECRET}" ^
                            -e Jwt__Issuer="${JWT_ISSUER}" ^
                            -e Jwt__Audience="${JWT_AUDIENCE}" ^
                            ${IMAGE_NAME}:latest
                    """
                }
            }
        }
    }
 
    post {
        success {
            echo "Backend pipeline completed successfully!"
        }
        failure {
            echo "Backend pipeline failed. Please check the logs."
        }
    }
}
