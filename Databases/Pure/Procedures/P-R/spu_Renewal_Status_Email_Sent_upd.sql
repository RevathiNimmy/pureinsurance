SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXEC DDLDropProcedure 'spu_Renewal_Status_Email_Sent_upd'
GO

CREATE PROCEDURE spu_Renewal_Status_Email_Sent_upd
		@renewal_insurance_file_cnt INT
AS

UPDATE Renewal_Status
SET email_sent=1,
    email_sent_date=GETDATE()
WHERE renewal_insurance_file_cnt=@renewal_insurance_file_cnt

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
