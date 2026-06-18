SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Delete_Credit_Control_Item'
GO

CREATE PROCEDURE spu_ACT_Delete_Credit_Control_Item
	@nCredit_control_item_id INT,
	@bDelete_Permanent BIT = 0, 
	@nLetter_Sent TINYINT = 0  
AS

BEGIN

IF (@bDelete_Permanent = 0)
    UPDATE Credit_Control_Item WITH (ROWLOCK)
    SET is_deleted=1,letter_sent=@nLetter_Sent 
    WHERE credit_control_item_id = @nCredit_control_item_id
ELSE
    DELETE Credit_Control_Item WITH (ROWLOCK)
    WHERE credit_control_item_id = @nCredit_control_item_id
END
GO




