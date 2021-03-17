#set -x
UIService="UIService"
UIService_Dockerfile_Localdir="Tasktower.UIService/Tasktower.UIService"

BoardService="BoardService"
BoardService_Dockerfile_Localdir="Tasktower.BoardService/Tasktower.BoardService"

OcelotAPIGateway="OcelotAPIGateway"
OcelotAPIGateway_Dockerfile_Localdir="Tasktower.OcelotAPIGateway/Tasktower.OcelotAPIGateway"

Servicenames=( \
    ${UIService} \
    ${BoardService} \
    ${OcelotAPIGateway} )

print_help()
{
    echo "Usage: build.sh [OPTIONS]"
    echo ""
    echo "Builds docker image, if no options specified all images are build"
    echo "Options:"
    echo "  -h                  help"
    echo "  -i                  list services"
    echo "                          ex) ./project-manager.sh -c \"checkout master\" "
    echo "  -s service          build a specific service"
}

print_service_names() {
    echo "service information"
    echo "-----------------------------"
    for servicename in ${Servicenames[@]};
    do
        echo ${servicename}
    done
}

build_image() {
    servicename=$1
    dockerfile_localdir=$2
    tmp_startdir=$(pwd)
    echo "----------------------------------------------------------------------"
    echo "Building $servicename"
    echo "----------------------------------------------------------------------"
    cd ${dockerfile_localdir}

    base_img="tasktower-${servicename,,}"
    tag=$(git log -1 --pretty=%H)

    img="${base_img}:${tag}"
    latest="${base_img}:latest"

    docker build --no-cache -t ${img} .
    docker tag ${img} ${latest}

    cd ${tmp_startdir}
} 

while getopts ihs: flag; do
    case "${flag}" in
        h)
            print_help
            exit 0
        ;;
        i)
            print_service_names
            exit 0
        ;;
        s) 
            if [[ " ${Servicenames[@]} " =~ " $OPTARG " ]]
            then
                Servicenames=( "$OPTARG" )
            else
                echo "$OPTARG not a valid service"
                exit -1
            fi
        ;;
    esac
done

cd ../../
for servicename in ${Servicenames[@]};
do
    declare -n dockerfile_path=${servicename}_Dockerfile_Localdir
    build_image ${servicename} $dockerfile_path
done
cd Tasktower.Manager/deploy
set -- $args