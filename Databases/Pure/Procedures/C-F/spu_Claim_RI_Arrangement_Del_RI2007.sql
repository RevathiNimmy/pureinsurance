SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Arrangement_Del_RI2007'
GO

CREATE PROCEDURE spu_Claim_RI_Arrangement_Del_RI2007
  @claim_id INT
  
AS  

 DELETE FROM Claim_RI_Arrangement_line_Broker_Participants
	WHERE claim_ri_arrangement_line_id IN(SELECT claim_ri_arrangement_line_id
									FROM Claim_RI_Arrangement_line WHERE 
									claim_id = @claim_id)
 
 DELETE FROM claim_ri_arrangement_line
	WHERE claim_id = @claim_id 
	
 DELETE FROM Claim_RI_Arrangement
	WHERE claim_id = @claim_id	

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO	 