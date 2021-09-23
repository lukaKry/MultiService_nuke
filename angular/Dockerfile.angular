#stage 1
FROM node:16-alpine as node
WORKDIR /usr/src/app
COPY package*.json .
RUN npm install
COPY . .
RUN npm run build

#stage 2
FROM nginx:1.21.3-alpine
COPY --from=node /usr/src/app/dist/demo /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/conf.d/default.conf