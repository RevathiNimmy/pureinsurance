Select DOC.insurance_file_cnt,SF.Insurance_Ref,DOC.Document_ID,Doc.Document_Ref,Spare,cast(TD.Amount as decimal(19,2)),SF.transaction_type_id,tt.[description],TD.accounting_Date    --- Find the Original Commission which should have been suspended
From Document  DOC
Inner JOIN Stats_Folder SF ON DOC.Document_Ref = SF.Document_Ref
inner JOIN TransDetail TD on TD.document_id = Doc.Document_ID
Inner JOIN Account ACCT on ACCT.account_id = TD.Account_ID
inner JOIN Ledger on Ledger.Ledger_ID = ACCT.Ledger_Id
inner JOIN Transaction_Type tt on SF.transaction_type_id = tt.transaction_type_id
Where Ledger.ledger_name = 'Sub Agent'
and cast(TD.Outstanding_Amount as INT) <> 0
and Doc.created_date > '2020-02-29'
--and SF.Insurance_ref Like 'HEPBUR00053366'
and DOC.Insurance_File_Cnt In
		(Select Distinct INF.Insurance_File_Cnt   --- Find All INC record missing Agent's Commission Suspense[Note, this does not include cancellation JN's)
				From PFPremiumFinance PFF
				Inner JOIN Insurance_File INF on INF.Insurance_File_Cnt = PFF.Insurance_File_Cnt
				Inner JOIN Document DOC on DOC.Insurance_File_Cnt = INF.Insurance_File_Cnt
				Inner JOIN TransDetail  TD on TD.Document_ID = DOC.Document_ID
				Inner JOIN Account ACCT on ACCT.Account_ID = TD.Account_ID

				Where LEFT(Document_ref, 3)  IN ('INC', 'IRC')
				and Doc.created_date > '2020-02-29'
				and pff.StatusInd <> '999'
				and TD.Document_ID NOT In 
				( Select Distinct(document_id) 
				From [dbo].[CashListItem_Instalments] CLINST
				Inner JOIN CashListitem CLI ON CLI.CashListitem_ID = CLINST.cashlistitem_id
				Inner JOIN TransDetail  TD  On TD.transdetail_id = CLI.transdetail_id)
				and Doc.Document_Ref NOT IN 


										(Select Distinct (Doc.Document_Ref)  ---   Find all records with Sub- Agents Commission Suspense
										From PFPremiumFinance PFF
										Inner JOIN Insurance_File INF on INF.Insurance_File_Cnt = PFF.Insurance_File_Cnt
										Inner JOIN Document DOC on DOC.Insurance_File_Cnt = INF.Insurance_File_Cnt
										Inner JOIN TransDetail  TD on TD.Document_ID = DOC.Document_ID
										Inner JOIN Account ACCT on ACCT.Account_ID = TD.Account_ID

										Where LEFT(Document_ref, 3)  IN ('INC', 'IRC')
										and Doc.created_date > '2020-02-29'
										and Acct.short_code = 'COLLACC'
										and TD.Document_ID NOT In 
										( Select Distinct(document_id) 
										From [dbo].[CashListItem_Instalments] CLINST
										Inner JOIN CashListitem CLI ON CLI.CashListitem_ID = CLINST.cashlistitem_id
										Inner JOIN TransDetail  TD  On TD.transdetail_id = CLI.transdetail_id)))

AND DOC.Insurance_File_Cnt NOT In (select insurance_file_cnt from PM058633_DataFix_Part1_log)

Order by SF.Insurance_Ref