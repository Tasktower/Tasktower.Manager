# set -x
baseDir=$(basename "$PWD")
repositories=( "Tasktower.Webutils" "Tasktower.UserService" "Tasktower.NginxGateway" "Tasktower.Migrator" )
HELP_FLAG=false
BRANCH=master
GIT_COMMAND=" status "

help_and_exit()
{
    echo "Usage: gitmanager.sh [OPTIONS]"
    echo "Options:"
    echo "  -h                  help"
    echo "  -c git_command      git command fot your repositories"
    echo "                          ex) ./project-manager.sh -c \"checkout master\" "
    echo "  -r repository       project repository e.g) ./project-manager.sh -r Tasktower.Webutils"
    exit 0
}

manageRepositories() 
{
    echo "__________ MANAGING UP REPOSITORIES __________"
    cd ../
    for repo in ${repositories[@]}; do
        echo "__________Seting up ${repo}__________"
        [ ! -d ${repo} ] && git clone https://github.com/Tasktower/${repo} 
        cd ./${repo}
        git ${GIT_COMMAND}   
        cd ../
    done
    cd ${baseDir}
    echo "__________ Finish managing up repositories __________"
}

while getopts r:c:b:h flag; do
    case "${flag}" in
        h) 
            help_and_exit
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
                echo "repository [$OPTARG] chosem"
            else
                echo "ERROR: repository [$OPTARG] not used"
                exit
            fi
            repositories=( "$OPTARG" )
        ;;
    esac
done


manageRepositories
