SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_Relationship_upd'
GO

CREATE PROCEDURE spe_Party_Relationship_upd
    @party_cnt int,
    @relation_cnt int,
    @relationship_type_id smallint,
    @description varchar(255)
AS
BEGIN
UPDATE Party_Relationship
    SET
    relationship_type_id=@relationship_type_id,
    description=@description
WHERE party_cnt = @party_cnt AND relation_cnt = @relation_cnt

DECLARE @complementary_type_id AS INT

SELECT @complementary_type_id = complementary_type_id 
  FROM relationship_type 
 WHERE relationship_type_id = @relationship_type_id AND is_deleted = 0
   AND effective_date <=  CONVERT(DATE, GETDATE())

IF ISNULL(@complementary_type_id, 0) <> 0 
BEGIN
UPDATE Party_Relationship
    SET
    relationship_type_id=@complementary_type_id,
    description=@description
WHERE party_cnt = @relation_cnt  AND relation_cnt = @party_cnt
END
ELSE
BEGIN
UPDATE Party_Relationship
    SET
    relationship_type_id=@relationship_type_id,
    description=@description
WHERE party_cnt = @relation_cnt  AND relation_cnt = @party_cnt
END

END

GO

