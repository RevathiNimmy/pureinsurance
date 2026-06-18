SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_Premium_Payment_Status'
GO

CREATE PROCEDURE spu_get_Premium_Payment_Status
    @claim_number varchar(30)
AS

/********************************************************************************************************/
/* Revision         Date            Who         Description of Modification                             */
/* --------         ----            ---         ---------------------------                             */
/*                  10/10/2001      JMK         Created                                                 */
/* 1.0              30/10/2001      JMK         Amended  for test  release                              */
/* 1.1              05/11/2001      JMK         rsa_transfer!                                           */
/********************************************************************************************************/
DECLARE  @PaymentType varchar(15),
         @PaidUpToDate int,
         @PolicyNum varchar(20),
		 @ClaimDate datetime,
		 @DefaultInstalment  int,
		 @AccountID int

-- put code here for finding out if premium payment is by instalments or not
SELECT @PaymentType = 'Single Payment'

IF @PaymentType = 'Instalments'
BEGIN
    -- Are payments up to date?
    SELECT @PaidUpToDate = 0
END
ELSE
BEGIN
   SELECT @PolicyNum = Policy_Number,
          @ClaimDate = Loss_From_Date
    FROM  Claim(NOLOCK)
	WHERE Claim_Number = @claim_number

 -- Has premium been fully paid?
		SELECT @PaidUpToDate =t.fully_matched,
			   @AccountID = a.Account_id
         FROM transdetail(NOLOCK) t
		INNER JOIN Account(NOLOCK) a
			ON a.account_id = t.account_id
		INNER JOIN insurance_file(NOLOCK) insf
			ON insf.insurance_file_cnt =(SELECT MAX(insurance_file_cnt)
			                               FROM insurance_file
										  WHERE insurance_ref =@PolicyNum)
		INNER JOIN party(NOLOCK) pty
			ON pty.party_cnt = insf.insured_cnt
		INNER JOIN Document(NOLOCK) d
			ON d.document_id  =t.document_id
		WHERE t.insurance_ref = @PolicyNum
			  AND t.fully_matched in (0)
			  AND d.documenttype_id in (4,15,17,37,42,44,46,52)
			  AND t.outstanding_amount <> 0

	-- PN75186
  SELECT @DefaultInstalment = Count(*)
   FROM  PFInstalments(NOLOCK) PFIns
    JOIN PFPremiumFinance(NOLOCK) PFPrem
	  ON PFIns.pfprem_finance_cnt = PFPrem.pfprem_finance_cnt
	JOIN Insurance_File(NOLOCK) Inf
	  ON Inf.insurance_file_cnt = PFPrem.Insurance_File_cnt
   WHERE PFIns.InstalmentNumber <> 0
     AND PFIns.Status <> 3
	 AND PFIns.DueDate<= @ClaimDate
	 AND Inf.insurance_ref = @PolicyNum
	 AND PFPrem.StatusInd IN ('040','900')
END
-- output
SELECT @PaymentType As PaymentType, IsNull(@PaidUpToDate, 1) As PaidUpToDate, @PolicyNum As PolicyNum, @ClaimDate As ClaimDate, ISNULL(@DefaultInstalment,0)As DefaultInstalmentCnt
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

