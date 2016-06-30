FROM ubuntu:16.04
ADD . /myapp
WORKDIR /myapp
# RUN sed -i 's/archive.ubuntu/cn.archive.ubuntu/' /etc/apt/sources.list && \
RUN echo "deb https://deb.nodesource.com/node_6.x xenial main" > /etc/apt/sources.list.d/nodesource.list && \
    apt-get update && \
    apt-get install -y mono-runtime build-essential libmono-2.0-dev mono-dmcs pkg-config python libfontconfig nodejs && \
    xz simsun.ttc.xz && \
    cp /myapp/simsun.ttc /usr/share/fonts/truetype/ && \
    npm install

EXPOSE 80
CMD node server.js
