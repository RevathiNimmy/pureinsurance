SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

execute ddldropprocedure 'spu_wp_debitagent'
go

CREATE PROCEDURE spu_wp_debitagent
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskID INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS
--KN (CMG) 251102 Created sp which displays SubAgents Brokerage and Agents Brokerage - replaces sp_wp_debitsubagent for SubAgents Brokerage
	DECLARE	@SAcomm_value numeric(19,4),
		@Acomm_value numeric(19,4),
		@TaxGroup varchar(25),
		@TaxGroupCode varchar(10),
		@TaxGroupId int,
		@SAcomm_taxes numeric(19,4),
		@Acomm_taxes numeric(19,4)

	DECLARE @source_id integer
	SELECT @source_id = source_id FROM insurance_file WHERE insurance_file_cnt = @InsuranceFileCnt

	select 	@SAcomm_value = abs(sum(ted.transaction_amount)),
		@SAcomm_taxes = abs(sum(ted.taxes_total))
	from	transaction_export_detail ted
	JOIN	transaction_export_folder tef
	on 	tef.transaction_export_folder_cnt = ted.transaction_export_folder_cnt
	WHERE	tef.document_ref = @DocumentRef and tef.source_id = @source_id
	and 	ted.spare IN ('BROK','COMM')
	and	tef.accounts_export_status <> 'f'
	
	select 	@Acomm_value = sum(ted.transaction_amount),
		@Acomm_taxes = sum(ted.taxes_total)
	from	transaction_export_detail ted
	JOIN	transaction_export_folder tef
	on 	tef.transaction_export_folder_cnt = ted.transaction_export_folder_cnt
	WHERE	tef.document_ref = @DocumentRef and tef.source_id = @source_id
	and 	ted.spare = 'AGENT'
	and	tef.accounts_export_status <> 'f'

	select 	@TaxGroupId = max(isnull(TED.Tax_Group_Id,0))
	from	transaction_export_detail ted
	JOIN	transaction_export_folder tef
	on 	tef.transaction_export_folder_cnt = ted.transaction_export_folder_cnt
	WHERE	tef.document_ref = @DocumentRef and tef.source_id = @source_id
	and 	ted.spare IN ('COMM','BROK','AGENT')
	and	tef.accounts_export_status <> 'f'
	
	if isnull(@TaxGroupId,0) <> 0
	Begin
		select @TaxGroup = Description, @TaxGroupCode=Code 
		from Tax_Group 
		where Tax_Group_Id = @TaxGroupId
	End

	IF @SAcomm_value IS NULL
		SELECT @SAcomm_value = 0.00
	IF @Acomm_value IS NULL
		SELECT @Acomm_value = 0.00
	IF @Acomm_value <> 0 and @SAcomm_value <> 0
		SELECT @SAcomm_value = @SAcomm_value + @Acomm_value

	SELECT 	'SAcomm_value' = @SAcomm_value,
		'Acomm_value' = @Acomm_value,
		'TaxGroup' = @TaxGroup,
		'TaxGroupCode' = @taxGroupCode,
		'SAcomm_taxes' = @SAcomm_taxes,
		'Acomm_taxes' = @Acomm_taxes

GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO