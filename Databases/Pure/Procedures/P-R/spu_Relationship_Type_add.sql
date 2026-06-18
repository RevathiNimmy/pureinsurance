SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Relationship_Type_add'
GO


CREATE PROCEDURE spu_Relationship_Type_add
    @relationship_type_id smallint,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @complementary_type_id smallint,
    @party_relationship_group_id smallint
AS


/* Insert the values */
INSERT INTO Relationship_Type
( relationship_type_id, caption_id, code, description, is_deleted, effective_date, complementary_type_id, party_relationship_group_id )
VALUES
( @relationship_type_id, @caption_id, @code, @description, @is_deleted, @effective_date, @complementary_type_id, @party_relationship_group_id )
GO



