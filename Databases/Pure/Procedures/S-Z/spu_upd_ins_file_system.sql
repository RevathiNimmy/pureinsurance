SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_upd_ins_file_system'
GO


CREATE PROCEDURE spu_upd_ins_file_system
    @insurance_file_cnt int,
    @transaction_type varchar(20),  
    @debitagainst smallint=NULL,       
    @paymentAccountId int=NULL,
    @lastTransDate datetime=NULL
AS

-- Start - PN 58743
IF ISNULL(@lastTransDate, 0)=0   
BEGIN  
	SET @lastTransDate = getdate()
END 
-- End - PN 58743

DECLARE @balance_type varchar(2)      
SELECT @balance_type=''      
IF   @debitagainst=1 SELECT @balance_type='OD'      
IF   @debitagainst=2 SELECT @balance_type='FB'      
--Start - Renuka - (WPR85_Cash_Deposit_Process)
IF   @debitagainst=4 SELECT @balance_type='CD'
--End - Renuka - (WPR85_Cash_Deposit_Process)
     
IF ISNULL(@paymentAccountId,0) <> 0 
BEGIN
Update Insurance_file SET intermediary_agent_account_id=@paymentAccountID,  
balance_type=@Balance_type WHERE insurance_file_cnt=@insurance_file_cnt  
END


DECLARE @transaction_type_id int
DECLARE @trans_type_description varchar(255)

SELECT  @transaction_type_id = transaction_type_id
FROM    transaction_type
WHERE   code = @transaction_type

Select @trans_type_description = last_trans_description 
FROM Insurance_File_System 
WHERE insurance_file_cnt = @insurance_file_cnt

if @trans_type_description is not null 
BEGIN
UPDATE  insurance_file_system
SET last_trans_date = @lastTransDate,  
    last_trans_type_id = @transaction_type_id,
	last_trans_description=@trans_type_description
WHERE   insurance_file_cnt = @insurance_file_cnt
END
ELSE
BEGIN

if @transaction_type_id is not null

UPDATE  insurance_file_system
SET last_trans_date = @lastTransDate,  
    last_trans_type_id = @transaction_type_id,
	 last_trans_description=case @transaction_type_id when 10 then 'Renewals' else last_trans_description end
WHERE   insurance_file_cnt = @insurance_file_cnt
END
GO


