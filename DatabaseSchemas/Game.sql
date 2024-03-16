PGDMP         %                |            GD_GAME    15.3    15.1 #    "           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            #           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            $           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            %           1262    54147    GD_GAME    DATABASE     }   CREATE DATABASE "GD_GAME" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'Russian_Russia.1251';
    DROP DATABASE "GD_GAME";
                Fljk236yhue    false                        2615    2200    public    SCHEMA     2   -- *not* creating schema, since initdb creates it
 2   -- *not* dropping schema, since initdb creates it
                postgres    false            &           0    0    SCHEMA public    ACL     Q   REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;
                   postgres    false    5            �            1255    54148 ,   create_character(integer, character varying) 	   PROCEDURE     �   CREATE PROCEDURE public.create_character(IN p_login_id integer, IN p_character_name character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
INSERT INTO "characters"(login_id, character_name) VALUES (p_login_id, p_character_name);
COMMIT;
END;
$$;
 f   DROP PROCEDURE public.create_character(IN p_login_id integer, IN p_character_name character varying);
       public          postgres    false    5            �            1255    70533    destroy_item(integer, bigint)    FUNCTION     �  CREATE FUNCTION public.destroy_item(p_owner_id integer, p_item_uid bigint) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_success BOOLEAN;
BEGIN
    BEGIN
        DELETE FROM items
        WHERE owner_id = p_owner_id AND item_uid = p_item_uid;
        
        IF FOUND THEN
            v_success := TRUE;
        ELSE
            v_success := FALSE;
        END IF;
    EXCEPTION WHEN OTHERS THEN
        v_success := FALSE;
    END;
    RETURN v_success;
END;
$$;
 J   DROP FUNCTION public.destroy_item(p_owner_id integer, p_item_uid bigint);
       public          Fljk236yhue    false    5            �            1255    54149    get_characters(integer)    FUNCTION     V  CREATE FUNCTION public.get_characters(p_login_id integer) RETURNS TABLE(character_datas jsonb)
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
       public          postgres    false    5            �            1255    54150 $   get_chatacer_data(character varying)    FUNCTION     T  CREATE FUNCTION public.get_chatacer_data(p_character_name character varying) RETURNS TABLE(json_obj jsonb)
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
       public          postgres    false    5            �            1255    54151    get_chatacer_position(integer)    FUNCTION     H  CREATE FUNCTION public.get_chatacer_position(p_character_id integer) RETURNS TABLE(json_obj jsonb)
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
       public          postgres    false    5            �            1255    54152    get_chatacer_stat(integer)    FUNCTION     $  CREATE FUNCTION public.get_chatacer_stat(p_character_id integer) RETURNS TABLE(json_obj jsonb)
    LANGUAGE plpgsql
    AS $$
   BEGIN
      return query
      SELECT jsonb_build_object(
		  'name', character_name) json_obj FROM "characters" WHERE character_id = p_character_id;
    END;
$$;
 @   DROP FUNCTION public.get_chatacer_stat(p_character_id integer);
       public          postgres    false    5            �            1255    54153    get_hotbar(integer)    FUNCTION     c  CREATE FUNCTION public.get_hotbar(p_character_id integer) RETURNS TABLE(json_obj jsonb)
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
       public          postgres    false    5            �            1255    63895    get_unique_item_id()    FUNCTION     k  CREATE FUNCTION public.get_unique_item_id() RETURNS bigint
    LANGUAGE plpgsql
    AS $$
DECLARE
    last_id BIGINT;
   begin
     SELECT MAX(item_uid) INTO last_id FROM items;
 IF last_id IS NULL THEN
     -- If there are no items, start from 1
     RETURN 1;
 ELSE
     -- Otherwise, return the next available ID
     RETURN last_id + 1;
 END IF;
    end;
$$;
 +   DROP FUNCTION public.get_unique_item_id();
       public          Fljk236yhue    false    5            �            1255    54154 %   is_character_belong(integer, integer)    FUNCTION     �   CREATE FUNCTION public.is_character_belong(p_login_id integer, p_character_id integer) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
   begin
    RETURN ((SELECT login_id FROM "characters" WHERE character_id = p_character_id) = p_login_id);
    end;
$$;
 V   DROP FUNCTION public.is_character_belong(p_login_id integer, p_character_id integer);
       public          postgres    false    5            �            1255    63896    load_inventory(integer)    FUNCTION     e  CREATE FUNCTION public.load_inventory(p_character_id integer) RETURNS TABLE(json_obj jsonb)
    LANGUAGE plpgsql
    AS $$
   begin
     return query
      SELECT jsonb_agg(jsonb_build_object(
	      'bagIndex', bag_index,
	      'slotIndex', cell_index,
	      'uniqueId', item_uid)) json_obj FROM "inventory" WHERE owner_id = p_character_id;
    end;
$$;
 =   DROP FUNCTION public.load_inventory(p_character_id integer);
       public          Fljk236yhue    false    5            �            1255    70531    load_items(integer)    FUNCTION     L  CREATE FUNCTION public.load_items(p_character_id integer) RETURNS TABLE(json_obj jsonb)
    LANGUAGE plpgsql
    AS $$
   begin
     return query
      SELECT jsonb_agg(jsonb_build_object(
	      'uid', item_uid,
	      'id', item_id,
	      'count', item_count)) json_obj FROM "items" WHERE owner_id = p_character_id;
    end;
$$;
 9   DROP FUNCTION public.load_items(p_character_id integer);
       public          Fljk236yhue    false    5            �            1255    70534 +   remove_inventory(integer, integer, integer)    FUNCTION       CREATE FUNCTION public.remove_inventory(p_owner_id integer, p_bag_index integer, p_cell_index integer) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    DELETE FROM inventory
    WHERE owner_id = p_owner_id AND bag_index = p_bag_index AND cell_index = p_cell_index;
END;
$$;
 f   DROP FUNCTION public.remove_inventory(p_owner_id integer, p_bag_index integer, p_cell_index integer);
       public          Fljk236yhue    false    5            �            1255    54155 1   set_character_position(integer, real, real, real) 	   PROCEDURE     �  CREATE PROCEDURE public.set_character_position(IN p_character_id integer, IN p_pos_x real, IN p_pos_y real, IN p_pos_z real)
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
       public          postgres    false    5            �            1255    54156    set_hotbar() 	   PROCEDURE     -  CREATE PROCEDURE public.set_hotbar()
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
       public          postgres    false    5            �            1255    54157 .   set_hotbar(integer, integer, integer, integer) 	   PROCEDURE     �  CREATE PROCEDURE public.set_hotbar(IN p_character_id integer, IN p_cell_index integer, IN p_cell_type integer, IN p_cell_value integer)
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
       public          postgres    false    5            �            1255    70536 3   upsert_inventory(integer, integer, integer, bigint)    FUNCTION     �  CREATE FUNCTION public.upsert_inventory(p_owner_id integer, p_bag_index integer, p_cell_index integer, p_item_uid bigint) RETURNS void
    LANGUAGE plpgsql
    AS $$
BEGIN
    INSERT INTO inventory(owner_id, bag_index, cell_index, item_uid)
    VALUES (p_owner_id, p_bag_index, p_cell_index, p_item_uid)
    ON CONFLICT (owner_id, bag_index, cell_index)
    DO UPDATE SET item_uid = p_item_uid;
END;
$$;
 y   DROP FUNCTION public.upsert_inventory(p_owner_id integer, p_bag_index integer, p_cell_index integer, p_item_uid bigint);
       public          Fljk236yhue    false    5            �            1255    70532 .   upsert_item(integer, bigint, integer, integer)    FUNCTION     t  CREATE FUNCTION public.upsert_item(p_owner_id integer, p_item_uid bigint, p_item_id integer, p_item_count integer) RETURNS boolean
    LANGUAGE plpgsql
    AS $$
DECLARE
    v_success BOOLEAN;
BEGIN
    BEGIN
        INSERT INTO items(owner_id, item_uid, item_id, item_count)
        VALUES (p_owner_id, p_item_uid, p_item_id, p_item_count)
        ON CONFLICT (item_uid)
        DO UPDATE SET item_count = p_item_count
        WHERE items.owner_id = p_owner_id AND items.item_id = p_item_id;
        
        v_success := TRUE;
    EXCEPTION WHEN OTHERS THEN
        v_success := FALSE;
    END;
    RETURN v_success;
END;
$$;
 r   DROP FUNCTION public.upsert_item(p_owner_id integer, p_item_uid bigint, p_item_id integer, p_item_count integer);
       public          Fljk236yhue    false    5            �            1259    54158    character_positions    TABLE        CREATE TABLE public.character_positions (
    character_id integer NOT NULL,
    pos_x real,
    pos_y real,
    pos_z real
);
 '   DROP TABLE public.character_positions;
       public         heap    postgres    false    5            �            1259    54161 
   characters    TABLE     �   CREATE TABLE public.characters (
    character_id integer NOT NULL,
    login_id integer,
    character_name character varying(30) NOT NULL
);
    DROP TABLE public.characters;
       public         heap    postgres    false    5            �            1259    54164    characters_character_id_seq    SEQUENCE     �   ALTER TABLE public.characters ALTER COLUMN character_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.characters_character_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    5    215            �            1259    54165    hotbar    TABLE     �   CREATE TABLE public.hotbar (
    character_id integer NOT NULL,
    cell_index integer NOT NULL,
    cell_type integer,
    cell_value integer
);
    DROP TABLE public.hotbar;
       public         heap    postgres    false    5            �            1259    63879 	   inventory    TABLE     �   CREATE TABLE public.inventory (
    owner_id integer NOT NULL,
    bag_index integer NOT NULL,
    cell_index integer NOT NULL,
    item_uid bigint
);
    DROP TABLE public.inventory;
       public         heap    Fljk236yhue    false    5            �            1259    63885    items    TABLE     �   CREATE TABLE public.items (
    item_uid bigint NOT NULL,
    item_id integer NOT NULL,
    item_count integer NOT NULL,
    owner_id integer
);
    DROP TABLE public.items;
       public         heap    Fljk236yhue    false    5            �           2606    54169    characters character_name 
   CONSTRAINT     ^   ALTER TABLE ONLY public.characters
    ADD CONSTRAINT character_name UNIQUE (character_name);
 C   ALTER TABLE ONLY public.characters DROP CONSTRAINT character_name;
       public            postgres    false    215            �           2606    54171 ,   character_positions character_positions_pkey 
   CONSTRAINT     t   ALTER TABLE ONLY public.character_positions
    ADD CONSTRAINT character_positions_pkey PRIMARY KEY (character_id);
 V   ALTER TABLE ONLY public.character_positions DROP CONSTRAINT character_positions_pkey;
       public            postgres    false    214            �           2606    54173    characters characters_pkey 
   CONSTRAINT     b   ALTER TABLE ONLY public.characters
    ADD CONSTRAINT characters_pkey PRIMARY KEY (character_id);
 D   ALTER TABLE ONLY public.characters DROP CONSTRAINT characters_pkey;
       public            postgres    false    215            �           2606    54175    hotbar hotbar_pkey 
   CONSTRAINT     f   ALTER TABLE ONLY public.hotbar
    ADD CONSTRAINT hotbar_pkey PRIMARY KEY (character_id, cell_index);
 <   ALTER TABLE ONLY public.hotbar DROP CONSTRAINT hotbar_pkey;
       public            postgres    false    217    217            �           2606    63883    inventory inventory_pkey 
   CONSTRAINT     s   ALTER TABLE ONLY public.inventory
    ADD CONSTRAINT inventory_pkey PRIMARY KEY (owner_id, bag_index, cell_index);
 B   ALTER TABLE ONLY public.inventory DROP CONSTRAINT inventory_pkey;
       public            Fljk236yhue    false    218    218    218            �           2606    63889    items items_pkey 
   CONSTRAINT     T   ALTER TABLE ONLY public.items
    ADD CONSTRAINT items_pkey PRIMARY KEY (item_uid);
 :   ALTER TABLE ONLY public.items DROP CONSTRAINT items_pkey;
       public            Fljk236yhue    false    219           