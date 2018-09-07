# Build Image
FROM node:10-alpine AS build

WORKDIR /usr/app

COPY package.json .
COPY package-lock.json .
COPY tsconfig.json .
COPY angular.json .

RUN npm install

COPY src src

RUN npm run build-prod

# Runtime Image
FROM nginx:latest

COPY --from=build /usr/app/dist/games-library-web /usr/share/nginx/html
