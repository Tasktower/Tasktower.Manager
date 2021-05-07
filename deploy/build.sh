#set -x

UIService="UIService"
BoardService="BoardService"
OcelotGateway="OcelotGateway"
SQLServerDatabase="SQLServerDatabase"
Migrator="Migrator"

Servicenames=( \
  ${UIService} \
  ${BoardService} \
  ${OcelotGateway} \
  ${SQLServerDatabase} \
  ${Migrator})

print_help()
{
  echo "Usage: build.sh [OPTIONS]"
  echo ""
  echo "Builds docker image, if no options specified all images are build"
  echo "Options:"
  echo "  -h                  help"
  echo "  -i                  list services"
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
  tmp_startdir=$(pwd)
  echo "----------------------------------------------------------------------"
  echo "Building $servicename"
  echo "----------------------------------------------------------------------"
  cd "Tasktower.${servicename}"

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
    *)
      print_help
      exit -1
    ;;
  esac
done

cd ../../
for servicename in ${Servicenames[@]};
do
  build_image ${servicename}
done
cd Tasktower.Manager/deploy
set -- $args