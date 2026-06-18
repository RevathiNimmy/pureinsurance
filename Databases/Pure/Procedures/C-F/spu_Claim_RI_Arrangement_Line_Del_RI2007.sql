SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Arrangement_Line_Del_RI2007'
GO

CREATE PROCEDURE spu_Claim_RI_Arrangement_Line_Del_RI2007
@claim_ri_arrangement_line_id INT  
  
AS  

 DECLARE @Type VARCHAR(2)  
  
 SELECT @Type = type
 FROM claim_ri_arrangement_line  
 WHERE claim_ri_arrangement_line_id = @claim_ri_arrangement_line_id  
  
 IF @Type <>'FX'
 	BEGIN
 	     Delete from Claim_RI_Arrangement_line_Broker_Participants where Claim_Ri_arrangement_line_id In  
 	     ( Select claim_Ri_arrangement_line_id From claim_ri_arrangement_line WHERE claim_ri_arrangement_line_id=@claim_ri_arrangement_line_id)  
 	
	     DELETE FROM claim_ri_arrangement_line  
	     WHERE claim_ri_arrangement_line_id = @claim_ri_arrangement_line_id  
	END
 ELSE IF @Type ='FX'  
 	BEGIN 
	     Delete from Claim_RI_Arrangement_line_Broker_Participants where Claim_Ri_arrangement_line_id In  
	     ( Select claim_Ri_arrangement_line_id From claim_ri_arrangement_line WHERE Grouping=@claim_ri_arrangement_line_id)  

	     DELETE FROM claim_ri_arrangement_line  
	     WHERE grouping = @claim_ri_arrangement_line_id  
	END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
