FROM maven:3.6.3-jdk-11 AS MAVEN_BUILD
 
# copy the source tree and the pom.xml to our new container
COPY ./realt-stats-api ./
 
# package our application code
RUN mvn clean package

# the second stage of our build will us
FROM openjdk:11-jre-slim 
 
# copy only the artifacts we need from the first stage and discard the rest
COPY --from=MAVEN_BUILD target/realt-stats-api-0.0.1-SNAPSHOT.jar /app.jar

RUN ls -la
 
# set the startup command to execute the jar
CMD ["java", "-jar", "app.jar"]
