# Imagen de Postgres con el script de inicializacion horneado adentro.
# (Montarlos desde el Mac falla al ejecutarlos; copiarlo a la imagen lo resuelve
#  y deja la imagen autocontenida para desplegar en el server.)
FROM postgres:17-alpine

COPY 01-init.sh /docker-entrypoint-initdb.d/01-init.sh

RUN chmod +x /docker-entrypoint-initdb.d/01-init.sh
