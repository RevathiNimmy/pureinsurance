SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Relationship_Type_update'
GO


CREATE PROCEDURE spu_Relationship_Type_update
    @relationship_type_id smallint,
    @caption_id int,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @complementary_type_id smallint,
    @party_relationship_group_id smallint
AS


/* Update the values */
UPDATE  Relationship_Type
SET caption_id = @caption_id,
    code = @code,
    description = @description,
    is_deleted = @is_deleted,
    effective_date = @effective_date,
    complementary_type_id = @complementary_type_id,
    party_relationship_group_id = @party_relationship_group_id
    WHERE   relationship_type_id = @relationship_type_id
GO



