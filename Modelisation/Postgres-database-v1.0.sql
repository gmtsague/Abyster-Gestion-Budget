/*==============================================================*/
/* Nom de SGBD :  PostgreSQL 8                                  */
/* Date de création :  14/02/2023 20:39:56                      */
/*==============================================================*/

BEGIN;

SET SESSION AUTHORIZATION abyster;

SET SEARCH_PATH TO budget, public;

drop index IF EXISTS association_5_fk;

drop index IF EXISTS association_3_fk;

drop index IF EXISTS autorisation_pk;

drop table IF EXISTS autorisation;

drop index IF EXISTS unique_categorie;

drop index IF EXISTS categorie_pk;

drop table IF EXISTS categorie;

drop index IF EXISTS unique_commande;

drop index IF EXISTS commande_pk;

drop table IF EXISTS commande;

drop index IF EXISTS unique_groupe;

drop index IF EXISTS groupe_pk;

drop table IF EXISTS groupe;

drop index IF EXISTS unique_operation;

drop index IF EXISTS association_2_fk;

drop index IF EXISTS association_1_fk;

drop index IF EXISTS operation_pk;

drop table IF EXISTS operation;

drop index IF EXISTS unique_users;

drop index IF EXISTS association_4_fk;

drop index IF EXISTS user_pk;

drop table IF EXISTS users;

/*==============================================================*/
/* Table : autorisation                                         */
/*==============================================================*/
create table autorisation (
   idautorisation       serial               not null,
   idcmde               int4                 not null,
   groupeid             int4                 not null,
   isactive             bool                 not null default false,
   constraint pk_autorisation primary key (idautorisation)
);

comment on table autorisation is
'Autorisation';

comment on column autorisation.idautorisation is
'Idautorisation';

comment on column autorisation.idcmde is
'Idcmde';

comment on column autorisation.groupeid is
'GroupeId';

comment on column autorisation.isactive is
'Isactive';

/*==============================================================*/
/* Index : autorisation_pk                                      */
/*==============================================================*/
create unique index autorisation_pk on autorisation (
idautorisation
);

/*==============================================================*/
/* Index : association_3_fk                                     */
/*==============================================================*/
create  index association_3_fk on autorisation (
idcmde
);

/*==============================================================*/
/* Index : association_5_fk                                     */
/*==============================================================*/
create  index association_5_fk on autorisation (
groupeid
);

/*==============================================================*/
/* Table : categorie                                            */
/*==============================================================*/
create table categorie (
   idcategorie          serial               not null,
   libelle              varchar(254)         not null,
   constraint pk_categorie primary key (idcategorie)
);

comment on table categorie is
'Categorie';

comment on column categorie.idcategorie is
'Idcategorie';

comment on column categorie.libelle is
'Libelle';

/*==============================================================*/
/* Index : categorie_pk                                         */
/*==============================================================*/
create unique index categorie_pk on categorie (
idcategorie
);

/*==============================================================*/
/* Index : unique_categorie                                     */
/*==============================================================*/
create unique index unique_categorie on categorie (
libelle
);

/*==============================================================*/
/* Table : commande                                             */
/*==============================================================*/
create table commande (
   idcmde               serial               not null,
   libelle              varchar(254)         not null,
   code                 int4                 not null,
   constraint pk_commande primary key (idcmde)
);

comment on table commande is
'Commande';

comment on column commande.idcmde is
'Idcmde';

comment on column commande.libelle is
'Libelle';

comment on column commande.code is
'Code';

/*==============================================================*/
/* Index : commande_pk                                          */
/*==============================================================*/
create unique index commande_pk on commande (
idcmde
);

/*==============================================================*/
/* Index : unique_commande                                      */
/*==============================================================*/
create unique index unique_commande on commande (
libelle
);

/*==============================================================*/
/* Table : groupe                                               */
/*==============================================================*/
create table groupe (
   groupeid             int4                 not null,
   libelle              varchar(254)         not null,
   constraint pk_groupe primary key (groupeid)
);

comment on table groupe is
'Groupe';

comment on column groupe.groupeid is
'GroupeId';

comment on column groupe.libelle is
'Libelle';

/*==============================================================*/
/* Index : groupe_pk                                            */
/*==============================================================*/
create unique index groupe_pk on groupe (
groupeid
);

/*==============================================================*/
/* Index : unique_groupe                                        */
/*==============================================================*/
create unique index unique_groupe on groupe (
libelle
);

/*==============================================================*/
/* Table : operation                                            */
/*==============================================================*/
create table operation (
   idoperation          serial               not null,
   idcategorie          int4                 not null,
   userid               int4                 not null,
   montant              numeric              not null,
   dateoperation        timestamp            not null,
   isrevenu             bool                 not null,
   constraint pk_operation primary key (idoperation)
);

comment on table operation is
'Operation';

comment on column operation.idoperation is
'Idoperation';

comment on column operation.idcategorie is
'Idcategorie';

comment on column operation.userid is
'Userid';

comment on column operation.montant is
'Montant';

comment on column operation.dateoperation is
'Dateoperation';

comment on column operation.isrevenu is
'Isrevenu';

/*==============================================================*/
/* Index : operation_pk                                         */
/*==============================================================*/
create unique index operation_pk on operation (
idoperation
);

/*==============================================================*/
/* Index : association_1_fk                                     */
/*==============================================================*/
create  index association_1_fk on operation (
userid
);

/*==============================================================*/
/* Index : association_2_fk                                     */
/*==============================================================*/
create  index association_2_fk on operation (
idcategorie
);

/*==============================================================*/
/* Index : unique_operation                                     */
/*==============================================================*/
create unique index unique_operation on operation (
idcategorie,
userid,
montant,
dateoperation,
isrevenu
);

/*==============================================================*/
/* Table : users                                                */
/*==============================================================*/
create table users (
   userid               serial               not null,
   groupeid             int4                 not null,
   nom                  varchar(254)         not null,
   prenom               varchar(254)         null,
   email                varchar(254)         not null,
   isactive             bool                 not null default false,
   password             varchar(254)         null,
   lastconnexion        timestamp            null,
   constraint pk_users primary key (userid)
);

comment on table users is
'Users';

comment on column users.userid is
'Userid';

comment on column users.groupeid is
'GroupeId';

comment on column users.nom is
'Nom';

comment on column users.prenom is
'Prenom';

comment on column users.email is
'Email';

comment on column users.isactive is
'Isactive';

comment on column users.password is
'Password';

comment on column users.lastconnexion is
'LastConnexion';

/*==============================================================*/
/* Index : user_pk                                              */
/*==============================================================*/
create unique index user_pk on users (
userid
);

/*==============================================================*/
/* Index : association_4_fk                                     */
/*==============================================================*/
create  index association_4_fk on users (
groupeid
);

/*==============================================================*/
/* Index : unique_users                                         */
/*==============================================================*/
create unique index unique_users on users (
email
);

alter table autorisation
   add constraint fk_autorisa_associati_commande foreign key (idcmde)
      references commande (idcmde)
      on delete restrict on update cascade;

alter table autorisation
   add constraint fk_autorisa_associati_groupe foreign key (groupeid)
      references groupe (groupeid)
      on delete restrict on update cascade;

alter table operation
   add constraint fk_operatio_associati_users foreign key (userid)
      references users (userid)
      on delete restrict on update cascade;

alter table operation
   add constraint fk_operatio_associati_categori foreign key (idcategorie)
      references categorie (idcategorie)
      on delete restrict on update cascade;

alter table users
   add constraint fk_users_associati_groupe foreign key (groupeid)
      references groupe (groupeid)
      on delete restrict on update cascade;

COMMIT;