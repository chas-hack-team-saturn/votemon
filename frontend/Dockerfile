FROM node:22-alpine AS builder
WORKDIR /app
COPY ./frontend/package.json package-lock.json ./
RUN npm install
COPY ./frontend/ ./
RUN npx vite build

FROM caddy:2-alpine

COPY --from=builder /app/dist /var/www/html
