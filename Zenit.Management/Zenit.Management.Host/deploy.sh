cmd=$1
tag=$(git describe --always)

usage() {
    echo "Usage: deploy.sh <command>"
    echo "Available commands:"
    echo "build             Build Docker image for the app and monitoring service"
    echo "up                Run app and monitoring service container"
}

if [[ -z "$cmd" ]]; 
then
    echo "Missing command"
    usage
    exit 1
fi

APP_IMAGE_NAME="zenit-management-api"

# Chuyển đến root folder của project
cd "$(dirname "$0")/../../" || exit 1

build() {
    docker build -f Zenit.Management/Zenit.Management.Host/Dockerfile -t $APP_IMAGE_NAME:$tag .
    docker tag $APP_IMAGE_NAME:$tag $APP_IMAGE_NAME:latest
}

up() {
    docker run -d --env-file Zenit.Management/Zenit.Management.Host/.env --network infrastructures_default -p 5212:5212 --name zenit-management-api $APP_IMAGE_NAME:latest
}

case $cmd in
    build)
        build
        ;;
    up)
        up
        ;;
    *)
        echo "Invalid command: $cmd"
        usage
        exit 1
        ;;
esac