docker run --rm --network hero-challenge `
    -v "$(pwd)\database\db-seed-postgres\init.sql:/scripts/init.sql" `
    -e PGPASSWORD="testpassword" `
    postgres:latest bash -c "psql -h postgres -p 5432 -U testuser -d herochallenge -f /scripts/init.sql"