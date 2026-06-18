SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_chk_risk_disp_ord_exists'
GO


CREATE PROCEDURE spu_chk_risk_disp_ord_exists
    @display_order int,
    @Type_id int
AS

/*
SP name:    sp_chk_risk_disp_ord_exists
Desc:       Used by Define Feilds Screen
        To find if a Caption being passed
    Exists in the risk_data_definition, before we attempt to Add it
Author:     SK
Date:       17/09/2000
*/
SELECT display_order
FROM Risk_data_definition
WHERE (display_order = @display_order) AND (Risk_type_id = @Type_id)
GO


