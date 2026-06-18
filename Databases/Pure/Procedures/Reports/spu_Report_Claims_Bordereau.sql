
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_Report_Claims_Bordereau'
GO

CREATE PROCEDURE
spu_Report_Claims_Bordereau
(
@branch_id int,
@Start_Date datetime,
@End_Date datetime,
@insurer_code varchar(20),
@IncludeCLR varchar(5)
)
AS

DECLARE @CLR int

IF @IncludeCLR='True'
	SELECT @CLR=29
ELSE
	SELECT @CLR=28

IF @insurer_code='ALL'
	SELECT @insurer_code=''
ELSE
	SELECT @Insurer_Code=ISNULL(@Insurer_Code,'')

IF @branch_id=0
	SELECT @branch_id=-1
ELSE
	SELECT @branch_id=ISNULL(@branch_id,-1)

SELECT
D.document_date AS TransactionDate,
TD.claim_ref AS ClaimNumber,
TD.insurance_ref AS PolicyNumber,
P.resolved_name AS ClientName,
C.loss_from_date AS LossDate,
D.document_ref AS TransactionNumber,
TD.Amount AS Amount,
PMU.username AS Operator,
D.company_id AS BranchId,
S.description AS BranchName,
P2.shortname AS InsurerCode,
P2.resolved_name AS InsurerName
FROM
Document D
INNER JOIN
TransDetail TD ON D.document_id=TD.document_id
INNER JOIN
PMUser PMU ON TD.operator_id=PMU.user_id
INNER JOIN
Account A ON TD.account_id=A.account_id
INNER JOIN
Party P2 ON A.account_key=P2.party_cnt
INNER JOIN
Source S ON D.company_id=S.source_id
INNER JOIN
Claim C ON D.claim_id=C.claim_id
INNER JOIN
Party P ON C.client_id=P.party_cnt
WHERE
D.documenttype_id IN (28, @CLR) AND
A.ledger_id=4 AND
(D.document_date BETWEEN @Start_Date AND @End_Date) AND
(@Insurer_Code='' OR P2.shortname=@Insurer_Code) AND
(@branch_id=-1 OR D.company_id=@branch_id)
ORDER BY
D.company_id, P2.shortname, D.document_date

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




