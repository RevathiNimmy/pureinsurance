SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_sir_commission_add'
GO

CREATE PROCEDURE spu_sir_commission_add
    @party_type_id int,
    @party_id int,
    @product_id int,
    @risk_type_id int,
    @transaction_type_id int,
    @commission_band_id int,
    @commission_grouping int,
    @effective_date datetime,
    @commission_rate numeric(19,10),
    @Is_Value tinyint,
    @tax_group_id int,
    @maximum_rate numeric(19,4) = null,
    @commission_level_id int = 0,
    @UserId INT,
    @UniqueId VARCHAR(50)
AS  

/********************************************************************************************************/
/* Stored Procedure spu_sir_commission_add                                   */
/********************************************************************************************************/
/* Revision             Description of Modification                                     Date        Who */
/* --------             ---------------------------                                     ----        --- */
/* 1.0                  SR11092000 - Created                                */
/* 1.1                  PB/CMG12072002 - Commission Grouping added          */
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

INSERT INTO Commission_arrangement (
    Party_type, 
    party_cnt, 
    product_id,
    risk_type_id, 
    transaction_type_id, 
    commission_band_id, 
    commission_grouping, 
    effective_date, 
    rate, 
    is_value, 
    is_Deleted, 
    tax_group_id,
    Maximum_rate,
    commission_level_id,
    UserId,
    UniqueId,
    ScreenHierarchy
)
VALUES (
    @party_type_id, 
    @party_id, 
    @product_id, 
    @risk_type_id,
    @transaction_type_id, 
    @commission_band_id, 
    @commission_grouping, 
    @effective_date, 
    @commission_rate, 
    @is_value,
    0, 
    @tax_group_id,
    @maximum_rate,
    @commission_level_id,
    @UserId,
    @UniqueId,
    @ScreenHierarchy
)

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
