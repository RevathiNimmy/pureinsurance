SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Ri_Arrangement_RiOverrideReasonId_upd'
GO
CREATE PROCEDURE spu_Ri_Arrangement_RiOverrideReasonId_upd
    @RiArrangementId integer,
    @RiOverrideReasonId integer
    
AS

UPDATE RI_Arrangement 
SET ri_override_reason_id =@RiOverrideReasonId
WHERE ri_arrangement_id =@RiArrangementId 
  
 GO