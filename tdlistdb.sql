--
-- PostgreSQL database dump
--

-- Dumped from database version 16.1
-- Dumped by pg_dump version 16.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- Name: Tasks; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Tasks" (
    "Id" uuid NOT NULL,
    "Title" text NOT NULL,
    "Description" text NOT NULL,
    "Deadline" timestamp with time zone NOT NULL,
    "Priority" integer NOT NULL,
    "Status" integer NOT NULL,
    "UserId" uuid NOT NULL
);


ALTER TABLE public."Tasks" OWNER TO postgres;

--
-- Name: Users; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Users" (
    "Id" uuid NOT NULL,
    "Username" text NOT NULL,
    "PasswordHash" text NOT NULL,
    "RefreshToken" text NOT NULL,
    "TokenCreated" timestamp with time zone NOT NULL,
    "TokenExpires" timestamp with time zone NOT NULL
);


ALTER TABLE public."Users" OWNER TO postgres;

--
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO postgres;

--
-- Data for Name: Tasks; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Tasks" ("Id", "Title", "Description", "Deadline", "Priority", "Status", "UserId") FROM stdin;
2d72489e-9cba-47a3-8a50-e95972526e76	string	string	2023-12-11 04:54:25.004+05	3	0	52c201b7-0afe-4766-bbf1-d54fae3044c4
32fbedc8-6cf6-423a-b45c-03cbab4a8eba	Название задачи	Описание задачи	2023-12-14 04:58:00+05	2	0	52c201b7-0afe-4766-bbf1-d54fae3044c4
08bdd58e-7ea6-462c-a2a6-c9aa23827a16	Название задачи 2	Описание задачи 2	2023-12-23 02:53:00+05	1	0	52c201b7-0afe-4766-bbf1-d54fae3044c4
4027c25b-ffbc-4a75-842a-901f212f67e0	kal2	stringovna	2023-12-09 19:52:46.124+05	2	1	52c201b7-0afe-4766-bbf1-d54fae3044c4
\.


--
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."Users" ("Id", "Username", "PasswordHash", "RefreshToken", "TokenCreated", "TokenExpires") FROM stdin;
52c201b7-0afe-4766-bbf1-d54fae3044c4	string	$2a$11$AjJuL6.diMJ8SMZICYC2Lu9na5I6oZoDxhd5cRKrD4WbuhvP348bK	eHHlVsON1ogAn7dh1eslwm/vyNs+XML9Na/AmGuVP8lt7tqv2taJVIamB0F2KTJQhM/+p+3A68OIUWzA1fCa/g==	2023-12-12 08:26:48.309573+05	2023-12-19 08:26:48.309632+05
2ab66e3e-9088-4b1a-b903-0ad30db96440	string1	$2a$11$Jv7Onl5OvLnaREL6cOIaceGaM2ba3a0yZ.qvGqv1IRpxySmTO4bwG		-infinity	-infinity
0c00cf1a-f66b-4267-a12d-ea265599c30d	string2	$2a$11$8I4PkHa4xwaEBPn6lljGmus50sJVvUqUSxH/sMRIYmZKvEKvdpLQm		-infinity	-infinity
\.


--
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") FROM stdin;
20231209193700_initial	8.0.0
\.


--
-- Name: Tasks PK_Tasks; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tasks"
    ADD CONSTRAINT "PK_Tasks" PRIMARY KEY ("Id");


--
-- Name: Users PK_Users; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Users"
    ADD CONSTRAINT "PK_Users" PRIMARY KEY ("Id");


--
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


--
-- Name: IX_Tasks_UserId; Type: INDEX; Schema: public; Owner: postgres
--

CREATE INDEX "IX_Tasks_UserId" ON public."Tasks" USING btree ("UserId");


--
-- Name: Tasks FK_Tasks_Users_UserId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Tasks"
    ADD CONSTRAINT "FK_Tasks_Users_UserId" FOREIGN KEY ("UserId") REFERENCES public."Users"("Id") ON DELETE CASCADE;


--
-- PostgreSQL database dump complete
--

