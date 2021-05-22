# Tasktower-Manager
This is a project and deployment manager for the tasktower app.

## Setting up Tasktower
#### <ins>Technologies you need</ins>
- Docker
- Git
- Bash shell (Git bash for windows users)
- .Net 5 sdk 
- Visual Studio 2019, Jet Brains Rider, or VS Code (visual studio 2019 can install .net 5 for you)
- Node.js
- SQL Server Managment Studio, Azure Data Studio, or DataGrip

### - Set up Nuke
Nuke is a build automation tool which we will use to manage our application development.
It offers uses such as git managment, running builds and tests, interfacing with docker, 
as well as handling deployment.

To install nuke, please go to Tasktower-Manager root directory in your bash shell and run:
```shell
chmod +x install.sh 
sh ./install.sh
```
You will now have nuke installed.
To confirm, run `nuke --help` and you will see the help text on how to use it.
For now don't worry too much about all of these commands, we will come back to them later.

To see all the services/projects your development environment will use, 
run `nuke ListServices`

### - How to setup and manage projects with git 

To install all of the services/projects, in your bash terminal 
at the root of the project type `nuke GitRun --git-clone-branch master`.

This will clone all of your projects to master. By default all projects
will clone to master. So you can ommit `--git-clone-branch` and run `nuke gitrun`
on it's own.

If you have visual studio or jetbrains rider, yiu can open the 
`Tasktower.Manager.sln` file as your solution and all of the projects 
you installed will pop up.

Here is some more commands to get you more acquainted with nuke.

How you can create a new branch:
```shell
nuke GitRun --git-command 'checkout -b new_branch'
```

How you can stage files for a specific service found in ListServices:
```shell
nuke GitRun --git-command 'add .' --service-name <some_service>
```

As you can see, the `--git-command` specifies a git command for you to use.

By default, git commands apply to all services, however to apply git commands
to one service, you need to specify it. Each service is it's own git repository.

To commit, or do any other commands with double quotes, you need to escape them from `"` to `//"`.
For example:

```shell
nuke GitRun --git-command 'commit -m \\"hello commit message\\"' --service-name <some_service>
```

### - Manage container deployments

Next is to build your docker images and run them locally. 

To build all of your custom docker images, you first need all of your 
projects installed and set up. 

Refer to the the previous section in git managment for more details.

To start building all of your docker images, you first need to publish 
your projects:
```shell
nuke DotnetPublish
```

_Note: If you are using windows, make sure the shell and sql files
in Migrator and SQLServerDatabase are using LF line endings and not CLRF. 
If they don't, the docker images you built with them won't work._

Once your images are up and running, you need to execute docker-compose
to start your containers. Run 
```shell
cd deploy
docker-compose -f docker-compose-develop.yml -p tasktower up -d
```

This will start all of your containers.

Keep in mind that docker compose may fail because the databases initialized yet in sql server,
if that is the case, just rerun the previous docker command when the databases are initialized.

You can use Azure Data Studio or SQL Server Managment Studio to check you sql server instance.

Refer to docker-compose.develop.yml to see what port and credentials SQL Server is running on.

Once this is all set up, go into your browser to `http://localhost:9090` and if the UI is running, then we successfully 
deployed tasktower. Localhost:9090 is where our api gateway is mapped to so you can use it to access our backend apis.
