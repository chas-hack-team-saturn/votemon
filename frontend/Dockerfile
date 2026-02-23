FROM node:22-alpine AS builder
WORKDIR /app
COPY ./frontend/package.json package-lock.json ./
RUN npm install
COPY ./frontend/ ./
RUN npx vite build

FROM nginx:alpine

COPY --from=builder /app/dist /usr/share/nginx/html

RUN echo 'server { listen 80; root /usr/share/nginx/html; index index.html; location / { try_files $uri $uri/ /index.html; } }' > /etc/nginx/conf.d/default.conf

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
