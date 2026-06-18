SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_chk_risk_caption_exists'
GO


CREATE PROCEDURE spu_chk_risk_caption_exists
    @Caption varchar(50),
    @Type_id int,
    @iType int
AS

/*SP name:  sp_chk_risk_caption_exists
Desc:       Used by Define Feilds Screen
        To find if a Caption being passed
        Exists in the risk_data_definition,
        before we attempt to Add it
Author:     SK
Date:       17/09/2000
Updated:    11/06/2000*/
DECLARE @sVar varchar(250)

select @sVar ="SELECT Caption
FROM Risk_data_definition
WHERE Caption = '" + convert(varchar(50),@Caption) + "' AND Risk_type_id = " + convert(varchar(50),@Type_id) + " AND "

IF @iType=6
select @sVar =@sVar + "TYPE=6"
ELSE
select @sVar =@sVar + "TYPE<>6"

BEGIN
Execute (@Svar)
END
GO


