SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Credit_Control_Item_Update_MTC'
GO

CREATE PROCEDURE spu_ACT_Credit_Control_Item_Update_MTC
@insurance_file_cnt INT  
AS

DECLARE @insurance_ref VARCHAR(50)

SELECT @insurance_ref= insurance_ref FROM insurance_file WHERE insurance_file_cnt=@insurance_file_cnt

BEGIN
UPDATE Credit_Control_Item SET  is_deleted=1 
            FROM Credit_Control_Item cc 
            INNER JOIN insurance_file i ON i.insurance_file_cnt=cc.insurance_file_cnt
            WHERE i.insurance_ref=@insurance_ref
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
