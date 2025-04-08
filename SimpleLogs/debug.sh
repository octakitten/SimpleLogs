#!/bin/bash
mv SimpleLogs.json SimpleLogs3.json.bak
mv SimpleLogs2.json.bak SimpleLogs.json
dotnet build --configuration Release
mv SimpleLogs.json SimpleLogs2.json.bak
mv SimpleLogs3.json.bak SimpleLogs.json