SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Get_Progress_Status'
GO

Create Procedure spu_CLM_Get_Progress_Status
	@transaction_type varchar(25) ='',
	@mode int=1
AS
	IF @mode = 0 -- PN 7147, @mode<>1
	BEGIN
		Select Progress_status_id , Description 
		From Progress_Status
	END
	ELSE
	BEGIN
		IF @transaction_type = 'C_CO'
			Select Progress_status_id , Description 
			From Progress_Status
			Where  is_closed_check_status <> 1
			And is_deleted <> 1
		ELSE
			Select Progress_status_id , Description 
			From Progress_Status
			Where is_deleted <> 1
	END