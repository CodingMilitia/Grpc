#!/bin/sh

cp ~/.nuget/packages/grpc.tools/1.10.0/tools/macosx_x64/protoc .
cp ~/.nuget/packages/grpc.tools/1.10.0/tools/macosx_x64/grpc_csharp_plugin .
rm -R Generated
mkdir Generated
./protoc service.proto --csharp_out ./Generated/. --grpc_out ./Generated/. --plugin=protoc-gen-grpc=grpc_csharp_plugin
rm protoc
rm grpc_csharp_plugin