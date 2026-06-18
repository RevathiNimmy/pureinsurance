SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Cheque'
GO


CREATE PROCEDURE spu_ACT_Update_Cheque
    @cheque_id int,
    @media_ref varchar(100)
AS

DECLARE @ReturnValue int

SELECT @ReturnValue = 0

IF @media_ref = '' 
    Set @media_ref= NULL

Begin Transaction

UPDATE Cheque
SET media_ref =  @media_ref
WHERE cheque_id=@cheque_id

IF @@ERROR <> 0
	GOTO Error_Routine

UPDATE Transdetail
SET    spare = @media_ref
FROM  Transdetail td Join Cheque c ON td.transdetail_id = c.transdetail_id  
WHERE c.cheque_id = @cheque_id

IF @@ERROR <> 0
	GOTO Error_Routine

UPDATE CashListItem
SET    media_ref = @media_ref
FROM  CashListItem
JOIN Transdetail td ON td.transdetail_id=CashListItem.transdetail_id
JOIN Cheque c ON c.transdetail_id = td.transdetail_id
WHERE c.cheque_id = @cheque_id

IF @@ERROR <> 0
	GOTO Error_Routine

Commit Transaction
GOTO End_Routine

Error_Routine:
	RollBack Transaction
	SELECT @ReturnValue = -1
	
End_Routine:
	Return @ReturnValue
GO


