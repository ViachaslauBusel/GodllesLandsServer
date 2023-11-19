PGDMP         ,            
    {            GL_Game %   12.10 (Ubuntu 12.10-0ubuntu0.20.04.1)    15.1     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    16562    GL_Game    DATABASE     u   CREATE DATABASE "GL_Game" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'en_US.UTF-8';
    DROP DATABASE "GL_Game";
                postgres    false                        2615    2200    public    SCHEMA     2   -- *not* creating schema, since initdb creates it
 2   -- *not* dropping schema, since initdb creates it
                postgres    false            �           0    0    SCHEMA public    ACL     Q   REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;
                   postgres    false    6            �            1255    16592 ,   create_character(integer, character varying) 	   PROCEDURE     �   CREATE PROCEDURE public.create_character(p_login_id integer, p_character_name character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
INSERT INTO "characters"(login_id, character_name) VALUES (p_login_id, p_character_name);
COMMIT;
END;
$$;
 `   DROP PROCEDURE public.create_character(p_login_id integer, p_character_name character varying);
       public          postgres    false    6            �            1255    16591    get_characters(integer)    FUNCTION     V  CREATE FUNCTION public.get_characters(p_login_id integer) RETURNS TABLE(character_datas jsonb)
    LANGUAGE plpgsql
    AS $$
   begin
    return query
      select jsonb_agg(
		  jsonb_build_object('character_id', character_id,
							 'character_name', character_name )
		  )
		  from "characters" where login_id = p_login_id;
    end;
$$;
 9   DROP FUNCTION public.get_characters(p_login_id integer);
       public          postgres    false    6            �            1255    16595 $   get_chatacer_data(character varying)    FUNCTION     T  CREATE FUNCTION public.get_chatacer_data(p_character_name character varying) RETURNS TABLE(json_obj jsonb)
    LANGUAGE plpgsql
    AS $$
   BEGIN
      return query
      SELECT jsonb_build_object(
		  'login_id', login_id,
		  'character_id', character_id) json_obj FROM "characters" WHERE character_name = p_character_name;
    END;
$$;
 L   DROP FUNCTION public.get_chatacer_data(p_character_name character varying);
       public          postgres    false    6            �            1255    16603    get_chatacer_position(integer)    FUNCTION     H  CREATE FUNCTION public.get_chatacer_position(p_character_id integer) RETURNS TABLE(json_obj jsonb)
    LANGUAGE plpgsql
    AS $$
   BEGIN
      return query
      SELECT jsonb_build_object(
		  'X', pos_x,
		  'Y', pos_y,
	      'Z', pos_Z) json_obj FROM "character_positions" WHERE character_id = p_character_id;
    END;
$$;
 D   DROP FUNCTION public.get_chatacer_position(p_character_id integer);
       public          postgres    false    6            �            1255    16602 %   is_character_belong(integer, integer)    FUNCTION     �   CREATE FUNCTION public.is_character_belong(p_login_id integer, p_character_id integer) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
   begin
    RETURN ((SELECT login_id FROM "characters" WHERE character_id = p_character_id) = p_login_id);
    end;
$$;
 V   DROP FUNCTION public.is_character_belong(p_login_id integer, p_character_id integer);
       public          postgres    false    6            �            1255    16601 1   set_character_position(integer, real, real, real) 	   PROCEDURE     �  CREATE PROCEDURE public.set_character_position(p_character_id integer, p_pos_x real, p_pos_y real, p_pos_z real)
    LANGUAGE plpgsql
    AS $$
BEGIN
INSERT INTO "character_positions"(character_id, pos_x, pos_y, pos_z) VALUES(p_character_id, p_pos_x, p_pos_y, p_pos_z)
ON CONFLICT(character_id) DO UPDATE SET pos_x = p_pos_x,
                                        pos_y = p_pos_y,
                                        pos_z = p_pos_z;
END;
$$;
 p   DROP PROCEDURE public.set_character_position(p_character_id integer, p_pos_x real, p_pos_y real, p_pos_z real);
       public          postgres    false    6            �            1259    16596    character_positions    TABLE        CREATE TABLE public.character_positions (
    character_id integer NOT NULL,
    pos_x real,
    pos_y real,
    pos_z real
);
 '   DROP TABLE public.character_positions;
       public         heap    postgres    false    6            �            1259    16586 
   characters    TABLE     �   CREATE TABLE public.characters (
    character_id integer NOT NULL,
    login_id integer,
    character_name character varying(30) NOT NULL
);
    DROP TABLE public.characters;
       public         heap    postgres    false    6            �            1259    16584    characters_character_id_seq    SEQUENCE     �   ALTER TABLE public.characters ALTER COLUMN character_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.characters_character_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    203    6                       2606    16594    characters character_name 
   CONSTRAINT     ^   ALTER TABLE ONLY public.characters
    ADD CONSTRAINT character_name UNIQUE (character_name);
 C   ALTER TABLE ONLY public.characters DROP CONSTRAINT character_name;
       public            postgres    false    203                       2606    16600 ,   character_positions character_positions_pkey 
   CONSTRAINT     t   ALTER TABLE ONLY public.character_positions
    ADD CONSTRAINT character_positions_pkey PRIMARY KEY (character_id);
 V   ALTER TABLE ONLY public.character_positions DROP CONSTRAINT character_positions_pkey;
       public            postgres    false    204                       2606    16590    characters characters_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public.characters
    ADD CONSTRAINT characters_pkey PRIMARY KEY (character_id);
 D   ALTER TABLE ONLY public.characters DROP CONSTRAINT characters_pkey;
       public            postgres    false    203           