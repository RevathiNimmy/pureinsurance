SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Update_Mandatory_Risk_Details'
GO

CREATE PROCEDURE spu_SIR_Update_Mandatory_Risk_Details  
	@risk_cnt INT,
    @risk_status_id TINYINT,  
    @description VARCHAR(50),  
    @variation_number TINYINT
AS  

UPDATE Risk
	SET risk_status_id = @risk_status_id,
	[description] = @description,
	variation_number = @variation_number
WHERE risk_cnt = @risk_cnt

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO