SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Setup_RSA_ChartOfAcc'
GO


CREATE PROCEDURE spu_ACT_Setup_RSA_ChartOfAcc
    @sub_branch_id int
AS

/****************************************************************************************************/
/* Adds all the basic folders required for RSA to the Orion database. */
/****************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 02/05/2001 RWH */
/* 1.1 Include Sub Agent Ledger. 09/07/2001 RWH */
/* 1.2 Include Tax Expense 10/07/2001 RWH */
/* 1.3 Include Claims stuff. 18/07/2001 RWH */
/* 1.4 Add new folders for Other Party Payable & Receivable ledgers. 23/07/2001 RWH */
/****************************************************************************************************/
-- All calls pass:
-- required folder name
-- parent folder name
-- account type (totalling type)

DECLARE 
    @temp_mapping_id int,
    @temp_element_id int,
    @temp_node_id int,
    @agent_mapping_id int,
    @insurer_mapping_id int,
    @client_mapping_id int,
    @client_element_id int,
    @client_node_id int,
    @description varchar(255),
    @company_id int

-- PWF 31/07/2002 get main branch
SELECT @company_id = source_id
FROM   sub_branch
WHERE  sub_branch_id = @sub_branch_id

--************ Find current Agent Ledger and rename it. ********************

EXEC spu_ACT_Setup_Map_Ledger 'AG', 'Agents', @sub_branch_id

--***********************************************************************************

--RWH (09/07/01) Sort Sub Agent Ledger.
--************ Find current Sub Agent Ledger and rename it. ********************

EXEC spu_ACT_Setup_Map_Ledger 'UB', 'Sub Agents', @sub_branch_id

--***********************************************************************************

--************ Find current Insurer Ledger and rename it. ********************

EXEC spu_ACT_Setup_Map_Ledger 'IN', 'RI Payable', @sub_branch_id

--***********************************************************************************

--************ Find current Client/Sales Ledger and rename it. ********************

EXEC spu_ACT_Setup_Map_Ledger 'SA', 'Direct', @sub_branch_id

--***********************************************************************************

--************************************* Sales Ledger *************************************
-- Already modified Agent and Client ledgeres will be mapped into sub-directories of this.

-- Does folder already exist ?
SELECT @temp_mapping_id = 0

SELECT @temp_mapping_id = mapping_id
FROM   Mapping
WHERE  description =  'Sales Ledger'
AND    company_id = @company_id

IF @temp_mapping_id = 0
    exec spu_ACT_Setup_add_folder 'Sales Ledger', 'Current Assets', 1, @company_id
ELSE
BEGIN
    SELECT @temp_node_id = 0

    SELECT @temp_node_id = node_id
    FROM   StructureTree
    WHERE  mapping_id = @temp_mapping_id

    IF @temp_node_id = 0
    BEGIN
        DELETE Mapping
        WHERE mapping_id = @temp_mapping_id

        exec spu_ACT_Setup_add_folder 'Sales Ledger', 'Current Assets', 1, @company_id
    END
    ELSE
        exec spu_ACT_Setup_map_folder 'Sales Ledger', 'Current Assets', @company_id

END

--********************************************************************************************

--************************************* Client Ledger *************************************
-- Move under Sales Ledger.

EXEC spu_ACT_Setup_map_folder 'Direct', 'Sales Ledger', @company_id

--*******************************************************************************************

--************************************* Agent Ledger *************************************
-- Move under Sales Ledger.

EXEC spu_ACT_Setup_map_folder 'Agents', 'Sales Ledger', @company_id

--********************************************************************************************

--RWH (09/07/01) Sort Sub Agent Ledger.
--************************************* Sub Agent Ledger *************************************
-- Move under 'Current Liabilities'.

EXEC spu_ACT_Setup_map_folder 'Sub Agents', 'Current Liabilities', @company_id

--********************************************************************************************

--*************************************** Insurer Ledger **********************************
-- Move to correct location.

EXEC spu_ACT_Setup_map_folder 'RI Payable', 'Current Liabilities', @company_id

--********************************************************************************************

-- ********************************** Revenue ********************************************

EXEC spu_ACT_Setup_folder 'Gross Written Premium', 'Income', @company_id

--*******************************************************************************************

--*************************************** Tax **********************************************

EXEC spu_ACT_Setup_folder 'Tax', 'Current Liabilities', @company_id

--*******************************************************************************************

--********************************** Commission ******************************************

EXEC spu_ACT_Setup_folder 'Lead Commission', 'Expense', @company_id

--********************************************************************************************

--************************* Nominal Premium for Insurer Treaty *************************

EXEC spu_ACT_Setup_folder 'RI Treaty Premium', 'Expense', @company_id

--********************************************************************************************

--************************** Nominal Premium for Insurer Other *************************

