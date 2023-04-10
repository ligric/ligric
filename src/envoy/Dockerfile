FROM envoyproxy/envoy:v1.25.4

RUN apt-get update

COPY ../envoy.yaml /etc/envoy/envoy.yaml

CMD /usr/local/bin/envoy -c /etc/envoy/envoy.yaml -l trace --log-path /tmp/envoy_info.log
