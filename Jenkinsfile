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
                script {
                    dockerImage = docker.build("${REGISTRY}/${DOCKER_IMAGE}:${DOCKER_TAG}", "-f Dockerfile .")
                }
            }
        }
        
        stage('Push Docker Image') {
            steps {
                script {
                    docker.withRegistry("https://${REGISTRY}", 'docker-credentials-id') {
                        dockerImage.push()
                        dockerImage.push('latest')
                    }
                }
            }
        }
        
        stage('Deploy') {
            steps {
                // In a real scenario, you might ssh into the server and run docker-compose pull && docker-compose up -d
                // Or use kubectl if deploying to Kubernetes.
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
