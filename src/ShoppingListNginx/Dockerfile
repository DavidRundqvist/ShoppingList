# Use official Nginx image (based on Debian/Alpine for Linux)
FROM nginx:latest

# Remove default nginx config
RUN rm /etc/nginx/conf.d/default.conf

# Copy our custom reverse proxy configuration
COPY nginx.conf /etc/nginx/nginx.conf
COPY ssl/server.crt /etc/nginx/ssl/server.crt
COPY ssl/server.key /etc/nginx/ssl/server.key

# Expose HTTPS port
EXPOSE 8130

# Start nginx in foreground
CMD ["nginx", "-g", "daemon off;"]
