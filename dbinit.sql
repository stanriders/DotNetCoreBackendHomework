CREATE TABLE IF NOT EXISTS public."TodoItems"
(
    "Id" uuid NOT NULL,
    "Title" text,
    "IsCompleted" boolean NOT NULL,
    CONSTRAINT "IdKey" PRIMARY KEY ("Id")
);