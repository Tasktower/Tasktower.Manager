# generate certs
rm ../docker/volumes/https/aspnetapp.pfx
dotnet dev-certs https -ep ../docker/volumes/https/aspnetapp.pfx -p password
dotnet dev-certs https -ep ../docker/volumes/https/aspnetapp.pfx --trust
