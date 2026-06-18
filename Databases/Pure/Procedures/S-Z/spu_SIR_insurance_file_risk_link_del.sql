SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_insurance_file_risk_link_del'
GO


CREATE PROCEDURE spu_SIR_insurance_file_risk_link_del
    @insurance_file_cnt int
AS

DELETE FROM insurance_file_risk_link
    WHERE insurance_file_cnt = @insurance_file_cnt
    


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
