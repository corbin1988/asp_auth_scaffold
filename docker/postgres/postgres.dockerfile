FROM postgres:16

# Set environment variables for PostgreSQL
ENV POSTGRES_DB=mydatabase
ENV POSTGRES_USER=myuser
ENV POSTGRES_PASSWORD=mypassword

# Expose PostgreSQL port
EXPOSE 5432

# Install TimescaleDB for PostgreSQL 16
RUN apt-get update \
    && apt-get install -y wget gnupg postgresql-common lsb-release curl \
    && echo "deb https://packagecloud.io/timescale/timescaledb/debian/ $(lsb_release -cs) main" \
        | tee /etc/apt/sources.list.d/timescaledb.list \
    && curl -L https://packagecloud.io/timescale/timescaledb/gpgkey | apt-key add - \
    && apt-get update \
    && apt-get install -y timescaledb-2-postgresql-16 \
    && rm -rf /var/lib/apt/lists/*

# Run the PostgreSQL server with default entrypoint and CMD
CMD ["postgres"]
