SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PMB_Policy_Fees_Sel'
GO

CREATE PROCEDURE spu_PMB_Policy_Fees_Sel
    @insurance_file_cnt int,
    @transaction_code   char(10) = NULL

AS

BEGIN

Declare @LinkId int,
        @risk_group_id int,
        @DMCode varchar(50),
        @Sql varchar(4096),
        @TransId int

-- Create temporary working table
Create Table #STT11(
    [name] varchar(255),
    party_cnt int,
    fee_percentage numeric(7, 4),
    fee_amount numeric(9, 4),
    commission_percentage numeric(7, 4), 
    commission_amount numeric(9, 4), 
    is_mandatory tinyint, 
    is_fee tinyint, 
    active tinyint, 
    discount numeric(9, 4),
    oi_key varchar(255),
    scheme_id int,
    ipt varchar(255),
    display_on_quotes tinyint
    )

-- Get the risk group
Select @risk_group_id = risk_group_id From risk_code where risk_code_id =
    (Select risk_code_id from insurance_file where insurance_file_cnt = @insurance_file_cnt)

-- Get the gis_policy_link_id
select @LinkId = gis_policy_link_id from gis_policy_link where insurance_file_cnt = @insurance_file_cnt

-- Get the datamodel code
select @DMCode = LTrim(RTrim(Code)) from gis_data_model where gis_data_model_id =
    (Select gis_data_model_id from gis_policy_link where gis_policy_link_id = @LinkId)

-- Does the output_fees table exist for this datamodel
IF EXISTS (select NULL from sysobjects where name = @DmCode + '_output_fees' And XType = 'U')
BEGIN
    -- Get any fees from the output tables
    Select @Sql = 
    'Insert Into #STT11  
        ([name], party_cnt, fee_percentage, fee_amount,  commission_percentage, 
        commission_amount, is_mandatory, is_fee, active, discount, oi_key, scheme_id, ipt, display_on_quotes)
    
        Select f.short_name, p.party_cnt, ISNULL(f.fee_percentage, 0), ISNULL(f.fee_amount, 0),
        ISNULL(f.commission_percentage, 0), ISNULL(f.commission_amount, 0), 1,
        CASE WHEN pt.code = ''FE'' THEN 
            1
        ELSE 
            0
        END,
        1, ISNULL(f.fee_discount, 0), '''', NULL, '''', 1
        From ' + @DmCode + '_output_fees f 
        Join party p on f.short_name = p.shortname
        Join Party_Type pt ON p.party_type_id = pt.party_type_id
        Where ' + @DmCode + '_policy_binder_id = ' + cast(@LinkId as char(10))
    
--    select @sql
    Exec (@Sql)
END

IF @transaction_code IS NOT NULL
BEGIN
    -- Get the id for the transaction type 
    Select @TransId = transaction_type_id from transaction_type where LTrim(RTrim(code)) = @transaction_code
    
    -- Get any compulsory Back-office fees
    Insert Into #STT11
        ([name], party_cnt, fee_percentage, fee_amount,  commission_percentage, 
        commission_amount, is_mandatory, is_fee, active, discount, oi_key, scheme_id, ipt, display_on_quotes)

        Select p.name, fa.party_cnt, ISNULL(fa.fee_percentage, 0), ISNULL(fa.fee_amount, 0),
        ISNULL(fa.commission_percentage, 0), ISNULL(fa.commission_amount, 0), 
        (Select 1 From transaction_type tt
                Where transaction_type_id = fa.transaction_type_id And code = @transaction_code),
        CASE WHEN pt.code = 'FE' THEN 
            1
        ELSE 
            0
        END,
        0, 0, '' , fa.extra_scheme_id, '', fa.display_on_quotes
        from fee_amounts fa
        Join party p on fa.party_cnt = p.party_cnt
        Join Party_Type pt ON p.party_type_id = pt.party_type_id
        Where fa.risk_group_id = @risk_group_id And fa.transaction_type_id = @TransId
END

Select * from #STT11
Drop Table #STT11

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO