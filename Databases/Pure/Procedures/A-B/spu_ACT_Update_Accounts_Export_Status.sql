SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_Update_Accounts_Export_Status'
GO


CREATE PROCEDURE spu_ACT_Update_Accounts_Export_Status
    @exportfoldercnt int,
    @statuscode char(1)
AS
update 
transaction_export_folder
set 
accounts_export_status = @statuscode
where
transaction_export_folder_cnt = @exportfoldercnt
GO
