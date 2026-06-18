SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Risk_Tax_Cal_Upd'
GO

 CREATE PROCEDURE spu_Risk_Tax_Cal_Upd
    @oldRisk_cnt int,
	@newRisk_cnt int,
    @insurance_File_cnt int
AS
begin
    -- Should we delete or update this record?
  
        UPDATE  Tax_Calculation
        SET     risk_cnt = @newRisk_cnt              
        WHERE   risk_cnt = @oldRisk_cnt and insurance_file_cnt= @insurance_File_cnt
end


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
