SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_ExpSer'
GO


CREATE PROCEDURE spu_Get_ExpSer
    @rsk_type_id int
AS


SELECT Expert_Service_Id, Description
FROM Expert_Service
WHERE Expert_Service_Id NOT IN
 (SELECT Expert_Service_Id
 FROM Risk_type_Expert_service
 WHERE (Risk_type_id = @rsk_type_id)) and is_deleted=0
GO


