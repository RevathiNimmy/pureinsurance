SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_void_transaction_log_add'
GO

CREATE PROCEDURE spu_void_transaction_log_add
@insurance_file_cnt int,
@user_id int
as


INSERT INTO VOID_REVERSE_TRANSACTION_LOG(void_insurance_file_cnt,user_id,reversal_date) VALUES (@insurance_file_cnt,@user_id,GetDate())

 SELECT SCOPE_IDENTITY() AS LastInsertedID;