-- Create Keyspace
CREATE KEYSPACE IF NOT EXISTS herochallenge WITH REPLICATION = { 'class' : 'SimpleStrategy', 'replication_factor' : 1 };

-- Create Table
CREATE TABLE IF NOT EXISTS herochallenge.hero (
    id int PRIMARY KEY,
    name text,
    class text,
    story text,
    weapon text
);
