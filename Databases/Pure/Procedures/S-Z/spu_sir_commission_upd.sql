SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_sir_commission_upd'
GO


CREATE PROCEDURE spu_sir_commission_upd
    @party_type_id int,
    @party_id int,
    @product_id int,
    @risk_type_id int,
    @transaction_type_id int,
    @commission_band_id int,
    @commission_grouping int,
    @Rate numeric(19,10),
    @is_value tinyint,
    @effective_date Datetime,
    @old_date Datetime,
    @tax_group_id int,
    @maximum_rate numeric(19,4) = null, 
    @commission_level_id int = null,
	@UserId INT,
	@UniqueId VARCHAR(50)
AS  

/********************************************************************************************************/
/* Stored Procedure spu_sir_commission_upd                                   							*/
/********************************************************************************************************/
/* Revision             Description of Modification                                     Date        Who */
/* --------             ---------------------------                                     ----        --- */
/* 1.0                  SR11092000 - Created                                							*/
/* 2.0					Nexus change - to prevent user from changing SSP commission		08/02/08    RDT	*/
/********************************************************************************************************/
DECLARE @party_type_description NVARCHAR(255)
DECLARE @party_shortname NVARCHAR(255)
DECLARE @product_code NVARCHAR(50)
DECLARE @risk_type_code NVARCHAR(50)
DECLARE @transaction_type_code NVARCHAR(50)
DECLARE @commission_band_code NVARCHAR(50)
DECLARE @commission_grouping_code NVARCHAR(50)
DECLARE @ScreenHierarchy NVARCHAR(500) 

SELECT 
    @party_type_description = COALESCE(PAT.Description, 'All'),
    @party_shortname = COALESCE(PT.Shortname, 'All'),
    @product_code = COALESCE(P.code, 'All'),
    @risk_type_code = COALESCE(RT.code, 'All'),
    @transaction_type_code = COALESCE(TT.code, 'All'),
    @commission_band_code = COALESCE(CB.code, 'All'),
    @commission_grouping_code = COALESCE(CG.code, 'All')
FROM 
    Commission_Arrangement
LEFT JOIN Party_Agent_Type PAT
    ON @party_type_id = PAT.party_agent_type_id
LEFT JOIN Party PT
    ON @party_id = PT.Party_cnt
LEFT JOIN Product P
    ON @product_id = P.Product_id
LEFT JOIN Risk_Type RT
    ON @risk_type_id = RT.Risk_type_id
LEFT JOIN Transaction_Type TT
    ON @transaction_type_id = TT.Transaction_type_id
LEFT JOIN Commission_Band CB
    ON @commission_band_id = CB.Commission_band_id
LEFT JOIN Commission_Grouping CG
    ON @commission_grouping = CG.Commission_grouping_id

SET @ScreenHierarchy = 'Commission Maintenance (' +
                        @party_type_description + ', ' +
                        @party_shortname + ', ' +
                        @product_code + ', ' +
                        @risk_type_code + ', ' +
                        @transaction_type_code + ', ' +
                        @commission_band_code + ', ' +
                        @commission_grouping_code + ')'


    Begin
    
    IF @commission_level_id = -1 or @commission_level_id = NULL
    SET @commission_level_id =0
    
    Update Commission_arrangement
        Set Rate = @Rate,
            Is_Value = @is_value,
            Effective_date = @effective_date,
	    	tax_group_id = @tax_group_id,
--Start - Renuka - (WPR64 Paralleling)
	    Maximum_rate=  @maximum_rate,
		UserId = @UserId,
		UniqueId = @UniqueId,
		ScreenHierarchy = @ScreenHierarchy
--End - Renuka - (WPR64 Paralleling)
	From Commission_arrangement ca
	left outer join Party_agent pa on pa.party_cnt = ca.party_cnt
        Where Party_type = @PArty_type_id
        And   ca.Party_cnt = @party_id
        And   ca.Product_id = @product_id
        And   ca.Risk_type_id = @risk_type_id
        And   ca.Transaction_type_id = @transaction_type_id
        And   ca.Commission_band_id = @commission_band_id
        And   ca.commission_grouping = @commission_grouping
        And   ca.Effective_date = @old_date
	And   IsNull(pa.is_ssp_subagent,0) <> 1
	And ca.commission_level_id = @commission_level_id
  
    End
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



