SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Cheque_Printed'
GO


CREATE PROCEDURE spu_ACT_Update_Cheque_Printed
    @cheque_id int,
    @printed_date datetime,
    @printed_by_user_id smallint
AS

	UPDATE Cheque
	SET printed_date =  @printed_date,
	printed_by_user_id = @printed_by_user_id
	WHERE cheque_id=@cheque_id

GO


