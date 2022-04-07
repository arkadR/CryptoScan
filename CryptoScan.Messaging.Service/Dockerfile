FROM maven:3.8.3-openjdk-11-slim as MAVEN_BUILD
COPY pom.xml /build/
COPY src /build/src/
WORKDIR /build/
RUN mvn package

FROM openjdk:11
WORKDIR /app
COPY --from=MAVEN_BUILD /build/target/messaging-0.0.1-SNAPSHOT.jar /app
EXPOSE 8004
ENTRYPOINT ["java", "-jar", "messaging-0.0.1-SNAPSHOT.jar"]