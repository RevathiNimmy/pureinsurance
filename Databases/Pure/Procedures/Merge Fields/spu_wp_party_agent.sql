SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_party_agent
GO

CREATE PROCEDURE spu_wp_party_agent
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @ClaimCnt INT,  
    @agent_type VARCHAR(255) OUTPUT,  
    @agent_origin VARCHAR(255) OUTPUT,  
    @agency_agreement_date DATETIME OUTPUT,  
    @agency_next_review_date DATETIME OUTPUT,  
    @agency_account_number VARCHAR(255) OUTPUT,  
    @is_branch TINYINT OUTPUT,  
    @is_head_office TINYINT OUTPUT,  
    @default_commission_percent NUMERIC(19,4) OUTPUT,  
    @trading_name VARCHAR(255) OUTPUT,  
    @binder_indicator INT OUTPUT,  
    @report_indicator INT OUTPUT,  
    @witholding_tax NUMERIC(19,4) OUTPUT,
    @Title VARCHAR(255) OUTPUT,
    @Contact_Perosn VARCHAR(255) OUTPUT,
    @First_Name VARCHAR(255) OUTPUT
AS
BEGIN  
  
    SELECT  
        @agent_type = pat.description,  
        @agent_origin = pao.description,  
        @agency_agreement_date = pa.agency_agreement_date,  
        @agency_next_review_date = pa.agency_next_review_date,  
        @agency_account_number = pa.agency_account_number,  
        @is_branch = pa.is_branch,  
        @is_head_office = pa.is_head_office,  
        @default_commission_percent = pa.default_commission_percent,  
        @trading_name = pa.trading_name,  
        @binder_indicator = pa.binder_indicator,  
        @report_indicator = pa.report_indicator,
        @Title = pa.Title,
        @Contact_Perosn = pa.Contact_Person,
        @First_Name = pa.First_Name
    FROM
        party_agent pa
        LEFT OUTER JOIN  party_agent_type pat
            ON pa.party_agent_type_id = pat.party_agent_type_id
        LEFT OUTER JOIN party_agent_origin pao  
            ON pa.party_agent_origin_id = pao.party_agent_origin_id  
    WHERE 
        pa.party_cnt = @PartyCnt 

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
