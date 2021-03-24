--
-- TOC entry 239 (class 1255 OID 42445)
-- Name: create_user_list(); Type: FUNCTION; Schema: public; Owner: ab_user
--

CREATE FUNCTION public.create_user_list() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO "public"."media_list" ("name", "list_type", "is_public", "user_id")
        VALUES ("Planned", 10, TRUE, NEW."Id");
    INSERT INTO "public"."media_list" ("name", "list_type", "is_public", "user_id")
        VALUES ("Watching", 20, TRUE, NEW."Id");
    INSERT INTO "public"."media_list" ("name", "list_type", "is_public", "user_id")
        VALUES ("Watched", 30, TRUE, NEW."Id");
    RETURN NEW;
END;
$$;


ALTER FUNCTION public.create_user_list() OWNER TO ab_user;

--
-- TOC entry 3055 (class 2620 OID 42451)
-- Name: Users tg_create_user_list; Type: TRIGGER; Schema: identity; Owner: ab_user
--

CREATE TRIGGER tg_create_user_list AFTER INSERT ON identity."Users" FOR EACH ROW EXECUTE FUNCTION public.create_user_list();
