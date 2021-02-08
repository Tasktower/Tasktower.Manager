FROM nginx

RUN apt-get update 
RUN apt-get install -y git
RUN git clone https://github.com/Tasktower/Tasktower.NginxGateway
WORKDIR "/Tasktower.NginxGateway"
RUN chmod -R 777 ./*
RUN cp nginx.conf /etc/nginx/nginx.conf
CMD ["bash", "entrypoint.sh"]