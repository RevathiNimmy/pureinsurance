EXECUTE DDLDropProcedure 'spu_SAM_Delete_Risk_Reinsurance'
GO

CREATE PROCEDURE spu_SAM_Delete_Risk_Reinsurance
    @risk_cnt int
AS

DECLARE @RI2007Enabled int
SELECT @RI2007Enabled = ISNULL(VALUE,0) FROM hidden_options WHERE option_number = 88 --RI2007Enabled 

DELETE FROM RI_Arrangement_line_Broker_Participants WHERE Ri_arrangement_line_id In  
  (SELECT ri_arrangement_line_id FROM  
 ri_arrangement_line AS rial  
INNER JOIN ri_arrangement ria  
 ON rial.ri_arrangement_id = ria.ri_arrangement_id  
WHERE  ria.risk_cnt = @risk_cnt )  
  
DELETE ri_arrangement_line FROM
	ri_arrangement_line AS rial
INNER JOIN ri_arrangement ria
	ON rial.ri_arrangement_id = ria.ri_arrangement_id
WHERE
	ria.risk_cnt = @risk_cnt
AND ((rial.type NOT IN ('F','FX') AND @RI2007Enabled =1) OR @RI2007Enabled=0)

IF (@RI2007Enabled =1)
IF (NOT EXISTS(select * FROM ri_arrangement_line AS rial INNER JOIN ri_arrangement ria 
			ON rial.ri_arrangement_id = ria.ri_arrangement_id 
			WHERE ria.risk_cnt = @risk_cnt 
			AND rial.type IN ('F','FX')) AND @RI2007Enabled =1) OR @RI2007Enabled =0
BEGIN
		DELETE FROM
			ri_arrangement
		WHERE
			risk_cnt = @risk_cnt
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


