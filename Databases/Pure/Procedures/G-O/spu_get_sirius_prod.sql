SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_sirius_prod'
GO


CREATE PROCEDURE spu_get_sirius_prod
AS

--SJP (13/06/2002) UW_Type selected by branch_id  = 1 and option_number = 1 
--to ensure unique record

SELECT Value
FROM Hidden_Options
WHERE branch_id = 1 and option_number = 1
GO


