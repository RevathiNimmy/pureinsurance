SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Relationship_add'
GO

CREATE PROCEDURE spe_Party_Relationship_add
    @party_cnt int,
    @relation_cnt int,
    @relationship_type_id smallint,
    @description varchar(255)
AS
BEGIN
INSERT INTO Party_Relationship (
    party_cnt ,
    relation_cnt ,
    relationship_type_id ,
    description )
VALUES (
    @party_cnt,
    @relation_cnt,
    @relationship_type_id,
    @description)
END

GO

