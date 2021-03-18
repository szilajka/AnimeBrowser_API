--
-- PostgreSQL database dump
--

-- Dumped from database version 13.2
-- Dumped by pg_dump version 13.2

-- Started on 2021-03-18 10:45:03

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
-- TOC entry 211 (class 1259 OID 26805)
-- Name: anime_info; Type: TABLE; Schema: public; Owner: ab_user
--

CREATE TABLE public.anime_info (
    id bigint NOT NULL,
    title character varying(255) NOT NULL,
    description character varying,
    is_nsfw boolean NOT NULL
);


ALTER TABLE public.anime_info OWNER TO ab_user;

--
-- TOC entry 223 (class 1259 OID 26973)
-- Name: anime_info_id_seq; Type: SEQUENCE; Schema: public; Owner: ab_user
--

ALTER TABLE public.anime_info ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.anime_info_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 222 (class 1259 OID 26963)
-- Name: anime_info_name; Type: TABLE; Schema: public; Owner: ab_user
--

CREATE TABLE public.anime_info_name (
    id bigint NOT NULL,
    title character varying(255) NOT NULL,
    anime_info_id bigint NOT NULL
);


ALTER TABLE public.anime_info_name OWNER TO ab_user;

--
-- TOC entry 224 (class 1259 OID 26975)
-- Name: anime_info_name_id_seq; Type: SEQUENCE; Schema: public; Owner: ab_user
--

ALTER TABLE public.anime_info_name ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.anime_info_name_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 213 (class 1259 OID 26826)
-- Name: episode; Type: TABLE; Schema: public; Owner: ab_user
--

CREATE TABLE public.episode (
    id bigint NOT NULL,
    episode_number integer NOT NULL,
    air_status integer NOT NULL,
    anime_info_id bigint NOT NULL,
    title character varying(255),
    rating numeric(3,2),
    description character varying,
    cover bytea,
    air_date date,
    season_id bigint NOT NULL
);


ALTER TABLE public.episode OWNER TO ab_user;

--
-- TOC entry 225 (class 1259 OID 26977)
-- Name: episode_id_seq; Type: SEQUENCE; Schema: public; Owner: ab_user
--

ALTER TABLE public.episode ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.episode_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 218 (class 1259 OID 26895)
-- Name: episode_media_list; Type: TABLE; Schema: public; Owner: ab_user
--

CREATE TABLE public.episode_media_list (
    id bigint NOT NULL,
    list_id bigint NOT NULL,
    episode_id bigint NOT NULL
);


ALTER TABLE public.episode_media_list OWNER TO ab_user;

--
-- TOC entry 226 (class 1259 OID 26979)
-- Name: episode_media_list_id_seq; Type: SEQUENCE; Schema: public; Owner: ab_user
--

ALTER TABLE public.episode_media_list ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.episode_media_list_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 219 (class 1259 OID 26910)
-- Name: episode_rating; Type: TABLE; Schema: public; Owner: ab_user
--

CREATE TABLE public.episode_rating (
    id bigint NOT NULL,
    rating integer NOT NULL,
    message character varying(30000),
    episode_id bigint NOT NULL,
    user_id text NOT NULL
);


ALTER TABLE public.episode_rating OWNER TO ab_user;

--
-- TOC entry 227 (class 1259 OID 26981)
-- Name: episode_rating_id_seq; Type: SEQUENCE; Schema: public; Owner: ab_user
--

ALTER TABLE public.episode_rating ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.episode_rating_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 214 (class 1259 OID 26844)
-- Name: genre; Type: TABLE; Schema: public; Owner: ab_user
--

CREATE TABLE public.genre (
    id bigint NOT NULL,
    genre_name character varying(100) NOT NULL,
    description character varying NOT NULL
);


ALTER TABLE public.genre OWNER TO ab_user;

--
-- TOC entry 228 (class 1259 OID 26983)
-- Name: genre_id_seq; Type: SEQUENCE; Schema: public; Owner: ab_user
--

ALTER TABLE public.genre ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.genre_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 216 (class 1259 OID 26867)
-- Name: media_list; Type: TABLE; Schema: public; Owner: ab_user
--

CREATE TABLE public.media_list (
    id bigint NOT NULL,
    name character varying(500),
    list_type integer NOT NULL,
    is_public boolean NOT NULL,
    user_id text NOT NULL
);


ALTER TABLE public.media_list OWNER TO ab_user;

--
-- TOC entry 229 (class 1259 OID 26985)
-- Name: media_list_id_seq; Type: SEQUENCE; Schema: public; Owner: ab_user
--

ALTER TABLE public.media_list ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.media_list_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 212 (class 1259 OID 26813)
-- Name: season; Type: TABLE; Schema: public; Owner: ab_user
--

CREATE TABLE public.season (
    id bigint NOT NULL,
    season_number integer NOT NULL,
    rating numeric(3,2),
    description character varying,
    start_date date,
    end_date date,
    air_status integer NOT NULL,
    number_of_episodes integer,
    cover_carousel bytea,
    cover bytea,
    title character varying(255) NOT NULL,
    anime_info_id bigint NOT NULL
);


