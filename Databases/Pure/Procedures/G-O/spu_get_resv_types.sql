SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_resv_types'
GO


CREATE PROCEDURE spu_get_resv_types
AS


SELECT Reserve_type_id, Name, Description, Include_in_Total, Is_Excess,is_indemnity,is_expense
FROM Reserve_type
GO


