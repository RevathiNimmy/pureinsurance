SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_reserve_type'
GO

CREATE PROCEDURE spu_get_reserve_type  
    @periltypeid numeric,  
    @perilid numeric  
AS  
  

BEGIN  
	If @periltypeid=0  
	BEGIN
	    	SELECT @periltypeid = peril_type_id 
		FROM claim_peril 
		WHERE claim_peril_id = @perilid  
	END

	SELECT 
		reserve_type_id,
		description,
		include_in_total 
	FROM reserve_type 
	WHERE reserve_type_id IN  
		(SELECT reserve_type_id 
		 FROM peril_type_reserve_type 
		 WHERE peril_type_id =@periltypeid)  
END  
	


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
