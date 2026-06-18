SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_RI_Arrangement_sel_bands'
GO

CREATE PROCEDURE spu_RI_Arrangement_sel_bands  
    @risk_cnt int  ,
    @RIversion_ID INT =1 
AS  
  
    DECLARE @RI2007Enabled INT
	SELECT @RI2007Enabled=ISNull(value,0) FROM hidden_options WHERE option_number=88

    Select Distinct
            rb.ri_band_id,
            rb.description
    From    ri_band rb
    Join    ri_arrangement ra
            On ra.ri_band_id = rb.ri_band_id
    Where   ra.risk_cnt = @risk_cnt  and ( (ra.version_id =@RIversion_ID AND  @RI2007Enabled=1 ) OR  @RIversion_ID =1)
    Order By
            rb.description

