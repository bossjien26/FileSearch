FROM mongo:latest

ENV MONGO_INITDB_ROOT_USERNAME = root
ENV MONGO_INITDB_ROOT_PASSWORD = example
ENV MONGODB_DATABASE = article
COPY seeder.json /seeder.json
# COPY ["MongoDb/seeder.json","/seeder.json"]
# CMD mongoimport --authenticationDatabase admin --username root --password example -d media --collection token --file /seeder.json

# CMD mongoimport --host mongodb --db media --collection token --type json --file /seeder.json --jsonArray
# CMD ["mongod"]
# ENTRYPOINT [ "mongoimport","--authenticationDatabase admin --username root --password example --uri mongodb://127.0.0.1:27017/media --collection token --file /seeder.json" ]