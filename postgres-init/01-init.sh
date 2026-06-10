
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname postgres \
  --set quinela_pw="$QUINELA_DB_PASSWORD" \
  --set userapp_pw="$USERAPP_DB_PASSWORD" <<-'EOSQL'
  CREATE ROLE user_quinela LOGIN PASSWORD :'quinela_pw';
  CREATE ROLE user_userapp LOGIN PASSWORD :'userapp_pw';

  CREATE DATABASE quinela OWNER user_quinela;
  CREATE DATABASE userapp OWNER user_userapp;
EOSQL

echo ">>> Roles y bases creados (user_quinela, user_userapp). El esquema lo crean las migraciones EF."
