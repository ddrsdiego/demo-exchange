FROM bitnami/mariadb:latest
ENV MARIADB_PASSWORD=admin
ENV MARIADB_ROOT_PASSWORD=admin
COPY sql/Scripts/*.sql /docker-entrypoint-initdb.d/
