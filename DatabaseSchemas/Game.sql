PGDMP     5    #                |            GL_Game %   14.10 (Ubuntu 14.10-0ubuntu0.22.04.1)    15.1     
           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false                       0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false                       0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false                       1262    16398    GL_Game    DATABASE     q   CREATE DATABASE "GL_Game" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'C.UTF-8';
    DROP DATABASE "GL_Game";
                postgres    false                        2615    2200    public    SCHEMA     2   -- *not* creating schema, since initdb creates it
 2   -- *not* dropping schema, since initdb creates it
                postgres    false                       0    0    SCHEMA public    ACL     Q   REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;
                   postgres    false    5            �            1255    16399 ,   create_character(integer, character varying) 	   PROCEDURE     �   CREATE PROCEDURE public.create_character(IN p_login_id integer, IN p_character_name character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
INSERT INTO "characters"(login_id, character_name) VALUES (p_login_id, p_character_name);
COMMIT;
END;
$$;
 f   DROP PROCEDURE public.create_character(IN p_login_id integer, IN p_character_name character varying);
       public          postgres    false    5            �            1255    16400    get_characters(integer)    FUNCTION     V  CREATE FUNCTION public.get_characters(p_login_id integer) RETURNS TABLE(character_datas jsonb)
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
       public          postgres    false    5            �            1255    16401 $   get_chatacer_data(character varying)    FUNCTION     T  CREATE FUNCTION public.get_chatacer_data(p_character_name character varying) RETURNS TABLE(json_obj jsonb)
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
       public          postgres    false    5            �            1255    16402    get_chatacer_position(integer)    FUNCTION     H  CREATE FUNCTION public.get_chatacer_position(p_character_id integer) RETURNS TABLE(json_obj jsonb)
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
       public          postgres    false    5            �            1255    16433    get_chatacer_stat(integer)    FUNCTION     $  CREATE FUNCTION public.get_chatacer_stat(p_character_id integer) RETURNS TABLE(json_obj jsonb)
    LANGUAGE plpgsql
    AS $$
   BEGIN
      return query
      SELECT jsonb_build_object(
		  'name', character_name) json_obj FROM "characters" WHERE character_id = p_character_id;
    END;
$$;
 @   DROP FUNCTION public.get_chatacer_stat(p_character_id integer);
       public          postgres    false    5            �            1255    16431    get_hotbar(integer)    FUNCTION     c  CREATE FUNCTION public.get_hotbar(p_character_id integer) RETURNS TABLE(json_obj jsonb)
    LANGUAGE plpgsql
    AS $$
   BEGIN
       return query
      SELECT jsonb_agg(
		  jsonb_build_object(
		    'CellIndex', cell_index,
		    'CellType', cell_type,
	        'CellValue', cell_value)) FROM "hotbar" WHERE character_id = p_character_id;
    END;
$$;
 9   DROP FUNCTION public.get_hotbar(p_character_id integer);
       public          postgres    false    5            �            1255    16403 %   is_character_belong(integer, integer)    FUNCTION     �   CREATE FUNCTION public.is_character_belong(p_login_id integer, p_character_id integer) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
   begin
    RETURN ((SELECT login_id FROM "characters" WHERE character_id = p_character_id) = p_login_id);
    end;
$$;
 V   DROP FUNCTION public.is_character_belong(p_login_id integer, p_character_id integer);
       public          postgres    false    5            �            1255    16404 1   set_character_position(integer, real, real, real) 	   PROCEDURE     �  CREATE PROCEDURE public.set_character_position(IN p_character_id integer, IN p_pos_x real, IN p_pos_y real, IN p_pos_z real)
    LANGUAGE plpgsql
    AS $$
BEGIN
INSERT INTO "character_positions"(character_id, pos_x, pos_y, pos_z) VALUES(p_character_id, p_pos_x, p_pos_y, p_pos_z)
ON CONFLICT(character_id) DO UPDATE SET pos_x = p_pos_x,
                                        pos_y = p_pos_y,
                                        pos_z = p_pos_z;
END;
$$;
 |   DROP PROCEDURE public.set_character_position(IN p_character_id integer, IN p_pos_x real, IN p_pos_y real, IN p_pos_z real);
       public          postgres    false    5            �            1255    16428    set_hotbar() 	   PROCEDURE     -  CREATE PROCEDURE public.set_hotbar()
    LANGUAGE sql
    AS $_$CREATE OR REPLACE PROCEDURE public.set_hotbar(
	IN p_character_id integer,
	IN p_cell_index integer,
	IN p_cell_type integer,
	IN p_cell_value integer)
	language plpgsql
	as $$
BEGIN
INSERT INTO "hotbar"(character_id, cell_index, cell_type, cell_value) VALUES(p_character_id, p_cell_index, p_cell_type, p_cell_value)
ON CONFLICT(character_id, cell_index) DO UPDATE SET cell_type = p_cell_type,
                                                    cell_value = p_cell_value;
 commit;
end;$$$_$;
 $   DROP PROCEDURE public.set_hotbar();
       public          postgres    false    5            �            1255    16432 .   set_hotbar(integer, integer, integer, integer) 	   PROCEDURE     �  CREATE PROCEDURE public.set_hotbar(IN p_character_id integer, IN p_cell_index integer, IN p_cell_type integer, IN p_cell_value integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
INSERT INTO "hotbar"(character_id, cell_index, cell_type, cell_value) VALUES(p_character_id, p_cell_index, p_cell_type, p_cell_value)
ON CONFLICT(character_id, cell_index) DO UPDATE SET cell_type = p_cell_type,
                                                    cell_value = p_cell_value;
 commit;
end;
$$;
 �   DROP PROCEDURE public.set_hotbar(IN p_character_id integer, IN p_cell_index integer, IN p_cell_type integer, IN p_cell_value integer);
       public          postgres    false    5            �            1259    16405    character_positions    TABLE        CREATE TABLE public.character_positions (
    character_id integer NOT NULL,
    pos_x real,
    pos_y real,
    pos_z real
);
 '   DROP TABLE public.character_positions;
       public         heap    postgres    false    5            �            1259    16408 
   characters    TABLE     �   CREATE TABLE public.characters (
    character_id integer NOT NULL,
    login_id integer,
    character_name character varying(30) NOT NULL
);
    DROP TABLE public.characters;
       public         heap    postgres    false    5            �            1259    16411    characters_character_id_seq    SEQUENCE     �   ALTER TABLE public.characters ALTER COLUMN character_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.characters_character_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    5    210            �            1259    16418    hotbar    TABLE     �   CREATE TABLE public.hotbar (
    character_id integer NOT NULL,
    cell_index integer NOT NULL,
    cell_type integer,
    cell_value integer
);
    DROP TABLE public.hotbar;
       public         heap    postgres    false    5            x           2606    16413    characters character_name 
   CONSTRAINT     ^   ALTER TABLE ONLY public.characters
    ADD CONSTRAINT character_name UNIQUE (character_name);
 C   ALTER TABLE ONLY public.characters DROP CONSTRAINT character_name;
       public            postgres    false    210            v           2606    16415 ,   character_positions character_positions_pkey 
   CONSTRAINT     t   ALTER TABLE ONLY public.character_positions
    ADD CONSTRAINT character_positions_pkey PRIMARY KEY (character_id);
 V   ALTER TABLE ONLY public.character_positions DROP CONSTRAINT character_positions_pkey;
       public            postgres    false    209            z           2606    16417    characters characters_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public.characters
    ADD CONSTRAINT characters_pkey PRIMARY KEY (character_id);
 D   ALTER TABLE ONLY public.characters DROP CONSTRAINT characters_pkey;
       public            postgres    false    210            |           2606    16422    hotbar hotbar_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.hotbar
    ADD CONSTRAINT hotbar_pkey PRIMARY KEY (character_id, cell_index);
 <   ALTER TABLE ONLY public.hotbar DROP CONSTRAINT hotbar_pkey;
       public            postgres    false    212    212           