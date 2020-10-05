FROM node:latest as build
WORKDIR /app

RUN npm install -g @angular/cli

COPY ./package.json .
RUN npm install
COPY . .
RUN ng build --prod

FROM nginx as runtime
COPY ./nginx.conf /etc/nginx/conf.d/nginx.conf

RUN rm -rf /usr/share/nginx/html/*
COPY --from=build /app/dist /usr/share/nginx/html