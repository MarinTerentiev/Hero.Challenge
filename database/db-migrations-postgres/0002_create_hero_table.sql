CREATE TABLE IF NOT EXISTS herochallenge.public.heroimport (
    id SERIAL PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    class VARCHAR(255) NOT NULL,
    story TEXT,
    weapon VARCHAR(255) NOT NULL,
    seedId UUID NOT NULL
);
