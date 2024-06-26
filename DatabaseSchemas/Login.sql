PGDMP         $                |            GL_Login %   14.10 (Ubuntu 14.10-0ubuntu0.22.04.1)    15.1     �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            �           1262    16385    GL_Login    DATABASE     r   CREATE DATABASE "GL_Login" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'C.UTF-8';
    DROP DATABASE "GL_Login";
                postgres    false                        2615    2200    public    SCHEMA     2   -- *not* creating schema, since initdb creates it
 2   -- *not* dropping schema, since initdb creates it
                postgres    false            �           0    0    SCHEMA public    ACL     Q   REVOKE USAGE ON SCHEMA public FROM PUBLIC;
GRANT ALL ON SCHEMA public TO PUBLIC;
                   postgres    false    5            �            1255    16386 4   create_account(character varying, character varying) 	   PROCEDURE     �   CREATE PROCEDURE public.create_account(IN p_login_name character varying, IN p_login_password character varying)
    LANGUAGE plpgsql
    AS $$
BEGIN
INSERT INTO accounts(login_name, login_password) VALUES (p_login_name, p_login_password);
END;
$$;
 p   DROP PROCEDURE public.create_account(IN p_login_name character varying, IN p_login_password character varying);
       public          postgres    false    5            �            1255    16387    get_account(character varying)    FUNCTION     B  CREATE FUNCTION public.get_account(p_login_name character varying) RETURNS TABLE(json_obj jsonb)
    LANGUAGE plpgsql
    AS $$
   BEGIN
      return query
      SELECT jsonb_build_object(
		  'login_id', login_id,
		  'login_password', login_password) json_obj FROM accounts WHERE login_name = p_login_name;
    END;
$$;
 B   DROP FUNCTION public.get_account(p_login_name character varying);
       public          postgres    false    5            �            1255    16388    get_session_token(integer)    FUNCTION     �   CREATE FUNCTION public.get_session_token(p_login_id integer) RETURNS integer
    LANGUAGE plpgsql
    AS $$
   begin
    RETURN(SELECT session_token FROM "accounts" WHERE login_id = p_login_id);
    end;
$$;
 <   DROP FUNCTION public.get_session_token(p_login_id integer);
       public          postgres    false    5            �            1255    16389 #   set_session_token(integer, integer) 	   PROCEDURE     �   CREATE PROCEDURE public.set_session_token(IN p_login_id integer, IN p_session_token integer)
    LANGUAGE plpgsql
    AS $$
BEGIN
UPDATE accounts SET session_token = p_session_token WHERE login_id = p_login_id;
END;
$$;
 \   DROP PROCEDURE public.set_session_token(IN p_login_id integer, IN p_session_token integer);
       public          postgres    false    5            �            1259    16390    accounts    TABLE     �   CREATE TABLE public.accounts (
    login_id integer NOT NULL,
    login_name character varying(30) NOT NULL,
    login_password character varying(50) NOT NULL,
    session_token integer
);
    DROP TABLE public.accounts;
       public         heap    postgres    false    5            �            1259    16393    accounts_login_id_seq    SEQUENCE     �   ALTER TABLE public.accounts ALTER COLUMN login_id ADD GENERATED ALWAYS AS IDENTITY (
    SEQUENCE NAME public.accounts_login_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);
            public          postgres    false    209    5            h           2606    16395    accounts accounts_pkey 
   CONSTRAINT     Z   ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT accounts_pkey PRIMARY KEY (login_id);
 @   ALTER TABLE ONLY public.accounts DROP CONSTRAINT accounts_pkey;
       public            postgres    false    209            j           2606    16397    accounts login_name 
   CONSTRAINT     T   ALTER TABLE ONLY public.accounts
    ADD CONSTRAINT login_name UNIQUE (login_name);
 =   ALTER TABLE ONLY public.accounts DROP CONSTRAINT login_name;
       public            postgres    false    209           