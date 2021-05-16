# set -x
baseDir=$(basename "$PWD")
repositories=( \
    "Tasktower-SQL-Server-Database" \
    "Tasktower-Ocelot-Gateway" \
    "Tasktower-Migrator" \
    "Tasktower-Project-Service" \
    "Tasktower-UI-Service" )
HELP_FLAG=false
BRANCH=master
GIT_COMMAND=" status "

help()
{
  echo "Usage: gitmanager.sh [OPTIONS]"
  echo "Options:"
  echo "  -h                  help"
  echo "  -c git_command      git command fot your repositories"
  echo "                          ex) ./project-manager.sh -c \"checkout master\" "
  echo "  -r repository       project repository e.g) ./project-manager.sh -r Tasktower-Project-Service"
  echo ""
  list_repositories
}

list_repositories()
{
  echo "Repositories"
  echo "-------------------"
  for repo in ${repositories[@]}; do
    echo "${repo}"
  done
}

manageRepositories() 
{
  echo "__________ MANAGING REPOSITORIES __________"
  cd ../
  for repo in ${repositories[@]}; do
    echo "__________Working on ${repo}__________"
    [ ! -d ${repo} ] && git clone https://github.com/Tasktower/${repo} 
    cd ./${repo}
    git ${GIT_COMMAND}   
    cd ../
  done
  cd ${baseDir}
  echo "__________ Finish working on repositories __________"
}

while getopts r:c:b:h flag; do
  case "${flag}" in
    h) 
      help
      exit 0
    ;;
    b) 
      BRANCH=$OPTARG
    ;;
    c) 
      GIT_COMMAND=$OPTARG
    ;;
    r)
      if [[ " ${repositories[@]} " =~ " $OPTARG " ]]
      then
        echo "repository [$OPTARG] chosen"
      else
        echo "ERROR: repository [$OPTARG] not used"
        exit
      fi
      repositories=( "$OPTARG" )
    ;;
    *)
      help
      exit 1
    ;;
  esac
done


manageRepositories
