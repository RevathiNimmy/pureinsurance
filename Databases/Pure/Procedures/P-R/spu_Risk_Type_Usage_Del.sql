SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Risk_Type_Usage_Del'
GO


CREATE PROCEDURE spu_Risk_Type_Usage_Del
    @risk_type_id int,
	@UserId int = NULL,
	@UniqueId varchar(50) = NUll,
	@ScreenHierarchy varchar(500) = NUll
AS

UPDATE rtu 
SET UserId=@UserId,UniqueId=@UniqueId, ScreenHierarchy = @ScreenHierarchy + '/Risk Group(' + 
    (SELECT RTRIM(LTRIM(rtg.Code)) 
     FROM risk_type_group rtg 
     WHERE rtg.risk_type_group_id = rtu.risk_type_group_id) + 
    ')'
FROM Risk_Type_Usage rtu
WHERE rtu.risk_type_id = @risk_type_id


DELETE FROM Risk_Type_Usage
WHERE   risk_type_id=@risk_type_id
GO


