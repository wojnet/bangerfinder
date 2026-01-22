# Stage 1: Build - Updated to Node 22 to satisfy Next.js requirements
FROM node:22 AS build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
# This runs the build script defined in your package.json
RUN npm run build 

# Stage 2: Serve
FROM nginx:alpine
# As discussed, Next.js builds into 'out' for static exports
COPY --from=build /app/out /usr/share/nginx/html 
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]