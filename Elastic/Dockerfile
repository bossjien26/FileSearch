FROM docker.elastic.co/elasticsearch/elasticsearch:7.14.0-arm64

# ARG ELASTIC_VERSION
# ENV VERSION=${ELASTIC_VERSION}
ENV ELASTIC_PASSWORD=test
ADD https://github.com/medcl/elasticsearch-analysis-ik/releases/download/v7.14.0/elasticsearch-analysis-ik-7.14.0.zip /tmp/
RUN /usr/share/elasticsearch/bin/elasticsearch-plugin install --batch file:///tmp/elasticsearch-analysis-ik-7.14.0.zip

RUN rm -rf /tmp/*

# RUN bin/elasticsearch-certutil ca --out config/elastic-stack-ca.p12 --pass 123456

# RUN bin/elasticsearch-certutil cert --ca config/elastic-stack-ca.p12 --ca-pass 123456 --out config/elastic-certificates.p12 --pass 123456


# RUN bin/elasticsearch-keystore create

# RUN sh -c '/bin/echo -e "123456" | sh bin/elasticsearch-keystore add xpack.security.transport.ssl.keystore.secure_password'

# RUN sh -c '/bin/echo -e "123456" | sh bin/elasticsearch-keystore add xpack.security.transport.ssl.truststore.secure_password'

# RUN chmod 777 /usr/share/elasticsearch/config/elastic-certificates.p12

# RUN chmod 777 /usr/share/elasticsearch/config/elastic-stack-ca.p12