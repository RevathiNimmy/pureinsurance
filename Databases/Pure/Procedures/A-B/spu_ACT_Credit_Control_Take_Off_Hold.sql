SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Credit_Control_Take_Off_Hold'
GO

CREATE PROCEDURE spu_ACT_Credit_Control_Take_Off_Hold
    @account_id INT
AS

BEGIN

-- this selects the item ids we need to update and the step ids to update with

    UPDATE Credit_Control_Item WITH (ROWLOCK)
       SET credit_control_step_id = ccs2.credit_control_step_id
      FROM Credit_Control_Item cci WITH (ROWLOCK)
INNER JOIN Credit_Control_Step ccs
        ON cci.credit_control_step_id = ccs.credit_control_step_id
INNER JOIN Credit_Control_Step ccs2
        ON ccs.credit_control_rule_id = ccs2.credit_control_rule_id
       AND ccs.off_hold_step = ccs2.step_number
     WHERE cci.account_id = @account_id
END


GO


