if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_wp_debitsubagent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_wp_debitsubagent]
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE spu_wp_debitsubagent	@PartyCnt INT,
						@InsuranceFileCnt INT,
						@ClaimCnt INT,
						@DocumentRef VARCHAR(25),
						@Instance1 INT,
						@Instance2 INT,
						@Instance3 INT
AS

--DC120203 -start -document ref can now have a shared premium indicator on the end, so remove it
DECLARE @SharedIndicator int

SELECT @SharedIndicator = CHARINDEX('|',@DocumentRef)

If @SharedIndicator <> 0
BEGIN
	SELECT @DocumentRef = SUBSTRING(@DocumentRef, 1, @SharedIndicator -1)
END
--DC120203 -end

	DECLARE	@comm_value numeric(19,4)

	select 	@comm_value = sum(ted.transaction_amount)
	from	transaction_export_detail ted
	JOIN	transaction_export_folder tef
	on 	tef.transaction_export_folder_cnt = ted.transaction_export_folder_cnt
	WHERE	tef.document_ref = @DocumentRef
	and 	ted.spare IN ('BROK','COMM')

	SELECT 'comm_value' = @comm_value



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

