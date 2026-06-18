SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Rsk_Type_sel'
GO


CREATE PROCEDURE spu_Rsk_Type_sel
AS


SELECT risk_type_id, code, description, show_information_checklist
FROM Risk_Type
WHERE is_deleted=0
GO


