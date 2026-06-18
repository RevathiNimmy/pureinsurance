SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_exp_servs_add'
GO


CREATE PROCEDURE spu_get_exp_servs_add
    @Risk_type_id int
AS


SELECT Expert_Service.Service_type_id, Expert_Service.Description
FROM Risk_type_Expert_service INNER JOIN
 Expert_Service ON
 Risk_type_Expert_service.Expert_Service_Id = Expert_Service.Expert_Service_Id
WHERE (Risk_type_Expert_service.Risk_type_id = @Risk_type_id)
GROUP BY Expert_Service.Service_type_id,
 Expert_Service.Description
GO


