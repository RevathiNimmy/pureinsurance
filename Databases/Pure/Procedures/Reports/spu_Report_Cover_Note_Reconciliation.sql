SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Report_Cover_Note_Reconciliation'
GO
CREATE PROCEDURE spu_Report_Cover_Note_Reconciliation
    @branch_id int,
    @product_code varchar(30),
    @agent_code varchar(30),
    @Sheet_status_id varchar(50),
    @group_by VARCHAR(30)
AS

DECLARE 
@iBranchID int,
@iProductID int,
@iAgentID int,
@iSheetStatusID int

SELECT @iBranchID = ISNULL(@branch_id, 0)

If (@product_code = 'ALL' or @product_code = '' or @product_code IS NULL)
    SELECT @iProductID = 0
Else
    SELECT TOP 1 @iProductID = product_id FROM product WHERE description like @product_code

If (@agent_code = 'ALL' or @agent_code = '' or @agent_code IS NULL)
    SELECT @iAgentID = 0
Else
    SELECT TOP 1 @iAgentID = party_cnt FROM Party WHERE shortname like @agent_code

If (@Sheet_status_id = 'ALL' or @Sheet_status_id = '' or @Sheet_status_id IS NULL)
    SELECT @iSheetStatusID = 0
Else
    SELECT TOP 1 @iSheetStatusID = cover_note_sheet_status_id FROM cover_note_sheet_status WHERE description like @Sheet_status_id

CREATE TABLE #tempCoverNote
(
    Book_Number           varchar (50)  NULL,
    Sheet_Number          int 		NULL,
    Sheet_status          varchar (50)  NULL,
    InsuranceRef          varchar (30)  NULL,
    Product_Code          varchar (20)  NULL,
    Branch_Code		  varchar (20)  NULL,
    Agent_Code            varchar (20)  NULL
)

INSERT INTO #tempCoverNote
    Select CNB.Book_Number,
	CNS.Cover_Sheet_Number,
	CNSS.description,
	ifi.insurance_ref,
	Prd.code 'Product Code',
	S.Code 'Branch Code',
	P.shortname
From Cover_Note_Sheet CNS
INNER JOIN Cover_Note_Book CNB ON CNB.Cover_Note_Book_Id = CNS.Cover_Note_Book_Id
INNER JOIN Cover_Note_Sheet_Status CNSS ON CNSS.Cover_Note_Sheet_Status_Id = CNS.Cover_Note_Sheet_Status_Id
LEFT JOIN Insurance_File IFI ON IFI.insurance_file_cnt = CNS.insurance_file_cnt
LEFT JOIN Product Prd ON PRD.Product_Id = IFI.Product_Id
LEFT JOIN Source S ON S.Source_Id = IFI.Source_Id 
LEFT JOIN Party P ON P.Party_Cnt = IFI.lead_agent_cnt
Where 
((IFI.Source_id = @iBranchID and @iBranchID <> 0) or (@iBranchID = 0))
AND
((IFI.Product_id = @iProductID and @iProductID <> 0) or (@iProductID = 0))
AND
((IFI.lead_agent_cnt = @iAgentID and @iAgentID <> 0) or (@iAgentID = 0))
AND
((CNS.cover_note_sheet_status_id = @iSheetStatusID and @iSheetStatusID <> 0) or (@iSheetStatusID = 0))
Order By CNB.Cover_Note_Book_Id

SELECT *,
    CASE @group_by
        WHEN 'Branch' THEN Branch_Code
        WHEN 'Product' THEN Product_Code
        WHEN 'Agent' THEN Agent_Code
        WHEN 'Cover Note Status' THEN Sheet_status
        ELSE ''
    END 'GroupByCode'
	FROM #tempCoverNote

DROP TABLE #tempCoverNote


GO

