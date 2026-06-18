SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ChckDelForPrlRskTyp'
GO

CREATE PROCEDURE spu_ChckDelForPrlRskTyp    
    @PerilType integer,    
    @ReserveType integer    
AS    

BEGIN
  
--*******************************************************************************************    
-- Version      Author  Date        Desc    
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting    
--*******************************************************************************************    
DECLARE @ReseveCount integer    
DECLARE @CanDelete  boolean    
    
	BEGIN    
		SELECT  @ReseveCount =  count(claim_peril.claim_peril_id)    
		FROM    claim_peril, reserve    
		WHERE   claim_peril.claim_peril_id = reserve.claim_peril_id    
		AND     Peril_type_id = @PerilType    
		AND     Reserve_type_id = @ReserveType    
	
		IF @ReseveCount > 0    
		    	SELECT @CanDelete = 0    
		ELSE    
			SELECT @CanDelete = 1    
	
		SELECT @CanDelete    
	END    
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
