#!/bin/sh

echo "-----> Adding credentials JSON"
printf "$GCLOUD_CREDENTIALS" | base64 --decode > ./google/credentials.json
exec ./google/cloud_sql_proxy -credential_file=./google/credentials.json -instances=$GCLOUD_INSTANCE
echo "-----> Process end"

exit 0