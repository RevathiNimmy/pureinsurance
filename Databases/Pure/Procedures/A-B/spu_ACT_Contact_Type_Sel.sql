 SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
 GO
 EXECUTE DDLDropProcedure 'spu_ACT_Contact_Type_Sel'
 GO
 CREATE  PROCEDURE spu_ACT_Contact_Type_Sel
 @sContactType VARCHAR(200)
 AS    
SELECT * FROM Contact_Type c
WHERE code =@sContactType AND c.is_deleted = 0
GO