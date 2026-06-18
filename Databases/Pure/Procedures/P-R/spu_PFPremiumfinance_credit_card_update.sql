SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_PFPremiumfinance_credit_card_update'
GO

CREATE PROCEDURE spu_PFPremiumfinance_credit_card_update 
@nInsuranceFileCnt INT

AS
DECLARE @nOldInsuranceFileCnt INT,
@CCNumber varchar(30),
@ExpiryDate varchar(10),
@CardStartDate varchar(10),
@Issue varchar(2),
@ccPin varchar(20),
@is_cardholder tinyint,
@cardholder_name varchar(100),
@cardholder_address1 varchar(100),
@cardholder_address2 varchar(100),
@cardholder_address3 varchar(100),
@cardholder_address4 varchar(100),
@cardholder_postcode varchar(100),
@card_type varchar(50),
@Party_Bank_Id INT ,
@currentPfPrem_financeCnt INT

DECLARE @kPFStatusIndLive CHAR(3) = '040'
DECLARE @kPFStatusIndOnHold  CHAR(3) = '140'
DECLARE @kPFStatusIndCompleted CHAR(3) = '900'
DECLARE @kPFStatusIndSuperseded CHAR(3) = '990'
DECLARE @kPFStatusIndCancelled CHAR(3) = '999'

SELECT @nOldInsuranceFileCnt = MAX(pfp.Insurance_File_Cnt)
					FROM pfpremiumfinance pfp INNER JOIN insurance_file i ON i.insurance_file_cnt=pfp.Insurance_File_Cnt
					INNER JOIN insurance_file inf ON inf.insurance_folder_cnt=i.insurance_folder_cnt
					WHERE inf.insurance_file_cnt=@nInsuranceFileCnt AND i.insurance_file_cnt<>@nInsuranceFileCnt 
					AND pfp.StatusInd IN (@kPFStatusIndLive,@kPFStatusIndOnHold,@kPFStatusIndCompleted,@kPFStatusIndSuperseded,@kPFStatusIndCancelled)

SELECT @currentPfPrem_financeCnt = MAX(pfprem_finance_cnt) FROM pfpremiumfinance
					WHERE insurance_file_cnt=@nOldInsuranceFileCnt  
					AND StatusInd IN (@kPFStatusIndLive,@kPFStatusIndOnHold,@kPFStatusIndCompleted,@kPFStatusIndSuperseded,@kPFStatusIndCancelled);


SELECT TOP 1 @CCNumber = cc_number,
			@ExpiryDate=cc_expiry_date,
			@CardStartDate=cc_start_date,
			@Issue=cc_issue,
			@ccPin=cc_pin,
			@is_cardholder=is_cardholder,
			@cardholder_name=cardholder_name,
			@cardholder_address1=cardholder_address1,
			@cardholder_address2=cardholder_address2,
			@cardholder_address3=cardholder_address3,
			@cardholder_address4=cardholder_address4,
			@cardholder_postcode=cardholder_postcode,
			@card_type=card_type,
			@Party_Bank_Id=party_bank_id
			FROM PFPremiumFinance WHERE Insurance_File_Cnt = @nOldInsuranceFileCnt AND pfprem_finance_cnt = @currentPfPrem_financeCnt
							   AND StatusInd IN (@kPFStatusIndLive,@kPFStatusIndOnHold,@kPFStatusIndCompleted,@kPFStatusIndSuperseded,@kPFStatusIndCancelled) ORDER BY pfprem_finance_version DESC

UPDATE PFPremiumFinance SET
		cc_number = @CCNumber,
		cc_expiry_date = @ExpiryDate,
		cc_start_date = @CardStartDate,
		cc_issue = @Issue,
		cc_pin = @ccPin,
		is_cardholder = @is_cardholder,
        cardholder_name = @cardholder_name,
        cardholder_address1 = @cardholder_address1,
        cardholder_address2 = @cardholder_address2,
        cardholder_address3 = @cardholder_address3,
        cardholder_address4 = @cardholder_address4,
        cardholder_postcode = @cardholder_postcode,
        card_type = @card_type,
		party_bank_id = @Party_Bank_Id WHERE Insurance_File_Cnt = @nInsuranceFileCnt


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

