EXEC DDLDropProcedure spu_rptSSRSCommissionByClassOfBusiness
GO
CREATE PROCEDURE spu_rptSSRSCommissionByClassOfBusiness
	@dtStartDate				DATETIME
	,@dtEndDate					DATETIME
AS

BEGIN

	DECLARE @ReinsuranceTypeEnumFAC		INT 
	DECLARE @ReinsuranceTypeEnumQUO     INT 
	DECLARE @ReinsuranceTypeEnumCOM     INT
	DECLARE @ReinsuranceTypeEnumCOI     INT
	DECLARE @ReinsuranceTypeEnumXOL     INT
	DECLARE @ReinsuranceTypeEnum001     INT 
	DECLARE @ReinsuranceTypeEnum002     INT 
	DECLARE @ReinsuranceTypeEnum003     INT 
	DECLARE @ReinsuranceTypeEnumRET     INT
	DECLARE @ReinsuranceTypeEnumFAX     INT
	DECLARE @ReinsuranceTypeEnumFAP     INT 
	DECLARE @ReinsuranceTypeEnumCAT     INT

	--Proportional--
	SELECT @ReinsuranceTypeEnumFAC = reinsurance_type_id FROM Reinsurance_type WHERE CODE = 'FAC'
	SELECT @ReinsuranceTypeEnum001 = reinsurance_type_id FROM Reinsurance_type WHERE CODE = '001'
	SELECT @ReinsuranceTypeEnum002 = reinsurance_type_id FROM Reinsurance_type WHERE CODE = '002'
	SELECT @ReinsuranceTypeEnum003 = reinsurance_type_id FROM Reinsurance_type WHERE CODE = '003'
	SELECT @ReinsuranceTypeEnumQUO = reinsurance_type_id FROM Reinsurance_type WHERE CODE = 'QUO'
	SELECT @ReinsuranceTypeEnumFAP = reinsurance_type_id FROM Reinsurance_type WHERE CODE = 'FAP'

	--Non Proportional--
	SELECT @ReinsuranceTypeEnumXOL = reinsurance_type_id FROM Reinsurance_type WHERE CODE = 'XOL'
	SELECT @ReinsuranceTypeEnumCAT = reinsurance_type_id FROM Reinsurance_type WHERE CODE = 'CAT'
	SELECT @ReinsuranceTypeEnumFAX = reinsurance_type_id FROM Reinsurance_type WHERE CODE = 'FAX'

	SELECT		COB.description								AS 'Major Class'
				,PT.Description								AS 'Premium Class'
				,SUM(AgentCommission.Commission)			AS 'Commission Value'
				,SUM(FACCommission.Commission)				AS 'ProportionalRI'
				,SUM(TreatyCommission.commission)			AS 'Non ProportionalRI'
	FROM Insurance_File INSF
	LEFT JOIN Insurance_File_Risk_Link INRL on INSF.insurance_file_cnt = INRL.insurance_file_cnt
	LEFT JOIN RI_Arrangement RIA ON RIA.risk_cnt = INRL.risk_cnt

	LEFT JOIN (
				SELECT ISNULL(SUM(AC.commission_value),0) AS Commission, AC.insurance_file_cnt FROM Agent_Commission AC 
				GROUP BY AC.insurance_file_cnt
				) AgentCommission ON INSF.insurance_file_cnt = AgentCommission.insurance_file_cnt

	LEFT JOIN ( 
				SELECT ISNULL(SUM(RIAL.commission_value),0) AS Commission, RIAL.ri_arrangement_id FROM RI_Arrangement_Line RIAL 				
				INNER JOIN Treaty T ON RIAL.treaty_id = T.treaty_id
				WHERE T.reinsurance_type_id IN(@ReinsuranceTypeEnumFAC,@ReinsuranceTypeEnumFAP,@ReinsuranceTypeEnumFAX) 
				GROUP BY RIAL.ri_arrangement_id
				) FACCommission ON RIA.ri_arrangement_id = FACCommission.ri_arrangement_id

	LEFT JOIN (
				SELECT ISNULL(SUM(RIAL.commission_value),0) AS Commission, RIAL.ri_arrangement_id FROM RI_Arrangement_Line RIAL 				
				INNER JOIN Treaty T ON RIAL.treaty_id = T.treaty_id
				WHERE T.reinsurance_type_id IN(@ReinsuranceTypeEnum001,@ReinsuranceTypeEnum002,@ReinsuranceTypeEnum003,@ReinsuranceTypeEnumQUO) 
				GROUP BY RIAL.ri_arrangement_id
			  ) TreatyCommission ON RIA.ri_arrangement_id = TreatyCommission.ri_arrangement_id 

	INNER JOIN Peril PER ON PER.risk_cnt = INRL.risk_cnt
	INNER JOIN Peril_Type PT ON PT.peril_type_id = PER.Peril_type_id
	INNER JOIN Class_Of_Business COB ON COB.class_of_business_id = PT.class_of_business_id
	WHERE INSF.inception_Date BETWEEN @dtStartDate AND @dtEndDate
	GROUP BY COB.description, PT.Description
END