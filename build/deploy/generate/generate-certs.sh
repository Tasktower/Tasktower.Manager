# generate certs
#rm ../docker/volumes/https/Tasktower.OcelotGateway.pfx
#dotnet dev-certs https -ep ../docker/volumes/https/Tasktower.OcelotGateway.pfx -p crypticpassword
#dotnet dev-certs https -ep ../docker/volumes/https/Tasktower.OcelotGateway.pfx --trust

rm -rf ../volumes/https
dotnet dev-certs https -ep ${HOME}/.aspnet/https/tasktower.pfx -p crypticpassword
dotnet dev-certs https --trust
cd ../docker/volumes
mkdir https
cp ${HOME}/.aspnet/https/tasktower.pfx ./https/