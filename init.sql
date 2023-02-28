-- Adminer 4.8.1 PostgreSQL 15.1 (Debian 15.1-1.pgdg110+1) dump

\connect "e-commerce";

CREATE TABLE "public"."Changelogs" (
    "Id" uuid NOT NULL,
    "Method" character varying(6),
    "TableName" character varying(50),
    "KeyValues" text,
    "NewValues" text,
    "OldValues" text,
    "ChangeBy" text,
    "ChangeDate" integer,
    CONSTRAINT "PK_Changelogs" PRIMARY KEY ("Id")
) WITH (oids = false);


CREATE TABLE "public"."OrderProducts" (
    "OrderID" uuid NOT NULL,
    "ProductId" uuid NOT NULL,
    "Quantity" integer NOT NULL,
    CONSTRAINT "PK_OrderProducts" PRIMARY KEY ("ProductId", "OrderID")
) WITH (oids = false);

CREATE INDEX "IX_OrderProducts_OrderID" ON "public"."OrderProducts" USING btree ("OrderID");


CREATE TABLE "public"."Orders" (
    "Id" uuid NOT NULL,
    "Status" text,
    "TotalPaid" integer NOT NULL,
    "VoucherId" uuid,
    "CreatedBy" uuid,
    "CreatedDate" bigint NOT NULL,
    "UpdatedBy" uuid,
    "UpdatedDate" bigint,
    "IsDeleted" boolean NOT NULL,
    CONSTRAINT "PK_Orders" PRIMARY KEY ("Id")
) WITH (oids = false);

CREATE INDEX "IX_Orders_VoucherId" ON "public"."Orders" USING btree ("VoucherId");


CREATE TABLE "public"."Products" (
    "Id" uuid NOT NULL,
    "Name" character varying(50),
    "Price" bigint,
    "CreatedBy" uuid,
    "CreatedDate" bigint NOT NULL,
    "UpdatedBy" uuid,
    "UpdatedDate" bigint,
    "IsDeleted" boolean NOT NULL,
    CONSTRAINT "PK_Products" PRIMARY KEY ("Id")
) WITH (oids = false);


CREATE TABLE "public"."TodoItems" (
    "Id" uuid NOT NULL,
    "ListId" uuid NOT NULL,
    "Title" character varying(200) NOT NULL,
    "Note" text,
    "Priority" integer NOT NULL,
    "Reminder" bigint,
    "Done" boolean NOT NULL,
    "CreatedBy" character varying(100),
    "CreatedDate" bigint NOT NULL,
    "UpdatedBy" character varying(100),
    "UpdatedDate" integer,
    "IsDeleted" boolean NOT NULL,
    CONSTRAINT "PK_TodoItems" PRIMARY KEY ("Id")
) WITH (oids = false);

CREATE INDEX "IX_TodoItems_ListId" ON "public"."TodoItems" USING btree ("ListId");


CREATE TABLE "public"."TodoLists" (
    "Id" uuid NOT NULL,
    "Title" character varying(200) NOT NULL,
    "Colour_Code" text NOT NULL,
    "CreatedBy" character varying(100),
    "CreatedDate" bigint NOT NULL,
    "UpdatedBy" character varying(100),
    "UpdatedDate" integer,
    "IsDeleted" boolean NOT NULL,
    CONSTRAINT "PK_TodoLists" PRIMARY KEY ("Id")
) WITH (oids = false);


CREATE TABLE "public"."Voucher" (
    "Id" uuid NOT NULL,
    "Name" character varying(50),
    "CreatedBy" uuid,
    "CreatedDate" bigint NOT NULL,
    "UpdatedBy" uuid,
    "UpdatedDate" bigint,
    "IsDeleted" boolean NOT NULL,
    CONSTRAINT "PK_Voucher" PRIMARY KEY ("Id")
) WITH (oids = false);


CREATE TABLE "public"."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
) WITH (oids = false);


ALTER TABLE ONLY "public"."OrderProducts" ADD CONSTRAINT "FK_OrderProducts_Orders_OrderID" FOREIGN KEY ("OrderID") REFERENCES "Orders"("Id") ON DELETE CASCADE NOT DEFERRABLE;
ALTER TABLE ONLY "public"."OrderProducts" ADD CONSTRAINT "FK_OrderProducts_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products"("Id") ON DELETE CASCADE NOT DEFERRABLE;

ALTER TABLE ONLY "public"."Orders" ADD CONSTRAINT "FK_Orders_Voucher_VoucherId" FOREIGN KEY ("VoucherId") REFERENCES "Voucher"("Id") NOT DEFERRABLE;

ALTER TABLE ONLY "public"."TodoItems" ADD CONSTRAINT "FK_TodoItems_TodoLists_ListId" FOREIGN KEY ("ListId") REFERENCES "TodoLists"("Id") ON DELETE CASCADE NOT DEFERRABLE;

-- 2023-02-23 12:08:09.780254+00
