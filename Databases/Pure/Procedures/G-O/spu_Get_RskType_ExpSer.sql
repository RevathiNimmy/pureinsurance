SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_RskType_ExpSer'
GO


CREATE PROCEDURE spu_Get_RskType_ExpSer
    @rsk_type_id int
AS


SELECT Risk_type_Expert_service.Risk_type_expert_service_id,
 Expert_Service.Expert_Service_Id,
 Expert_Service.description
FROM Risk_type_Expert_service INNER JOIN
 Expert_Service ON
 Risk_type_Expert_service.Expert_Service_Id = Expert_Service.Expert_Service_Id
WHERE (Risk_type_Expert_service.Risk_type_id = @rsk_type_id)
GO