ALTER TABLE public.season OWNER TO ab_user;

--
-- TOC entry 215 (class 1259 OID 26852)
-- Name: season_genre; Type: TABLE; Schema: public; Owner: ab_user
--

CREATE TABLE public.season_genre (
    id bigint NOT NULL,
    genre_id bigint NOT NULL,
    season_id bigint NOT NULL
);


ALTER TABLE public.season_genre OWNER TO ab_user;

--
-- TOC entry 231 (class 1259 OID 26989)
-- Name: season_genre_id_seq; Type: SEQUENCE; Schema: public; Owner: ab_user
--

ALTER TABLE public.season_genre ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.season_genre_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 230 (class 1259 OID 26987)
-- Name: season_id_seq; Type: SEQUENCE; Schema: public; Owner: ab_user
--

ALTER TABLE public.season ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.season_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 217 (class 1259 OID 26880)
-- Name: season_media_list; Type: TABLE; Schema: public; Owner: ab_user
--

CREATE TABLE public.season_media_list (
    id bigint NOT NULL,
    list_id bigint NOT NULL,
    season_id bigint NOT NULL
);


ALTER TABLE public.season_media_list OWNER TO ab_user;

--
-- TOC entry 232 (class 1259 OID 26991)
-- Name: season_media_list_id_seq; Type: SEQUENCE; Schema: public; Owner: ab_user
--

ALTER TABLE public.season_media_list ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.season_media_list_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 221 (class 1259 OID 26946)
-- Name: season_name; Type: TABLE; Schema: public; Owner: ab_user
--

CREATE TABLE public.season_name (
    id bigint NOT NULL,
    title character varying(255) NOT NULL,
    season_id bigint NOT NULL
);


ALTER TABLE public.season_name OWNER TO ab_user;

--
-- TOC entry 233 (class 1259 OID 26993)
-- Name: season_name_id_seq; Type: SEQUENCE; Schema: public; Owner: ab_user
--

ALTER TABLE public.season_name ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.season_name_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 220 (class 1259 OID 26928)
-- Name: season_rating; Type: TABLE; Schema: public; Owner: ab_user
--

CREATE TABLE public.season_rating (
    id bigint NOT NULL,
    rating integer NOT NULL,
    message character varying(30000),
    season_id bigint NOT NULL,
    user_id text NOT NULL
);


ALTER TABLE public.season_rating OWNER TO ab_user;

--
-- TOC entry 234 (class 1259 OID 26995)
-- Name: season_rating_id_seq; Type: SEQUENCE; Schema: public; Owner: ab_user
--

ALTER TABLE public.season_rating ALTER COLUMN id ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME public.season_rating_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);


--
-- TOC entry 3015 (class 2606 OID 26967)
-- Name: anime_info_name anime_info_name_pkey; Type: CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.anime_info_name
    ADD CONSTRAINT anime_info_name_pkey PRIMARY KEY (id);


--
-- TOC entry 2993 (class 2606 OID 26812)
-- Name: anime_info anime_info_pkey; Type: CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.anime_info
    ADD CONSTRAINT anime_info_pkey PRIMARY KEY (id);


--
-- TOC entry 3007 (class 2606 OID 26899)
-- Name: episode_media_list episode_media_list_pkey; Type: CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.episode_media_list
    ADD CONSTRAINT episode_media_list_pkey PRIMARY KEY (id);


--
-- TOC entry 2997 (class 2606 OID 26833)
-- Name: episode episode_pkey; Type: CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.episode
    ADD CONSTRAINT episode_pkey PRIMARY KEY (id);


--
-- TOC entry 3009 (class 2606 OID 26917)
-- Name: episode_rating episode_rating_pkey; Type: CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.episode_rating
    ADD CONSTRAINT episode_rating_pkey PRIMARY KEY (id);


--
-- TOC entry 2999 (class 2606 OID 26851)
-- Name: genre genre_pkey; Type: CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.genre
    ADD CONSTRAINT genre_pkey PRIMARY KEY (id);


--
-- TOC entry 3003 (class 2606 OID 26874)
-- Name: media_list media_list_pkey; Type: CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.media_list
    ADD CONSTRAINT media_list_pkey PRIMARY KEY (id);


--
-- TOC entry 3001 (class 2606 OID 26856)
-- Name: season_genre season_genre_pkey; Type: CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season_genre
    ADD CONSTRAINT season_genre_pkey PRIMARY KEY (id);


--
-- TOC entry 3005 (class 2606 OID 26884)
-- Name: season_media_list season_media_list_pkey; Type: CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season_media_list
    ADD CONSTRAINT season_media_list_pkey PRIMARY KEY (id);


--
-- TOC entry 3013 (class 2606 OID 26953)
-- Name: season_name season_name_pkey; Type: CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season_name
    ADD CONSTRAINT season_name_pkey PRIMARY KEY (id);


