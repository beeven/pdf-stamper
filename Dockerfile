FROM ubuntu:16.04
ADD . /myapp
WORKDIR /myapp
# RUN sed -i 's/archive.ubuntu/cn.archive.ubuntu/' /etc/apt/sources.list && \
RUN apt-get update && \
    apt-get install -y wget && \
    wget -qO- https://deb.nodesource.com/setup_6.x | bash - && \
    apt-get update && \
    apt-get install -y mono-runtime build-essential libmono-2.0-dev mono-dmcs pkg-config python libfontconfig nodejs nano && \
    xz -d simsun.ttc.xz && \
    cp /myapp/simsun.ttc /usr/share/fonts/truetype/ && \
    npm install

EXPOSE 80
CMD node server.js
