servicenames=( \
    "UIService" \
    "BoardService" \
    "OcelotAPIGateway" )

if [[ " ${servicenames[@]} " =~ " $1 " ]]
then
servicenames=( "$1" )
fi

cd ../../
for servicename in ${servicenames[@]};
do
    echo "----------------------------------------------------------------------"
    echo "Building $servicename"
    echo "----------------------------------------------------------------------"
    docker build \
        --no-cache \
        -f ./Tasktower.Manager/docker/dockerfiles/$servicename/Dockerfile  \
        -t "tasktower-${servicename,,}" \
        .
done
cd Tasktower.Manager/docker
set -- $args