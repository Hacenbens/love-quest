pipeline {
    agent { label 'dotnet' }

    environment {
        DOTNET_CLI_TELEMETRY_OPTOUT = '1'
        DOTNET_NOLOGO = '1'
        CONFIGURATION = 'Release'
    }

    options {
        buildDiscarder(logRotator(numToKeepStr: '10'))
    }

    stages {
        stage('Checkout') {
            steps { checkout scm }
        }
        stage('Restore') {
            steps { sh 'dotnet restore LoveQuest.slnx' }
        }
        stage('Build') {
            steps { sh 'dotnet build LoveQuest.slnx --configuration $CONFIGURATION --no-restore' }
        }
        stage('Test') {
            steps {
                sh '''dotnet test LoveQuest.slnx --configuration $CONFIGURATION --no-build \
                      --logger "trx;LogFileName=results.trx" --results-directory TestResults'''
            }
        }
        stage('Publish') {
            steps {
                sh 'dotnet publish src/LoveQuest.Web/LoveQuest.Web.csproj --configuration $CONFIGURATION --no-build --output publish'
            }
        }
        stage('Package Artifact') {
            steps {
                sh 'tar -czf love-quest-publish.tar.gz -C publish .'
            }
        }
        stage('Archive') {
            steps {
                archiveArtifacts artifacts: 'publish/**,love-quest-publish.tar.gz', fingerprint: true
            }
        }
    }

    post {
        always { archiveArtifacts artifacts: 'TestResults/**/*.trx', allowEmptyArchive: true }
    }
}
