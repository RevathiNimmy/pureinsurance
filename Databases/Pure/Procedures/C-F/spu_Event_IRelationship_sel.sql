SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Event_IRelationship_sel'
GO


CREATE PROCEDURE spu_Event_IRelationship_sel
    @insurance_file_cnt int
AS


SELECT
    insurance_file_cnt,
    relation_cnt,
    policy_relationship_type_id,
    description
FROM Event_Policy_Relationship
WHERE insurance_file_cnt = @insurance_file_cnt
GO


