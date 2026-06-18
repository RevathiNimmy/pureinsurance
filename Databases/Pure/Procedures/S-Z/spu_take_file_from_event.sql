SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_take_file_from_event'
GO
 

CREATE PROCEDURE spu_take_file_from_event
    @event_cnt INT,
    @insurance_file_cnt INT 
AS
BEGIN

DECLARE	@Event_COB_Rating_section_id Integer,
	@Event_Premium_Excluding_Tax Numeric(19,4),
	@Event_Tax_applied Numeric(19,4),
	@Event_Premium_Including_Tax Numeric(19,4),	
	@Event_Tax_group_id integer,
	@Event_Commission_Cnt integer,
	@Event_Commission_Percentage Numeric(19,4),
	@Event_Commission_Charge Numeric(19,4),
	@Event_Commission_Net numeric(19,4),
	@Event_Commission_tax_applied Numeric(19,4),
	@Event_Commission_Payable Numeric(19,4),
	@Event_Commission_Tax_group_id integer,
	@Event_Is_minimum_brokerage Tinyint,
	@Event_Override_rate_table Tinyint,
	@Event_Base_Premium_Excluding_Tax Numeric(19,4),
	@Event_Base_Tax_Applied Numeric(19,4),
	@Event_Base_Premium_Including_Tax Numeric(19,4),
	@Event_Base_Commission_Charge Numeric(19,4),
	@Event_Base_Commission_Net Numeric(19,4),
	@Event_Base_Commission_Tax_Applied Numeric(19,4),
	@Event_Base_Commission_Payable Numeric(19,4)


DECLARE C_Event_Sections CURSOR FAST_FORWARD FOR 
	SELECT COB_Rating_Section_Id, 
	Premium_Excluding_Tax ,
	Tax_applied   ,
	Premium_Including_Tax,	
	Tax_group_id ,
	Commission_Cnt ,
	Commission_Percentage ,
	Commission_Charge ,
	Commission_Net ,
	Commission_tax_applied ,
	Commission_Payable  ,
	Commission_Tax_group_id  ,
	Is_minimum_brokerage ,
	Override_rate_table ,
	Base_Premium_Excluding_Tax  ,
	Base_Tax_Applied  ,
	Base_Premium_Including_Tax ,
	Base_Commission_Charge ,
	Base_Commission_Net  ,
	Base_Commission_Tax_Applied  ,
	Base_Commission_Payable 
	FROM Event_Insurance_COB_Section
	WHERE Insurance_File_cnt = @event_cnt

OPEN C_Event_Sections 

FETCH NEXT FROM C_Event_Sections INTO
	@Event_COB_Rating_section_id ,
	@Event_Premium_Excluding_Tax ,
	@Event_Tax_applied ,
	@Event_Premium_Including_Tax ,	
	@Event_Tax_group_id  ,
	@Event_Commission_Cnt  ,
	@Event_Commission_Percentage ,
	@Event_Commission_Charge ,
	@Event_Commission_Net ,
	@Event_Commission_tax_applied ,
	@Event_Commission_Payable ,
	@Event_Commission_Tax_group_id ,
	@Event_Is_minimum_brokerage ,
	@Event_Override_rate_table ,
	@Event_Base_Premium_Excluding_Tax ,
	@Event_Base_Tax_Applied ,
	@Event_Base_Premium_Including_Tax  ,
	@Event_Base_Commission_Charge ,
	@Event_Base_Commission_Net ,
	@Event_Base_Commission_Tax_Applied ,
	@Event_Base_Commission_Payable 

WHILE (@@FETCH_STATUS = 0)
BEGIN

	IF exists (SELECT * FROM Insurance_COB_Section 
		WHERE (COB_Rating_Section_Id = @Event_COB_Rating_Section_id
		OR COB_Rating_Section_Id IS NULL)
		AND Insurance_File_Cnt = @insurance_file_cnt)
  	BEGIN
		UPDATE  S
		SET S.premium_excluding_tax = S.premium_excluding_tax - ES.premium_excluding_tax,
		S.tax_applied = S.tax_applied - ES.tax_applied,
		S.premium_including_tax = S.premium_including_tax - ES.premium_including_tax,
		S.commission_net = S.commission_net - ES.commission_net,
		S.commission_tax_applied = S.commission_tax_applied - ES.commission_tax_applied,
		S.commission_payable = S.commission_payable - ES.commission_payable 
		FROM Insurance_COB_Section S
		JOIN Event_Insurance_COB_Section ES ON ISNULL(S.COB_Rating_Section_id,0) = ISNULL(ES.COB_Rating_Section_id,0)
		WHERE (ES.COB_Rating_Section_Id = @Event_COB_Rating_Section_id
			OR ES.COB_Rating_Section_Id IS NULL)
		AND ES.insurance_file_cnt = @event_cnt
		AND S.insurance_file_cnt =  @insurance_file_Cnt
  	END
	 
	FETCH NEXT FROM C_Event_Sections INTO
		@Event_COB_Rating_section_id ,
		@Event_Premium_Excluding_Tax ,
		@Event_Tax_applied ,
		@Event_Premium_Including_Tax ,	
		@Event_Tax_group_id  ,
		@Event_Commission_Cnt  ,
		@Event_Commission_Percentage ,
		@Event_Commission_Charge ,
		@Event_Commission_Net ,
		@Event_Commission_tax_applied ,
		@Event_Commission_Payable ,
		@Event_Commission_Tax_group_id ,
		@Event_Is_minimum_brokerage ,
		@Event_Override_rate_table ,
		@Event_Base_Premium_Excluding_Tax ,
		@Event_Base_Tax_Applied ,
		@Event_Base_Premium_Including_Tax  ,
		@Event_Base_Commission_Charge ,
		@Event_Base_Commission_Net ,
		@Event_Base_Commission_Tax_Applied ,
		@Event_Base_Commission_Payable 

END 

CLOSE C_Event_Sections
DEALLOCATE C_Event_Sections


exec spu_update_insurance_file_from_COB_Sections @insurance_file_cnt
END 

GO


