SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Delete_Cheque'
GO


CREATE PROCEDURE spu_ACT_Delete_Cheque
    @cheque_id int
AS


DELETE FROM Cheque
WHERE cheque_id = @cheque_id
GO


