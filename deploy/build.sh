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
    cd Tasktower.$servicename/Tasktower.$servicename
    docker build \
        --no-cache \
        -t "tasktower-${servicename,,}" \
        .
    cd ../../
done
cd Tasktower.Manager/deploy
set -- $args