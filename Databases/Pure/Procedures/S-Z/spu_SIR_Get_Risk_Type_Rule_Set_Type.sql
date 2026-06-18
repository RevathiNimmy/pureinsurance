SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Risk_Type_Rule_Set_Type'
GO

CREATE PROCEDURE spu_SIR_Get_Risk_Type_Rule_Set_Type

@UserID     int,
@ProductID  int,
@Transaction_Type_Code varchar(10)

AS
    SELECT risk_type_rule_set_type_id FROM Rule_Set WHERE  Rule_Set_Id
        IN
        (SELECT uarl.Rule_Set_Id FROM PMUser_Authority_Rule_Set_Link uarl,  Transaction_Type tt
         WHERE uarl.Authority_Level_Type_Id
                    IN
                    (SELECT Authority_Level_Type_Id FROM PMUser_Authority_Level   WHERE User_Id = @UserID And Product_ID=  @ProductID)


    	AND uarl.Transaction_type_id = tt.Transaction_type_id AND tt.code =  @Transaction_Type_Code)
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