EXEC spu_ACT_Setup_folder 'RI Other Premium', 'Expense', @company_id

--********************************************************************************************

--*********************** Nominal Commission for Insurer Treaty ***********************

EXEC spu_ACT_Setup_folder 'RI Treaty Commission', 'Income', @company_id

--********************************************************************************************

--************************** Nominal Premium for Insurer Other *************************

EXEC spu_ACT_Setup_folder 'RI Other Commission', 'Income', @company_id

--********************************************************************************************

--************************** Nominal Premium for Coinsurer *************************

EXEC spu_ACT_Setup_folder 'Coinsured Premium', 'Expense', @company_id

--********************************************************************************************

--************************** Nominal Commission for Coinsurer *************************

EXEC spu_ACT_Setup_folder 'Coinsured Commission', 'Income', @company_id

--********************************************************************************************

-- RWH(10/07/01)
--************************** Expense for taxes not applied to client *************************

EXEC spu_ACT_Setup_folder 'Tax Expense', 'Expense', @company_id

--********************************************************************************************

-- CLAIMS CLAIMS CLAIMS CLAIMS CLAIMS CLAIMS CLAIMS CLAIMS CLAIMS

-- LIABILITIES
--*************************** Total expected claims ***************************

EXEC spu_ACT_Setup_folder 'O/S Claims Adj', 'Current Liabilities', @company_id

--********************************************************************************************

--*************************** Default claims payable ***************************

EXEC spu_ACT_Setup_folder 'Claims Payable', 'Current Liabilities', @company_id

--********************************************************************************************

--*************************** Other Party Payable ***************************

EXEC spu_ACT_Setup_folder 'Other Party Payable', 'Current Liabilities', @company_id

--********************************************************************************************

-- ASSETS
--*********************** Default account for salvage receipts. ************************

EXEC spu_ACT_Setup_folder 'Claims Receivable', 'Sales Ledger', @company_id

--********************************************************************************************

-- INCOME
--************************** Reinsurer's (Treaty) share of claim *************************

EXEC spu_ACT_Setup_folder 'RI TTY Claims Rec.', 'Income', @company_id

--********************************************************************************************

--************************** Reinsurer's (Other) share of claim *************************

EXEC spu_ACT_Setup_folder 'RI Other Claims Rec.', 'Income', @company_id

--********************************************************************************************

--************************** Coinsurer's share of claim *************************

EXEC spu_ACT_Setup_folder 'CI Claims Rec.', 'Income', @company_id

--********************************************************************************************

--************************** Salvage receipts for claims *************************

EXEC spu_ACT_Setup_folder 'Claims Salvage Rec.', 'Income', @company_id

--********************************************************************************************

--************************** Third Party receipts for claims *************************

EXEC spu_ACT_Setup_folder 'Claims TP Rec.', 'Income', @company_id

--********************************************************************************************

--************************** Other Party Receivable *************************

EXEC spu_ACT_Setup_folder 'Other Party R''able', 'Sales Ledger', @company_id

--********************************************************************************************

-- EXPENSE
--************************** Total claim or reserve entered **************************

EXEC spu_ACT_Setup_folder 'Grs Claims Incurred', 'Expense', @company_id

--********************************************************************************************

--************************ Reinsurer (Treaty) share of salvage ************************

EXEC spu_ACT_Setup_folder 'RI TTY Salvage Rec.', 'Expense', @company_id

--********************************************************************************************

--************************ Reinsurer (Other) share of salvage ************************

EXEC spu_ACT_Setup_folder 'RI Other Salvage Rec', 'Expense', @company_id

--********************************************************************************************

--************************ Coinsurer share of salvage ************************

EXEC spu_ACT_Setup_folder 'CI Salvage Rec.', 'Expense', @company_id

--********************************************************************************************

--************************ Reinsurer (Treaty) share of TPR ************************

EXEC spu_ACT_Setup_folder 'RI TTY TP Rec.', 'Expense', @company_id

--********************************************************************************************

--************************ Reinsurer (Other) share of TPR ************************

EXEC spu_ACT_Setup_folder 'RI Other TP Rec.', 'Expense', @company_id

--********************************************************************************************

--************************ Coinsurer share of TPR ************************

EXEC spu_ACT_Setup_folder 'CI TP Rec.', 'Expense', @company_id

--********************************************************************************************

-- Set up Other party ledger mapping.

--************ Other Party Receivable. ********************

EXEC spu_ACT_Setup_Map_Ledger 'OR', 'Other Party R''able', @sub_branch_id

--***********************************************************************************

--************ Other Party Payable ********************

EXEC spu_ACT_Setup_Map_Ledger 'OP', 'Other Party Payable', @sub_branch_id

--***********************************************************************************
GO