--
-- TOC entry 2995 (class 2606 OID 26820)
-- Name: season season_pkey; Type: CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season
    ADD CONSTRAINT season_pkey PRIMARY KEY (id);


--
-- TOC entry 3011 (class 2606 OID 26935)
-- Name: season_rating season_rating_pkey; Type: CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season_rating
    ADD CONSTRAINT season_rating_pkey PRIMARY KEY (id);

--
-- TOC entry 3037 (class 2606 OID 26968)
-- Name: anime_info_name fk_anime_info_name_anime_info_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.anime_info_name
    ADD CONSTRAINT fk_anime_info_name_anime_info_id FOREIGN KEY (anime_info_id) REFERENCES public.anime_info(id) NOT VALID;


--
-- TOC entry 3023 (class 2606 OID 26834)
-- Name: episode fk_episode_anime_info_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.episode
    ADD CONSTRAINT fk_episode_anime_info_id FOREIGN KEY (anime_info_id) REFERENCES public.anime_info(id) NOT VALID;


--
-- TOC entry 3031 (class 2606 OID 26905)
-- Name: episode_media_list fk_episode_media_list_episode_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.episode_media_list
    ADD CONSTRAINT fk_episode_media_list_episode_id FOREIGN KEY (episode_id) REFERENCES public.episode(id) NOT VALID;


--
-- TOC entry 3030 (class 2606 OID 26900)
-- Name: episode_media_list fk_episode_media_list_list_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.episode_media_list
    ADD CONSTRAINT fk_episode_media_list_list_id FOREIGN KEY (list_id) REFERENCES public.media_list(id) NOT VALID;


--
-- TOC entry 3032 (class 2606 OID 26918)
-- Name: episode_rating fk_episode_rating_episode_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.episode_rating
    ADD CONSTRAINT fk_episode_rating_episode_id FOREIGN KEY (episode_id) REFERENCES public.episode(id) NOT VALID;


--
-- TOC entry 3033 (class 2606 OID 26923)
-- Name: episode_rating fk_episode_rating_user_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.episode_rating
    ADD CONSTRAINT fk_episode_rating_user_id FOREIGN KEY (user_id) REFERENCES identity."Users"("Id") NOT VALID;


--
-- TOC entry 3024 (class 2606 OID 26839)
-- Name: episode fk_episode_season_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.episode
    ADD CONSTRAINT fk_episode_season_id FOREIGN KEY (season_id) REFERENCES public.season(id) NOT VALID;


--
-- TOC entry 3027 (class 2606 OID 26875)
-- Name: media_list fk_media_list_user_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.media_list
    ADD CONSTRAINT fk_media_list_user_id FOREIGN KEY (user_id) REFERENCES identity."Users"("Id") NOT VALID;


--
-- TOC entry 3022 (class 2606 OID 26821)
-- Name: season fk_season_anime_info_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season
    ADD CONSTRAINT fk_season_anime_info_id FOREIGN KEY (anime_info_id) REFERENCES public.anime_info(id) NOT VALID;


--
-- TOC entry 3025 (class 2606 OID 26857)
-- Name: season_genre fk_season_genre_genre_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season_genre
    ADD CONSTRAINT fk_season_genre_genre_id FOREIGN KEY (genre_id) REFERENCES public.genre(id) NOT VALID;


--
-- TOC entry 3026 (class 2606 OID 26862)
-- Name: season_genre fk_season_genre_season_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season_genre
    ADD CONSTRAINT fk_season_genre_season_id FOREIGN KEY (season_id) REFERENCES public.season(id) NOT VALID;


--
-- TOC entry 3028 (class 2606 OID 26885)
-- Name: season_media_list fk_season_media_list_list_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season_media_list
    ADD CONSTRAINT fk_season_media_list_list_id FOREIGN KEY (list_id) REFERENCES public.media_list(id) NOT VALID;


--
-- TOC entry 3029 (class 2606 OID 26890)
-- Name: season_media_list fk_season_media_list_season_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season_media_list
    ADD CONSTRAINT fk_season_media_list_season_id FOREIGN KEY (season_id) REFERENCES public.season(id) NOT VALID;


--
-- TOC entry 3036 (class 2606 OID 26958)
-- Name: season_name fk_season_name_season_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season_name
    ADD CONSTRAINT fk_season_name_season_id FOREIGN KEY (season_id) REFERENCES public.season(id) NOT VALID;


--
-- TOC entry 3034 (class 2606 OID 26936)
-- Name: season_rating fk_season_rating_season_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season_rating
    ADD CONSTRAINT fk_season_rating_season_id FOREIGN KEY (season_id) REFERENCES public.season(id) NOT VALID;


--
-- TOC entry 3035 (class 2606 OID 26941)
-- Name: season_rating fk_season_rating_user_id; Type: FK CONSTRAINT; Schema: public; Owner: ab_user
--

ALTER TABLE ONLY public.season_rating
    ADD CONSTRAINT fk_season_rating_user_id FOREIGN KEY (user_id) REFERENCES identity."Users"("Id") NOT VALID;



--
-- PostgreSQL database dump complete
--

