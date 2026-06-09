pipeline {
    agent any

    environment {
        DOCKER_IMAGE = 'football-api'
        DOCKER_TAG = "v${env.BUILD_NUMBER}"
        REGISTRY = 'your-docker-registry.com'
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
        
        stage('Build Docker Image') {
            steps {
                bat "docker build -t ${REGISTRY}/${DOCKER_IMAGE}:${DOCKER_TAG} -f Dockerfile ."
            }
        }
        
        stage('Push Docker Image') {
            steps {
                withCredentials([usernamePassword(credentialsId: 'docker-credentials-id', passwordVariable: 'DOCKER_PASS', usernameVariable: 'DOCKER_USER')]) {
                    bat "echo %DOCKER_PASS% | docker login ${REGISTRY} -u %DOCKER_USER% --password-stdin"
                    bat "docker push ${REGISTRY}/${DOCKER_IMAGE}:${DOCKER_TAG}"
                    bat "docker tag ${REGISTRY}/${DOCKER_IMAGE}:${DOCKER_TAG} ${REGISTRY}/${DOCKER_IMAGE}:latest"
                    bat "docker push ${REGISTRY}/${DOCKER_IMAGE}:latest"
                }
            }
        }
        
        stage('Deploy') {
            steps {
                echo "Deploying ${DOCKER_IMAGE}:${DOCKER_TAG}..."
            }
        }
    }
    
    post {
        always {
            cleanWs()
        }
    }
}
