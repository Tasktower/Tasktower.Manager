# Tasktower.Manager
This is a project and deployment manager for the tasktower app.

## Setting up Tasktower
#### <ins>Technologies you need</ins>
- Docker
- Git
- Bash shell (Git bash for windows users)
- .Net 5 sdk 
- Visual Studio 2019 or VS Code (visual studio 2019 can install .net 5 for you)
- Node.js
- SQL Server Managment Studio or Azure Data Studio

### 1. How to setup and manage projects with git 

To install all of the projects, in your bash terminal 
at the root of the project type `sh repo-manager.sh` .

This will clone all of your projects.

If you have visual studio, 
can open the `Tasktower.Manager.sln` file as your solution 
and all of the projects you installed will pop up.

The purpose of *repo-manager.sh* is to apply git commands 
to all projects at once. This makes it easy to keep all microservices
up to date. To keep all projects up to date with the master branch,
run:
```
sh repo-manager.sh -c " checkout master " # this checks out all projects to the master branch
sh repo-manager.sh -c " pull origin master " # this pulls all projects from the master branch
```

You can achieve the same for a specific project.
For example we will use *Tasktower.Migrator*.

```
sh repo-manager.sh -r Tasktower.Migrator -c " checkout master " # this checks out Tasktower.Migrator  to the master branch
sh repo-manager.sh -r Tasktower.Migrator  -c " pull origin master " # this pulls from the master branch
```
This appliess to all other git commands as well as the -c option allows you to pass any git command between quotes.

To see more details as to the type of arguments you can 
pass to `sh repo-manager.sh`, run `sh repo-manager.sh -h`.

### 2. Manage container deployments

Next is to build your docker images and run them locally. 
To go into the docker directory, run `cd docker`.

To build all of your custom docker images, you first need all of your 
projects installed and set up (refer to .you need to run:
```
sh ./build.sh
```
Once your images are up and running, you need to execute docker-compose
to start your containers. Run 
```
docker-compose -f docker-compose.develop.yml -p tasktower up -d
```

This will start all of your containers.

Keep in mind that docker compose may fail because the databases aren't properly initialized in sql server,
if that is the case, open Azure Data Studio or SQL Server Managment Studio and create your needed databases 
and rerun the docker compose command. 

Refer to docker-compose.develop.yml to see what port and credentials SQL Server is running on.

Once this is all set up, go into your browser to `http://localhost:8080` and if the UI is running, then we succesfully 
deployed tasktower. Localhost:8080 is where our api gateway is mapped to so you can use it to access our backend apis.

Later on we will switch to https deployment for better security.
