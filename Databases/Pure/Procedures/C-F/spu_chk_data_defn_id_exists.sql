SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_chk_data_defn_id_exists'
GO


CREATE PROCEDURE spu_chk_data_defn_id_exists
    @data_defn_id int,
    @Mode bit
AS

--*******************************************************************************************
-- Version      Author  Date        Desc
-- 1.00.0001    TN      27/04/2001  process data from work tables for underwriting
--
--*******************************************************************************************
DECLARE @AgentUnderwriter varchar(1)

SELECT  @AgentUnderwriter = value
FROM    hidden_options
WHERE   branch_id = 1 and option_number = 1

IF @AgentUnderwriter is null
    SELECT @AgentUnderwriter = "A"

IF @AgentUnderwriter = ""
    SELECT @AgentUnderwriter = "A"

IF @AgentUnderwriter = "A"
    if @Mode=0
        --SELECT claim_user_defined_risk_data_id
        --Changed the Claim_user_defined_risk_data table
        --Primary key column to claim_user_def_risk_data_id
        SELECT claim_user_def_risk_data_id
        FROM Claim_user_defined_risk_data
        WHERE (risk_data_defn_id = @data_defn_id)
    Else
        SELECT user_defined_peril_data_id
        FROM user_defined_peril_data
        WHERE (peril_data_defn_id = @data_defn_id)

ELSE
    IF @Mode = 0
        SELECT  claim_user_def_risk_data_id
        FROM    Claim_user_defined_risk_data
        WHERE   risk_data_defn_id = @data_defn_id
    ELSE
        SELECT  user_defined_peril_data_id
        FROM    user_defined_peril_data
        WHERE   peril_data_defn_id = @data_defn_id
GO


