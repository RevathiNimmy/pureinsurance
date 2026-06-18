SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
execute ddldropprocedure 'spu_wp_LinePercentage'
go
CREATE  PROCEDURE spu_wp_LinePercentage 
	 @PartyCnt INT,
     @InsuranceFileCnt INT,
     @RiskID INT,
     @ClaimCnt INT,
     @DocumentRef VARCHAR(25),
     @Instance1 INT,
     @Instance2 INT,
     @Instance3 INT
AS

DECLARE
    
    @total_written_percentage numeric(19,4),
    @total_signed_percentage numeric(19,4),
	@event_insurance_file_cnt INT

    SELECT
     @event_insurance_file_cnt = EIF.insurance_file_cnt
    FROM
    transaction_export_folder TEF
    JOIN event_log EL
    ON EL.event_cnt=TEF.event_log_id
    JOIN event_insurance_file EIF
    ON EIF.insurance_folder_cnt=EL.event_cnt
    WHERE TEF.document_ref = @DocumentRef
    AND TEF.accounts_export_status='c'
    AND TEF.source_id = (SELECT source_id FROM insurance_file WHERE insurance_file_cnt = @InsuranceFileCnt)

SELECT
    @total_written_percentage = ROUND(sum(epc.written_line_percentage),2),
    @total_signed_percentage = ROUND(sum(epc.signed_line_percentage),2)
    
FROM event_policy_coinsurers epc
WHERE epc.insurance_file_cnt = @event_insurance_file_cnt

SELECT
	'total_written_percentage' = @total_written_percentage,
    'total_signed_percentage' = @total_signed_percentage
    
    
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

