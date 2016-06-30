FROM ubuntu:16.04
ADD . /myapp
WORKDIR /myapp
RUN cat /etc/apt/sources.list | sed 's/archive.ubuntu/cn.archive.ubuntu/' > sources.list2 &&
    mv /etc/apt/sources.list2 /etc/apt/sources.list &&
    apt-get update &&
    apt-get install -y wget mono-runtime libmono-2.0-dev mono-dmcs pkg-config python libfontconfig &&
    wget -qO- https://deb.nodesource.com/setup_6.x | bash - &&
    apt-get install -y nodejs &&
    xz simsun.ttc.xz &&
    cp /myapp/simsun.ttc /usr/share/fonts/truetype/ &&
    npm install
CMD node server.js
