SELECT 'CREATE DATABASE fox_stevenle' WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'fox_stevenle')\gexec

DO $$ BEGIN
    IF NOT EXISTS (SELECT * FROM pg_user WHERE usename = 'fox_stevenle') THEN
CREATE ROLE fox_stevenle LOGIN SUPERUSER password 'HteVuDclI4pMTiUMRp8fQN3wXfqMRf';
END IF;
END $$;