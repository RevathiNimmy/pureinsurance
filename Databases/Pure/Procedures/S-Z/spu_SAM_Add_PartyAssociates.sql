SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SAM_Add_PartyAssociates
GO

CREATE PROCEDURE spu_SAM_Add_PartyAssociates
    @party_cnt INT,  
    @relation_cnt INT,   
    @relationship_type_id SMALLINT,  
    @description VARCHAR(255) 
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


DECLARE @complementary_type_id AS INT

SELECT @complementary_type_id = complementary_type_id 
  FROM relationship_type 
 WHERE relationship_type_id = @relationship_type_id AND is_deleted = 0
   AND effective_date <=  CONVERT(DATE, GETDATE())

IF ISNULL(@complementary_type_id, 0) <> 0 
BEGIN
INSERT INTO Party_Relationship (  
    party_cnt ,  
    relation_cnt ,  
    relationship_type_id ,  
    description )  
VALUES (  
    @relation_cnt,  
    @party_cnt,
    @complementary_type_id,  
    @description)
END
ELSE
BEGIN
INSERT INTO Party_Relationship (  
    party_cnt ,  
    relation_cnt ,  
    relationship_type_id ,  
    description )  
VALUES (  
    @relation_cnt,  
    @party_cnt,
    @relationship_type_id,  
    @description)
END

END


GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

