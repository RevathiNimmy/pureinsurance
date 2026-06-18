SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_REN_update_renewal_status'
GO
CREATE procedure spu_REN_update_renewal_status
@RENIFileCnt int,
@Statuscode varchar(15)
As
Begin

Declare @Statusid int

SELECT @Statusid = renewal_status_type_id 
FROM renewal_status_type 
WHERE RTrim(code) = @Statuscode 

UPDATE renewal_status
SET renewal_status_type_id = @Statusid
WHERE renewal_insurance_file_cnt = @RENIFileCnt

End
Go