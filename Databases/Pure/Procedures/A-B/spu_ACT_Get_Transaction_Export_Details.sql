SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_ACT_Get_Transaction_Export_Details
GO

CREATE PROCEDURE spu_ACT_Get_Transaction_Export_Details
@agentcode VARCHAR(20) = NULL,
@transtypeid int = NULL
AS  
BEGIN
    SELECT
        TEF.transaction_export_folder_cnt Cnt,  
        TEF.insurance_holder_shortname ClientCode,  
        TEF.agent_shortname AgentCode,  
        TEF.insurance_ref PolicyNumber,  
        TEF.cover_start_date CoverStartDate,  
        PMU.username Operator,  
        SUM
           (
            CASE
            WHEN TED.spare = 'GROSS'
                THEN TED.transaction_amount
            ELSE
                0
            END
           ) GrossAmount,
        SUM
           (
            CASE
            WHEN TED.spare = 'COMM'
                THEN TED.transaction_amount
            ELSE
                0
            END
           ) Commission,
        SUM
           (
            CASE
            WHEN TED.spare = 'TAX' 
                THEN TED.transaction_amount
            ELSE
                0
            END
           ) Tax,
        TEF.currency_code Currency,  
        TEF.transaction_type_code TransactionType,  
        TEF.accounts_export_status ExportStatus  
    FROM
        transaction_export_folder TEF
        INNER JOIN PMUser PMU
            ON PMU.user_id = TEF.created_by_user_id  
        LEFT OUTER JOIN transaction_export_detail TED  
            ON TEF.transaction_export_folder_cnt = TED.transaction_export_folder_cnt  
    WHERE 
        UPPER(TEF.accounts_export_status) <> 'C'
        AND UPPER(TEF.accounts_export_status) <> 'F'
        AND (
             TEF.agent_shortname = @agentcode 
             OR @agentcode IS NULL
            )
        AND (
             TEF.transaction_type_id = @transtypeid 
             OR @transtypeid IS NULL
            )
    GROUP BY
        TEF.transaction_export_folder_cnt,
        TEF.insurance_holder_shortname,
        TEF.agent_shortname,
        TEF.insurance_ref,
        TEF.cover_start_date,
        PMU.username,
        TEF.currency_code,
        TEF.transaction_type_code,
        TEF.accounts_export_status
    ORDER BY
        TEF.transaction_export_folder_cnt

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO