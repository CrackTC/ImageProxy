cd $(dirname $0)
dotnet publish -c Release --runtime alpine-x64
cp ./bin/Release/net7.0/alpine-x64/publish/imageproxy ./app/
docker buildx build .
